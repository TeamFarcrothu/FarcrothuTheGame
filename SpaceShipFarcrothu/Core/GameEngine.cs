namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using System.Linq;
    using Handlers;
    using GameObjects;
    using Factories;
    using Interfaces;
    using Data;
    using Utils.Assets;
    using Utils.Enums;
    using Utils.Globals;

    public class GameEngine : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly StarField starfield;
        private readonly HUD hud;

        private SpriteBatch spriteBatch;
        private KeyboardState keyState;
        private VideoPlayer videoPlayer;
        private Texture2D texture;

        private Random random;
        private State gameState;

        private Player player;
        private Player player2;
        private readonly InputHandler inputHandler;

        private Boss boss;

        private bool bossHasInstance;
        private bool introPlayed;

        private GameDatabase db;

        public GameEngine()
        {
            this.starfield = new StarField();
            this.inputHandler = new InputHandler();
            this.graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = Globals.MAIN_SCREEN_WIDTH,
                PreferredBackBufferHeight = Globals.MAIN_SCREEN_HEIGHT
            };

            this.Window.Title = "Traveling to FARCROTHU";
            this.Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.db = new GameDatabase();
            this.gameState = State.Intro;
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.videoPlayer = new VideoPlayer();
            this.random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            TexturesManager.LoadContent(this.Content);
            SoundManager.LoadContent(this.Content);
            VideoManager.LoadContent(this.Content);

            this.starfield.LoadContent(this.Content);

            MediaPlayer.Play(SoundManager.IntroSong);
        }

        protected override void UnloadContent()
        {
            this.Content.Unload();
            this.Exit();
        }

        protected override void Update(GameTime gameTime)
        {
            this.keyState = Keyboard.GetState();

            if (this.keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (this.keyState.IsKeyDown(Keys.OemMinus) && MediaPlayer.Volume >= 0.02f)
            {
                MediaPlayer.Volume -= 0.02f;
            }

            if (this.keyState.IsKeyDown(Keys.OemPlus) && MediaPlayer.Volume <= 0.98f)
            {
                MediaPlayer.Volume += 0.02f;
            }

            if (this.keyState.IsKeyDown(Keys.Space) && this.videoPlayer.State == MediaState.Playing)
            {
                this.videoPlayer.Stop();
            }

            //UPDATING Game state
            switch (this.gameState)
            {
                case State.Intro:
                    this.GetIntroByStateIntro();
                    break;

                case State.Playing:
                    {
                        this.starfield.Speed = 3;

                        this.Play(gameTime);

                        //Clear dead players
                        this.db.Players.GetAll().RemoveAll(p => p.IsAlive == false);

                        //Set gameover
                        if (this.db.Players.GetAll().All(p => p.IsAlive == false))
                        {
                            this.gameState = State.GameOver;
                        }

                        //update players
                        this.db.Players.GetAll().ForEach(p => p.Update(gameTime));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.Move(p));
                        this.db.Players.GetAll().ForEach(p => p.InputHandler.PlayerShoot(this.db.Bullets, this.db.Players, p.Id));

                        this.starfield.Update(gameTime);
                        break;
                    }

                //UPDATING MENU STATE
                case State.Menu:
                    this.ChoosePlayerModeByStateMenu(gameTime);
                    break;

                //UPDATING GAMEOVER STATE or WINNING STATE
                case State.GameOver:
                case State.Winning:
                    this.keyState = Keyboard.GetState();

                    if (this.keyState.IsKeyDown(Keys.Space))
                    {
                        this.SetupNewGame();
                    }

                    this.starfield.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void GetIntroByStateIntro()
        {
            if (this.videoPlayer.State == MediaState.Stopped && this.introPlayed == false)
            {
                this.videoPlayer.Play(VideoManager.Video);
                this.introPlayed = true;
            }
            else if (this.videoPlayer.State == MediaState.Stopped && this.introPlayed == true)
            {
                this.gameState = State.Menu;
            }
        }

        private void ChoosePlayerModeByStateMenu(GameTime gameTime)
        {
            this.keyState = Keyboard.GetState();

            //Setup two players game
            if (this.keyState.IsKeyDown(Keys.D2))
            {
                this.player = new Player(new Vector2(600, 600), this.inputHandler, 1);
                this.db.Players.AddEntity(this.player);

                this.player2 = new Player(new Vector2(700, 600), this.inputHandler, 2);
                this.db.Players.AddEntity(this.player2);

                this.gameState = State.Playing;
                MediaPlayer.Play(SoundManager.BgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            //Setup single player game
            if (this.keyState.IsKeyDown(Keys.D1))
            {
                this.player = new Player(new Vector2(600, 600), this.inputHandler, 1);
                this.db.Players.AddEntity(this.player);

                this.gameState = State.Playing;
                MediaPlayer.Play(SoundManager.BgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            this.starfield.Update(gameTime);
            this.starfield.Speed = 1;
        }

        private void SetupNewGame()
        {
            MediaPlayer.Stop();

            this.db.Enemies.Dispose();
            this.db.Asteroids.Dispose();
            this.db.Explosions.Dispose();
            this.db.Players.Dispose();
            this.db.Bullets.Dispose();
            this.db.Items.Dispose();

            this.bossHasInstance = false;

            this.gameState = State.Menu;
        }

        private void Play(GameTime gameTime)
        {
            //Enable boss mode if some of players have enough points   ## its 150 just for testing
            if (this.db.Players.GetAll().Any(s => s.Score >= 1000))
            {
               this.EnableBossMode(gameTime);
            }
            else
            {
                for (int i = 0; i < this.db.Enemies.GetCount(); i++)
                {
                    BulletsFactory.EnemyShoot(this.db.Bullets, this.db.Enemies.GetAll()[i]);
                }

                //Creating entities
                EnemyFactory.CreateEnemies(this.db.Enemies, this.random);
                AsteroidFactory.CreateAsteroids(this.db.Asteroids, this.random);

                ItemFactory.CreateItems(this.db.Items, this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.random);
                ItemFactory.CreateItems(this.db.Items, this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.random);

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
                CollisionHandler.CheckForCollision(this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.db.Players.GetAll(), this.db.Explosions.GetAll());

                // Handle collisions between players and enemy items
                CollisionHandler.CheckPlayerItemCollisions(this.db.Items.GetAll(), this.db.Players.GetAll());

                ExplosionFactory.CreateExplosion(this.db.Explosions, this.db.Enemies.GetAll().Cast<IGameObject>().ToList());
                ExplosionFactory.CreateExplosion(this.db.Explosions, this.db.Asteroids.GetAll().Cast<IGameObject>().ToList());

                //Updating entities
                this.db.Enemies.GetAll().ForEach(e => e.Update(gameTime));
                this.db.Asteroids.GetAll().ForEach(a => a.Update(gameTime));
                this.db.Items.GetAll().ForEach(i => i.Update(gameTime));

                // Cleaning with mr.Proper
                EntityCleanerHandler.ClearEnemies(this.db.Enemies);
                EntityCleanerHandler.ClearAsteroids(this.db.Asteroids);
                EntityCleanerHandler.ClearExplosion(this.db.Explosions);
                EntityCleanerHandler.ClearPlayers(this.db.Players);
            }

            //Update 
            this.db.Bullets.GetAll().ForEach(b => b.Update(gameTime));
            this.db.Explosions.GetAll().ForEach(e => e.Update(gameTime));


            StatsManager.UpdatePlayersStats(this.db.Players.GetAll());

            //Handle collisions between bullets and gameobjects
            CollisionHandler.CheckPlayerBulletsCollisions(this.db.Enemies.GetAll().Cast<IGameObject>().ToList(), this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
            CollisionHandler.CheckPlayerBulletsCollisions(this.db.Asteroids.GetAll().Cast<IGameObject>().ToList(), this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());
            CollisionHandler.CheckEnemiesBulletsCollisions(this.db.Bullets.GetAll(), this.db.Players.GetAll());

            EntityCleanerHandler.ClearBullets(this.db.Bullets);        
        }

        private void EnableBossMode(GameTime gameTime)
        {
            this.boss = Boss.Instance;
            this.bossHasInstance = true;

            if (this.bossHasInstance)
            {
                CollisionHandler.CheckBossBulletsCollisions(this.db.Bullets.GetAll(), this.db.Players.GetAll());
                CollisionHandler.CheckPlayerBulletsCollisions(new List<IGameObject>() { this.boss }, this.db.Bullets.GetAll(), this.db.Players.GetAll(), this.db.Explosions.GetAll());

                BulletsFactory.BossShoot(this.db.Bullets, this.boss);

                this.boss.Update(gameTime);

                if (!this.boss.IsVisible)
                {
                    this.gameState = State.Winning;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            switch (this.gameState)
            {
                // DRAWING PLAYING STATE
                case State.Playing:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        //this.hud.Draw(this.spriteBatch);
                        this.DrawAllGameObjects();

                        if (this.bossHasInstance)
                            this.DrawBoss();

                        break;
                    }
                // DRAWING MENU STATE
                case State.Menu:
                    {
                        this.DrawStarfield(TexturesManager.MenuImage); break;
                    }
                // DRAWING GAMEOVER STATE
                case State.GameOver:
                    {
                        this.DrawStarfield(TexturesManager.GameoverImage); break;
                    }
                // DRAWING WINNING STATE
                case State.Winning:
                    {
                        this.DrawStarfield(TexturesManager.WinningImage); break;
                    }

                //DRAWING INTRO VIDEO
                case State.Intro:
                    if (this.videoPlayer.State != MediaState.Stopped)
                    {
                        this.texture = this.videoPlayer.GetTexture();
                        this.spriteBatch.Draw(this.texture, this.GraphicsDevice.Viewport.Bounds, Color.White);
                    }
                    break;
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawAllGameObjects()
        {
            this.db.Enemies.GetAll().ForEach(e => e.Draw(this.spriteBatch));
            this.db.Bullets.GetAll().ForEach(b => b.Draw(this.spriteBatch));
            this.db.Asteroids.GetAll().ForEach(a => a.Draw(this.spriteBatch));
            this.db.Explosions.GetAll().ForEach(e => e.Draw(this.spriteBatch));

            foreach (var item in this.db.Items.GetAll())
            {
                item.Draw(this.spriteBatch);
            }
            this.db.Players.GetAll().ForEach(p => p.Draw(this.spriteBatch));
        }

        private void DrawBoss()
        {
            if (this.db.Asteroids.GetAll().Any())
                this.db.Asteroids.Dispose();
            if (this.db.Enemies.GetAll().Any())
                this.db.Enemies.Dispose();

            this.boss.Draw(this.spriteBatch);
        }

        private void DrawStarfield(Texture2D stateImage)
        {
            this.starfield.Draw(this.spriteBatch);
            this.spriteBatch.Draw(stateImage, new Vector2(0, 0), Color.White);
        }
    }
}