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
    using Sound;

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

        //private Texture2D bulletTexture;

        public static Texture2D explosionTexture;
        public static Texture2D bulletTexture;
        public static Texture2D asteroidTexture;
        public static Texture2D bossTexture;
        public static Texture2D healthTexture;

        private bool twoPlayersMode;

        VideoPlayer videoPlayer;
        Video video;
        private Texture2D texture;
        private bool introPlayed;

        public GameEngine()
        {
            this.graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768
            };

            this.Window.Title = "Traveling to FARCROTHU";
            this.Content.RootDirectory = "Content";
            this.menuImage = null;
            this.gameoverImage = null;
            this.winningImage = null;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.hud.LoadContent(this.Content);

            //if (this.bossHasInstance)
            //    this.boss.LoadContent(this.Content);

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
            bossTexture = this.Content.Load<Texture2D>("space_Boss_Level_1");
            healthTexture = this.Content.Load<Texture2D>("healthbar");
            Enemy.enemyTexture = this.Content.Load<Texture2D>("enemy_ship");

            //video = Content.Load<Video>("sample");
            //videoPlayer = new VideoPlayer();
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
                    //if (videoPlayer.State == MediaState.Stopped && this.introPlayed == false)
                    //{
                    //    videoPlayer.Play(video);
                    //    this.introPlayed = true;
                    //}
                    //else if (videoPlayer.State == MediaState.Stopped && this.introPlayed == true)
                    //{
                    this.gameState = State.Menu;
                    //}
                    break;
                case State.Playing:
                    {
                        this.starfield.Speed = 3;

                        this.Play(gameTime);

                        //this.ActivateSecondBullet();

                        if (!this.player.IsAlive && !this.twoPlayersMode)
                        {
                            this.gameState = State.GameOver;
                        }

                        //If two players mode is active update second player too
                        if (this.twoPlayersMode)
                        {
                            this.player2.Update(gameTime);

                            //Check if both players are dead and switch to Gameover screen if so
                            if (!this.player.IsAlive && !this.player2.IsAlive)
                            {
                                this.gameState = State.GameOver;
                            }
                        }

                        this.starfield.Update(gameTime);

                        break;
                    }

                //UPDATING MENU STATE
                case State.Menu:
                    var keyState = Keyboard.GetState();

                    if (keyState.IsKeyDown(Keys.D2))
                    {
                        this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                        this.player2 = new Player(this.player2Texture, new Vector2(700, 600), 2);
                        this.player2.LoadContent(this.Content);
                        this.player.LoadContent(this.Content);

                        this.twoPlayersMode = true;
                        this.gameState = State.Playing;
                        MediaPlayer.Play(this.soundManager.bgMusic);
                        MediaPlayer.Volume = 0.5f;
                    }

                    if (keyState.IsKeyDown(Keys.D1))
                    {
                        this.player = new Player(this.player1Texture, new Vector2(600, 600), 1);
                        this.player.LoadContent(this.Content);

                        this.twoPlayersMode = false;
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
                        MediaPlayer.Stop();

                        Enemy.Enemies.Clear();
                        Asteroid.Asteroids.Clear();
                        Explosion.Explosions.Clear();

                        foreach (var pl in Player.Players)
                        {
                            pl.Health = 200;
                            pl.IsAlive = true;
                        }

                        this.bossHasInstance = false;

                        this.gameState = State.Menu;
                    }

                    this.starfield.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void Play(GameTime gameTime)
        {


            //Enable boss mode if we have enough points   ## its 10 just for testing
            if (this.player.Score >= 3000 || (this.twoPlayersMode && this.player2.Score >= 3000))
            {
                this.EnableBossMode(gameTime);
            }
            else
            {
                for (int i = 0; i < Enemy.Enemies.Count; i++)
                {
                    Enemy.Enemies[i].Update(gameTime);
                }

                ColisionHandler.CheckForCollision(Player.Players, Asteroid.Asteroids);
                ColisionHandler.CheckForCollision(Player.Players, Enemy.Enemies);
                //ColisionHandler.CheckPlayerBulletsCollisions(this.player, this.enemyList);

                // if two players mode is active check second player too
                // ColisionHandler.CheckPlayerBulletsCollisions(this.player2, this.enemyList);

                foreach (var asteroid in Asteroid.Asteroids)
                {
                    asteroid.Update(gameTime);
                }

                Enemy.LoadEnemies();
                Asteroid.LoadAsteroids();
            }

            //this.hud.Update(gameTime);
            this.hud.UpdatePlayersInfo(Player.Players);
            StatsManager.UpdatePlayersStats(Player.Players);
            this.player.Update(gameTime);

            //Update all bullets 
            for (int i = 0; i < Bullet.Bullets.Count; i++)
            {
                Bullet.Bullets[i].Update(gameTime);
            }

            ColisionHandler.CheckPlayerBulletsCollisions(this.player, Enemy.Enemies);
            ColisionHandler.CheckPlayerBulletsCollisions(this.player, Asteroid.Asteroids);
            ColisionHandler.CheckEnemiesBulletsCollisions(Enemy.Enemies);

            foreach (var explosion in Explosion.Explosions)
            {
                explosion.Update(gameTime);
            }
        }

        private void EnableBossMode(GameTime gameTime)
        {
            this.boss = Boss.Instance;
            this.bossHasInstance = true;

            if (this.bossHasInstance)
            {
                ColisionHandler.CheckBossBulletsCollisions();
                ColisionHandler.CheckPlayerBulletsCollisions(this.player, new List<GameObject>() { this.boss });

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

                        this.player.Draw(this.spriteBatch);

                        if (this.twoPlayersMode)
                            this.player2.Draw(this.spriteBatch);

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

                        if (this.bossHasInstance)
                        {
                            if (Asteroid.Asteroids.Any())
                                Asteroid.Asteroids.Clear();
                            if (Enemy.Enemies.Any())
                                Enemy.Enemies.Clear();

                            this.boss.Draw(this.spriteBatch);
                        }

                        break;
                    }

                // DRAWING MENU STATE
                case State.Menu:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        this.spriteBatch.Draw(this.menuImage, new Vector2(0, 0), Color.White);

                        break;
                    }
                // DRAWING GAMEOVER STATE
                case State.GameOver:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        this.spriteBatch.Draw(this.gameoverImage, new Vector2(0, 0), Color.White);

                        break;
                    }

                // DRAWING WINNING STATE
                case State.Winning:
                    {
                        this.starfield.Draw(this.spriteBatch);
                        this.spriteBatch.Draw(this.winningImage, new Vector2(0, 0), Color.White);

                        break;
                    }

                    // DRAWING INTRO VIDEO
                    //case State.Intro:
                    //    if (videoPlayer.State != MediaState.Stopped)
                    //    {
                    //        texture = videoPlayer.GetTexture();
                    //        spriteBatch.Draw(texture, GraphicsDevice.Viewport.Bounds, Color.White);
                    //    }
                    //    break;
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        //private void LoadBoss()
        //{
        //    if (!this.bossHasInstance)
        //    {
        //        //                                                  -600
        //        this.boss = new Boss(bossTexture, new Vector2(501, 10));
        //        this.bossHasInstance = true;
        //        this.boss.LoadContent(this.Content);
        //    }
        //}

        private void ActivateSecondBullet()
        {
            if (this.player.Score >= hud.p1neededPointsToNextLevel)
            {
                hud.p1hasEnoughToNextLevel = true;
                if (hud.p1hasEnoughToNextLevel)
                {
                    player.Level++;
                    hud.p1hasEnoughToNextLevel = false;
                    hud.p1neededPointsToNextLevel += hud.p1neededPointsToNextLevel;
                }
                //if (hud.playerLevel == 3)
                //{
                //    this.player.isSecondBulletActive = true;
                //}
                //if (hud.playerLevel == 5)
                //{
                //    this.player.isThirdBulletActive = true;
                //}
            }

            if (this.twoPlayersMode)
            {
                if (this.player2.Score >= hud.p2neededPointsToNextLevel)
                {
                    hud.p2hasEnoughToNextLevel = true;
                    if (hud.p2hasEnoughToNextLevel)
                    {
                        //hud.player2Level++;
                        hud.p2hasEnoughToNextLevel = false;
                        hud.p2neededPointsToNextLevel += hud.p2neededPointsToNextLevel;
                    }
                    //if (hud.player2Level == 3)
                    //{
                    //    this.player2.isSecondBulletActive = true;
                    //}
                    //if (hud.player2Level == 5)
                    //{
                    //    this.player2.isThirdBulletActive = true;
                    //}
                }

            }
        }
    }
}