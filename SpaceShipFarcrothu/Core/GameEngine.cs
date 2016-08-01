using SpaceShipFartrothu.Multimedia;

namespace SpaceShipFartrothu.Core
{
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

    public class GameEngine : Game
    {
        //Set first state 
        private State gameState = State.Intro;

        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;

        //Instances
        private Player player;
        private Player player2;

        private readonly StarField starfield = new StarField();
        private readonly HUD hud = new HUD();
        private readonly SoundManager soundManager = new SoundManager();

        private Boss boss;
        private bool bossHasInstance;

        private Texture2D menuImage;
        private Texture2D gameoverImage;
        private Texture2D winningImage;

        private Texture2D player1Texture;
        private Texture2D player2Texture;

        public static Texture2D explosionTexture;
        public static Texture2D bulletTexture;
        public static Texture2D asteroidTexture;
        public static Texture2D itemTexture; //-------------------------
        public static Texture2D bossTexture;
        public static Texture2D healthTexture;

        private VideoManager videoManager = new VideoManager();
        private Texture2D texture;
        private VideoPlayer videoPlayer = new VideoPlayer();
        private bool introPlayed;

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

            asteroidTexture = this.Content.Load<Texture2D>("asteroid");
            explosionTexture = this.Content.Load<Texture2D>("explosion");
            bulletTexture = this.Content.Load<Texture2D>("bullet");
            itemTexture = this.Content.Load<Texture2D>("health_potion");//---------------------------------
            bossTexture = this.Content.Load<Texture2D>("space_Boss_Level_1");
            healthTexture = this.Content.Load<Texture2D>("healthbar");
            Enemy.enemyTexture = this.Content.Load<Texture2D>("enemy_ship");

