using SpaceShipFartrothu.Multimedia;

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
    using Effects;
    using GameObjects;
    using Globals;
    using GameObjects.Items;
    using Factories;
    using Interfaces;

    public class GameEngine : Game
    {
        //Set first state 
        private State gameState = State.Intro;

        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;

        private Random random = new Random();
        //Instances
        private Player player;
        private Player player2;

        private readonly StarField starfield = new StarField();
        private readonly HUD hud = new HUD();
        private readonly SoundManager soundManager = new SoundManager();

        private KeyboardState keyState;

        //private Boss boss;
        private bool bossHasInstance;

        private Texture2D menuImage;
        private Texture2D gameoverImage;
        private Texture2D winningImage;

        private Texture2D player1Texture;
        private Texture2D player2Texture;

        public Texture2D explosionTexture;
        public Texture2D bulletTexture;
        public Texture2D asteroidTexture;
        public Texture2D enemyTexture;

        public static Texture2D itemTexture; //-------------------------
        public static Texture2D bossTexture;
        public static Texture2D healthTexture;

        private VideoManager videoManager = new VideoManager();
        private Texture2D texture;
        private VideoPlayer videoPlayer = new VideoPlayer();
        private bool introPlayed;

        //Factories



        public List<IGameObject> Asteroids = new List<IGameObject>();

        public List<IBullet> Bullets = new List<IBullet>();

        public List<IGameObject> Enemies = new List<IGameObject>();
        public List<InputHandler> inputHandlers = new List<InputHandler>();

        public List<IPlayer> Players = new List<IPlayer>();

        public List<IExplosion> Explosions = new List<IExplosion>();


        public GameEngine()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = Globals.MAIN_SCREEN_WIDTH,
                PreferredBackBufferHeight = Globals.MAIN_SCREEN_HEIGHT
            };

            this.Window.Title = "Traveling to FARCROTHU";
            this.Content.RootDirectory = "Content";
            this.menuImage = null;
            this.gameoverImage = null;
            this.winningImage = null;
        }

        protected override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.hud.LoadContent(this.Content);
            this.starfield.LoadContent(this.Content);
            this.soundManager.LoadContent(this.Content);
            //MediaPlayer.Play(this.soundManager.intro);

            this.menuImage = this.Content.Load<Texture2D>("menu_image");
            this.gameoverImage = this.Content.Load<Texture2D>("gameover_image");
            this.winningImage = this.Content.Load<Texture2D>("winning_image");
            this.player1Texture = this.Content.Load<Texture2D>("ship_p1");
            this.player2Texture = this.Content.Load<Texture2D>("ship_p2");
            this.asteroidTexture = this.Content.Load<Texture2D>("asteroid");

            this.explosionTexture = this.Content.Load<Texture2D>("explosion");
          
            this.bulletTexture = this.Content.Load<Texture2D>("bullet");

            itemTexture = this.Content.Load<Texture2D>("health_potion");//---------------------------------
            bossTexture = this.Content.Load<Texture2D>("space_Boss_Level_1");
            healthTexture = this.Content.Load<Texture2D>("healthbar");
            this.enemyTexture = this.Content.Load<Texture2D>("enemy_ship");

            this.videoManager.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
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
                        this.inputHandlers.ForEach(i => i.Move(this.keyState));

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
                this.videoPlayer.Play(this.videoManager.Video);
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
                this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                this.Players.Add(this.player);

                this.player2 = new Player(this.player2Texture, new Vector2(700, 600), 2);
                this.Players.Add(this.player2);

                this.gameState = State.Playing;
                MediaPlayer.Play(this.soundManager.bgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            //Setup single player game
            if (this.keyState.IsKeyDown(Keys.D1))
            {
                this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                this.Players.Add(this.player);

                this.gameState = State.Playing;
                MediaPlayer.Play(this.soundManager.bgMusic);
                MediaPlayer.Volume = 0.5f;
            }

            foreach (Player player in this.Players)
            {
                player.LoadContent(this.Content);
                this.inputHandlers.Add(new InputHandler(this.player));
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

                    BulletsFactory.EnemyShoot(this.Bullets, (IEnemy)this.Enemies[i], this.bulletTexture);
                }

                foreach (var asteroid in this.Asteroids)
                {
                    asteroid.Update(gameTime);
                }

                foreach (var item in HealthItem.HealthItems)
                {
                    item.Update(gameTime);
                }

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(this.Asteroids, this.Players, this.Explosions);
                CollisionHandler.CheckForCollision(this.Enemies, this.Players, this.Explosions);
                //CollisionHandler.CheckForCollision(HealthItem.HealthItems);

                EnemyFactory.CreateEnemies(this.Enemies, this.random, this.enemyTexture, this.bulletTexture);
                AsteroidFactory.CreateAsteroids(this.Asteroids, this.random, this.asteroidTexture);

                // CLeaning
                EntityCleanerHandler.ClearEnemyBullets(this.Bullets);
                EntityCleanerHandler.ClearEnemies(this.Enemies);
                EntityCleanerHandler.ClearExplosion(this.Explosions);
            }

            this.inputHandlers.ForEach(i => i.PlayerShoot(this.keyState, this.Bullets, this.bulletTexture, this.soundManager));

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

            HealthItem.Update();
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
                        this.DrawStarfield(this.menuImage); break;
                    }
                // DRAWING GAMEOVER STATE
                case State.GameOver:
                    {
                        this.DrawStarfield(this.gameoverImage); break;
                    }
                // DRAWING WINNING STATE
                case State.Winning:
                    {
                        this.DrawStarfield(this.winningImage); break;
                    }

                //DRAWING INTRO VIDEO
                case State.Intro:
                    if (videoPlayer.State != MediaState.Stopped)
                    {
                        texture = videoPlayer.GetTexture();
                        spriteBatch.Draw(texture, GraphicsDevice.Viewport.Bounds, Color.White);
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

            foreach (var item in HealthItem.HealthItems)
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
            if (HealthItem.HealthItems.Any())
                HealthItem.HealthItems.Clear();

            //this.boss.Draw(this.spriteBatch);
        }

        private void DrawStarfield(Texture2D stateImage)
        {
            this.starfield.Draw(this.spriteBatch);
            this.spriteBatch.Draw(stateImage, new Vector2(0, 0), Color.White);
        }
    }
}