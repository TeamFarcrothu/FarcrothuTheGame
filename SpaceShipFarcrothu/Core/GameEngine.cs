namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;
    using Handlers;
    using GameObjects;
    using GameObjects.Items;
    using Factories;
    using Interfaces;
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
        //private Boss boss;

        private bool bossHasInstance;
        private bool introPlayed;

        //Lists
        public List<IGameObject> Asteroids = new List<IGameObject>();

        public List<IBullet> Bullets = new List<IBullet>();

        public List<IGameObject> Enemies = new List<IGameObject>();
        public List<InputHandler> InputHandlers = new List<InputHandler>();

        public List<IPlayer> Players = new List<IPlayer>();

        public List<IExplosion> Explosions = new List<IExplosion>();

        public List<IGameObject> Items = new List<IGameObject>();

        public GameEngine()
        {
            this.starfield = new StarField();
            this.hud = new HUD();
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

            this.hud.LoadContent(this.Content);
            this.starfield.LoadContent(this.Content);

            //MediaPlayer.Play(this.soundManager.intro);
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

                        if (this.Players.Any(p => p.IsAlive == false))
                        {
                            this.gameState = State.GameOver;
                        }

                        //update players
                        this.Players.ForEach(p => p.Update(gameTime));
                        this.InputHandlers.ForEach(i => i.Move(this.keyState));

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
                this.player = new Player(new Vector2(600, 600), 1);
                this.Players.Add(this.player);

                this.player2 = new Player(new Vector2(700, 600), 2);
                this.Players.Add(this.player2);

                this.gameState = State.Playing;
                MediaPlayer.Play(SoundManager.BgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            //Setup single player game
            if (this.keyState.IsKeyDown(Keys.D1))
            {
                this.player = new Player(new Vector2(600, 600), 1);
                this.Players.Add(this.player);

                this.gameState = State.Playing;
                MediaPlayer.Play(SoundManager.BgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            foreach (Player player in this.Players)
            {
                player.LoadContent(this.Content);
                this.InputHandlers.Add(new InputHandler(this.player));
            }

            this.starfield.Update(gameTime);
            this.starfield.Speed = 1;
        }

        private void SetupNewGame()
        {
            MediaPlayer.Stop();

            this.Enemies.Clear();
            this.Asteroids.Clear();
            this.Explosions.Clear();
            this.Players.Clear();
            this.Bullets.Clear();

            this.bossHasInstance = false;

            this.gameState = State.Menu;
        }

        private void Play(GameTime gameTime)
        {
            //Enable boss mode if some of players have enough points   ## its 150 just for testing
            if (this.Players.Any(s => s.Score >= 3000))
            {
                // this.EnableBossMode(gameTime);
            }
            else
            {
                for (int i = 0; i < this.Enemies.Count; i++)
                {
                    this.Enemies[i].Update(gameTime);

                    BulletsFactory.EnemyShoot(this.Bullets, (IEnemy)this.Enemies[i]);
                }

                foreach (var asteroid in this.Asteroids)
                {
                    asteroid.Update(gameTime);
                }


                // Cycle through health items, remove invisible and update visible ones.
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    if (!item.IsVisible)
                    {
                        Items.RemoveAt(i);
                    }
                    else
                    {
                        item.Update(gameTime);
                    }
                }

                /*
                 * foreach (var item in HealthItem.HealthItems)
                 * {
                 *     item.Update(gameTime);
                 * }
                 */

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(this.Asteroids, this.Players, this.Explosions);
                CollisionHandler.CheckForCollision(this.Enemies, this.Players, this.Explosions);
                
                // Handle collisions between players and enemy items
                CollisionHandler.CheckPlayerItemCollisions(this.Items, this.Players);

                EnemyFactory.CreateEnemies(this.Enemies, this.random);
                AsteroidFactory.CreateAsteroids(this.Asteroids, this.random);
                ItemFactory.CreateItems(this.Items, this.Enemies, this.random);
                ItemFactory.CreateItems(this.Items, this.Asteroids, this.random);

                // CLeaning
                EntityCleanerHandler.ClearEnemyBullets(this.Bullets);
                EntityCleanerHandler.ClearEnemies(this.Enemies);
                EntityCleanerHandler.ClearExplosion(this.Explosions);
            }

            this.InputHandlers.ForEach(i => i.PlayerShoot(this.keyState, this.Bullets));

            this.hud.UpdatePlayersInfo(this.Players);

            StatsManager.UpdatePlayersStats(this.Players);

            //Handle collisions between bullets and gameobjects
            CollisionHandler.CheckPlayerBulletsCollisions(this.Enemies, this.Bullets, this.Players, this.Explosions);
            CollisionHandler.CheckPlayerBulletsCollisions(this.Asteroids, this.Bullets, this.Players, this.Explosions);
            CollisionHandler.CheckEnemiesBulletsCollisions(this.Bullets, this.Players);

            //Update all bullets 
            for (int i = 0; i < this.Bullets.Count; i++)
            {
                this.Bullets[i].Update(gameTime);
            }

            //Update all explosions
            this.Explosions.ForEach(e => e.Update(gameTime));

            // Updates static list in HealthItem class - integrated in HealthItem instance update method.
            // HealthItem.Update(); 
        }

        //private void EnableBossMode(GameTime gameTime)
        //{
        //    this.boss = Boss.Instance;
        //    this.bossHasInstance = true;

        //    if (this.bossHasInstance)
        //    {
        //        CollisionHandler.CheckBossBulletsCollisions();
        //        //CollisionHandler.CheckPlayerBulletsCollisions(new List<GameObject>() { this.boss });

        //        this.boss.Update(gameTime);

        //        if (!this.boss.IsVisible)
        //        {
        //            this.gameState = State.Winning;
        //        }
        //    }
        //}

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
                        this.hud.Draw(this.spriteBatch);
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
            this.Players.ForEach(p => p.Draw(this.spriteBatch));
            this.Enemies.ForEach(e => e.Draw(this.spriteBatch));
            this.Bullets.ForEach(b => b.Draw(this.spriteBatch));
            this.Asteroids.ForEach(a => a.Draw(this.spriteBatch));
            this.Explosions.ForEach(e => e.Draw(this.spriteBatch));

            foreach (var item in Items)
            {
                item.Draw(this.spriteBatch);
            }
        }

        private void DrawBoss()
        {
            if (this.Asteroids.Any())
                this.Asteroids.Clear();
            if (this.Enemies.Any())
                this.Enemies.Clear();
            if (this.Items.Any())
                this.Items.Clear();

            //this.boss.Draw(this.spriteBatch);
        }

        private void DrawStarfield(Texture2D stateImage)
        {
            this.starfield.Draw(this.spriteBatch);
            this.spriteBatch.Draw(stateImage, new Vector2(0, 0), Color.White);
        }
    }
}