            this.videoManager.LoadContent(this.Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus) && MediaPlayer.Volume >= 0.02f)
            {
                MediaPlayer.Volume -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus) && MediaPlayer.Volume <= 0.98f)
            {
                MediaPlayer.Volume += 0.02f;
            }
            //UPDATING Game state
            switch (this.gameState)
            {
                case State.Intro:
                    if (videoPlayer.State == MediaState.Stopped && this.introPlayed == false)
                    {
                        videoPlayer.Play(this.videoManager.Video);
                        this.introPlayed = true;
                    }
                    else if (videoPlayer.State == MediaState.Stopped && this.introPlayed == true)
                    {
                        this.gameState = State.Menu;
                    }
                    break;
                case State.Playing:
                    {
                        this.starfield.Speed = 3;

                        this.Play(gameTime);

                        if (Player.Players.Any(p => p.IsAlive == false))
                        {
                            this.gameState = State.GameOver;
                        }

                        Player.Players.ForEach(p => p.Update(gameTime));
                        this.starfield.Update(gameTime);

                        break;
                    }

                //UPDATING MENU STATE
                case State.Menu:
                    var keyState = Keyboard.GetState();

                    //Setup two players game
                    if (keyState.IsKeyDown(Keys.D2))
                    {
                        this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                        this.player2 = new Player(this.player2Texture, new Vector2(700, 600), 2);
                        this.player2.LoadContent(this.Content);
                        this.player.LoadContent(this.Content);

                        //this.twoPlayersMode = true;
                        this.gameState = State.Playing;
                        MediaPlayer.Play(this.soundManager.bgMusic);
                        MediaPlayer.Volume = 0.5f;
                    }

                    //Setup single player game
                    if (keyState.IsKeyDown(Keys.D1))
                    {
                        this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                        this.player.LoadContent(this.Content);

                        //this.twoPlayersMode = false;
                        this.gameState = State.Playing;
                        MediaPlayer.Play(this.soundManager.bgMusic);
                        MediaPlayer.Volume = 0.5f;
                    }

                    this.starfield.Update(gameTime);
                    this.starfield.Speed = 1;
                    break;

                //UPDATING GAMEOVER STATE or WINNING STATE
                case State.GameOver:
                case State.Winning:
                    var currentKeyState = Keyboard.GetState();

                    if (currentKeyState.IsKeyDown(Keys.Space))
                    {
                        this.SetupNewGame();
                    }

                    this.starfield.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void SetupNewGame()
        {
            MediaPlayer.Stop();

            Enemy.Enemies.Clear();
            Asteroid.Asteroids.Clear();
            Explosion.Explosions.Clear();
            Player.Players.Clear();
            Bullet.Bullets.Clear();

            this.bossHasInstance = false;

            this.gameState = State.Menu;
        }

        private void Play(GameTime gameTime)
        {
            //Enable boss mode if some of players have enough points   ## its 150 just for testing
            if (Player.Players.Any(s => s.Score >= 3000))
            {
                this.EnableBossMode(gameTime);
            }
            else
            {
                for (int i = 0; i < Enemy.Enemies.Count; i++)
                {
                    Enemy.Enemies[i].Update(gameTime);
                }

                foreach (var asteroid in Asteroid.Asteroids)
                {
                    asteroid.Update(gameTime);
                }

                foreach (var item in HealthItem.HealthItems)
                {
                    item.Update(gameTime);
                }

                // Handle collisions between players and enemy objects
                CollisionHandler.CheckForCollision(Asteroid.Asteroids);
                CollisionHandler.CheckForCollision(Enemy.Enemies);
                CollisionHandler.CheckForCollision(HealthItem.HealthItems);

                Enemy.LoadEnemies();
                Asteroid.LoadAsteroids();
            }

            this.hud.UpdatePlayersInfo(Player.Players);
            StatsManager.UpdatePlayersStats(Player.Players);

            //Handle collisions between bullets and gameobjects
            CollisionHandler.CheckPlayerBulletsCollisions(Enemy.Enemies);
            CollisionHandler.CheckPlayerBulletsCollisions(Asteroid.Asteroids);
            CollisionHandler.CheckEnemiesBulletsCollisions();

            //Update all bullets 
            for (int i = 0; i < Bullet.Bullets.Count; i++)
            {
                Bullet.Bullets[i].Update(gameTime);
            }

            //Update all explosions
            foreach (var explosion in Explosion.Explosions)
            {
                explosion.Update(gameTime);
            }

            HealthItem.Update();
        }

        private void EnableBossMode(GameTime gameTime)
        {
            this.boss = Boss.Instance;
            this.bossHasInstance = true;

            if (this.bossHasInstance)
            {
                CollisionHandler.CheckBossBulletsCollisions();
                CollisionHandler.CheckPlayerBulletsCollisions(new List<GameObject>() { this.boss });

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
            Player.Players.ForEach(p => p.Draw(this.spriteBatch));

            foreach (var enemy in Enemy.Enemies)
            {
                enemy.Draw(this.spriteBatch);
            }

            foreach (var bullet in Bullet.Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var explosion in Explosion.Explosions)
            {
                explosion.Draw(this.spriteBatch);

            }

            foreach (var asteroid in Asteroid.Asteroids)
            {
                asteroid.Draw(this.spriteBatch);
            }

            foreach (var item in HealthItem.HealthItems)
            {
                item.Draw(this.spriteBatch);
            }
        }

        private void DrawBoss()
        {
            if (Asteroid.Asteroids.Any())
                Asteroid.Asteroids.Clear();
            if (Enemy.Enemies.Any())
                Enemy.Enemies.Clear();
            if (HealthItem.HealthItems.Any())
                HealthItem.HealthItems.Clear();

            this.boss.Draw(this.spriteBatch);
        }

        private void DrawStarfield(Texture2D stateImage)
        {
            this.starfield.Draw(this.spriteBatch);
            this.spriteBatch.Draw(stateImage, new Vector2(0, 0), Color.White);
        }

        //private void ActivateSecondBullet()
        //{
        //    if (this.player.Score >= hud.p1neededPointsToNextLevel)
        //    {
        //        hud.p1hasEnoughToNextLevel = true;
        //        if (hud.p1hasEnoughToNextLevel)
        //        {
        //            player.Level++;
        //            hud.p1hasEnoughToNextLevel = false;
        //            hud.p1neededPointsToNextLevel += hud.p1neededPointsToNextLevel;
        //        }
        //        //if (hud.playerLevel == 3)
        //        //{
        //        //    this.player.isSecondBulletActive = true;
        //        //}
        //        //if (hud.playerLevel == 5)
        //        //{
        //        //    this.player.isThirdBulletActive = true;
        //        //}
        //    }

        //    if (this.twoPlayersMode)
        //    {
        //        if (this.player2.Score >= hud.p2neededPointsToNextLevel)
        //        {
        //            hud.p2hasEnoughToNextLevel = true;
        //            if (hud.p2hasEnoughToNextLevel)
        //            {
        //                //hud.player2Level++;
        //                hud.p2hasEnoughToNextLevel = false;
        //                hud.p2neededPointsToNextLevel += hud.p2neededPointsToNextLevel;
        //            }
        //            //if (hud.player2Level == 3)
        //            //{
        //            //    this.player2.isSecondBulletActive = true;
        //            //}
        //            //if (hud.player2Level == 5)
        //            //{
        //            //    this.player2.isThirdBulletActive = true;
        //            //}
        //        }

        //    }
        //}
    }
}