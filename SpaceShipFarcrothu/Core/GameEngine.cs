namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Media;

    using Effects;
    using GameObjects;
    using Players;
    using Sound;

    public class GameEngine : Game
    {
        //Set first state 
        private State gameState = State.Menu;

        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;

        // Instances
        private readonly Random random = new Random();
        private PlayerNew player;// = new PlayerNew("ship_p1", new Vector2(600, 600), 1);
        private PlayerNew player2;//= new PlayerNew("ship_p2", new Vector2(700, 600), 2);
        private readonly StarField starfield = new StarField();
        private readonly HUD hud = new HUD();
        private readonly SoundManager soundManager = new SoundManager();

        // Lists
        private readonly List<Asteroid> asteroids = new List<Asteroid>();
        private readonly List<Enemy> enemyList = new List<Enemy>();
        private readonly List<Explosion> explosionList = new List<Explosion>();

        private Boss boss;
        private bool bossHasInstance;

        private int enemyBulletDamage;
        private int bossBulletDamage;
        private Texture2D menuImage;
        private Texture2D gameoverImage;
        private Texture2D winningImage;
        private bool twoPlayersMode;

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
            this.enemyBulletDamage = 10;
            this.bossBulletDamage = 30;
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
            //this.player.LoadContent(this.Content);     
            //this.player2.LoadContent(this.Content);     
            this.starfield.LoadContent(this.Content);
            this.soundManager.LoadContent(this.Content);

            MediaPlayer.Play(this.soundManager.intro);

            this.menuImage = this.Content.Load<Texture2D>("menu_image");
            this.gameoverImage = this.Content.Load<Texture2D>("gameover_image");
            this.winningImage = this.Content.Load<Texture2D>("winning_image");
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
                case State.Playing:
                    {
                        this.starfield.Speed = 3;
                        this.Play(gameTime);

                        this.hud.Update(gameTime);


                        this.player.Update(gameTime);

                        //Clear bullets if player is dead
                        if (!this.player.isAlive)
                            this.player.bulletList.Clear();

                        if (!this.player.isAlive && !this.twoPlayersMode)
                        {
                            this.gameState = State.GameOver;
                        }

                        //If two players mode is active update second player too
                        if (twoPlayersMode)
                        {
                            this.player2.Update(gameTime);

                            if (!this.player2.isAlive)
                                this.player2.bulletList.Clear();

                            //Check if both players are dead and switch to Gameover screen if so
                            if (!this.player.isAlive && !this.player2.isAlive)
                            {
                                this.gameState = State.GameOver;
                            }
                        }

                        this.starfield.Update(gameTime);
                        this.ManageExplosions();
                        break;
                    }

                //UPDATING MENU STATE
                case State.Menu:
                    var keyState = Keyboard.GetState();

                    if (keyState.IsKeyDown(Keys.D2))
                    {
                        this.player = new PlayerNew("ship_p1", new Vector2(600, 600), 1);
                        this.player2 = new PlayerNew("ship_p2", new Vector2(700, 600), 2);
                        this.player2.LoadContent(this.Content);
                        this.player.LoadContent(this.Content);
                        this.twoPlayersMode = true;
                        this.gameState = State.Playing;
                        MediaPlayer.Play(this.soundManager.bgMusic);
                        MediaPlayer.Volume = 0.5f;
                    }

                    if (keyState.IsKeyDown(Keys.D1))
                    {
                        this.player = new PlayerNew("ship_p1", new Vector2(600, 600), 1);
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

                        this.enemyList.Clear();
                        this.asteroids.Clear();
                        this.explosionList.Clear();

                        this.player.health = 200;
                        this.player.isAlive = true;
                        this.hud.playerscore = 0;

                        this.bossHasInstance = false;

                        if (twoPlayersMode)
                        {
                            this.player2.isAlive = true;
                            this.player2.health = 200;
                            this.hud.player2score = 0;
                        }

                        this.gameState = State.Menu;
                    }

                    this.starfield.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void Play(GameTime gameTime)
        {
            if (this.hud.playerscore >= 10 || (twoPlayersMode && this.hud.player2score >= 10))
            {
                this.EnableBossMode(gameTime);
            }
            else
            {
                this.ManageEnemyCollisions(gameTime);

                foreach (var explosion in this.explosionList)
                {
                    explosion.Update(gameTime);
                }

                foreach (var asteroid in this.asteroids)
                {
                    if (asteroid.BoundingBox.Intersects(this.player.boundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.hud.playerscore += 5;
                        asteroid.IsVisible = false;
                        this.player.health -= 20;
                    }


                    foreach (var bullet in this.player.bulletList)
                    {
                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                        {
                            this.soundManager.explodeSound.Play();
                            this.explosionList.Add(
                                new Explosion(this.Content.Load<Texture2D>("explosion"),
                                    new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                            this.hud.playerscore += 5;
                            asteroid.IsVisible = false;
                            bullet.IsVisible = false;
                        }
                    }

                    if (twoPlayersMode)
                    {
                        if (asteroid.BoundingBox.Intersects(this.player2.boundingBox))
                        {
                            this.soundManager.explodeSound.Play();
                            asteroid.IsVisible = false;
                            this.player2.health -= 20;
                            this.hud.player2score += 5;
                        }
                        foreach (var bullet in this.player2.bulletList)
                        {
                            if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                            {
                                this.soundManager.explodeSound.Play();
                                this.explosionList.Add(
                                    new Explosion(this.Content.Load<Texture2D>("explosion"),
                                        new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                                this.hud.player2score += 5;
                                asteroid.IsVisible = false;
                                bullet.IsVisible = false;
                            }
                        }
                    }

                    asteroid.Update(gameTime);
                }

                this.LoadAsteroids();
                this.LoadEnemies();
            }
        }

        private void EnableBossMode(GameTime gameTime)
        {
            this.LoadBoss();

            if (this.boss.boundingBox.Intersects(this.player.boundingBox))
            {
                this.player.health -= 50;
                // this.hud.playerscore += 20;
            }

            if (twoPlayersMode)
            {
                if (this.boss.boundingBox.Intersects(this.player2.boundingBox))
                {
                    this.soundManager.explodeSound.Play();
                    this.player2.health -= 50;
                    //this.hud.player2score += 20;
                }
            }

            // Boss bullet collisions
            foreach (var bullet in this.boss.bulletList)
            {
                if (this.player.boundingBox.Intersects(bullet.BoundingBox))
                {
                    this.soundManager.explodeSound.Play();
                    this.player.health -= this.bossBulletDamage;
                    bullet.IsVisible = false;
                }

                if (twoPlayersMode)
                {
                    if (this.player2.boundingBox.Intersects(bullet.BoundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.player2.health -= this.bossBulletDamage;
                        bullet.IsVisible = false;
                    }
                }
            }

            // player bullet lists colliding with boss ship
            for (int i = 0; i < this.player.bulletList.Count; i++)
            {
                if (this.player.bulletList[i].BoundingBox.Intersects(this.boss.boundingBox))
                {
                    this.soundManager.explodeSound.Play();
                    this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                        new Vector2(this.boss.position.X + this.boss.texture.Height / 2,
                            this.boss.position.Y + this.boss.texture.Width / 2)));
                    // this.hud.playerscore += 20;
                    this.player.bulletList[i].IsVisible = false;
                    this.boss.health -= 2;
                }
            }

            if (twoPlayersMode)
            {
                for (int i = 0; i < this.player2.bulletList.Count; i++)
                {
                    if (this.player2.bulletList[i].BoundingBox.Intersects(this.boss.boundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.explosionList.Add(new Explosion(this.Content.Load<Texture2D>("explosion"),
                            new Vector2(this.boss.position.X + this.boss.texture.Height / 2,
                                this.boss.position.Y + this.boss.texture.Width / 2)));
                        // this.hud.player2score += 20;
                        this.player2.bulletList[i].IsVisible = false;
                        this.boss.health -= 2;
                    }
                }
            }

            foreach (var explosion in this.explosionList)
            {
                explosion.Update(gameTime);
            }

            this.boss.Update(gameTime);

            if (!this.boss.isVisible)
            {
                this.gameState = State.Winning;
            }
        }

        private void ManageEnemyCollisions(GameTime gameTime)
        {
            foreach (Enemy enemy in this.enemyList)
            {
                if (enemy.boundingBox.Intersects(this.player.boundingBox))
                {
                    this.soundManager.explodeSound.Play();
                    this.player.health -= 40;
                    this.hud.playerscore += 20;
                    enemy.isVisible = false;
                }

                if (twoPlayersMode)
                {
                    if (enemy.boundingBox.Intersects(this.player2.boundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.player2.health -= 40;
                        this.hud.player2score += 20;
                        enemy.isVisible = false;
                    }
                }

                for (int i = 0; i < enemy.bulletList.Count; i++)
                {
                    if (this.player.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.player.health -= this.enemyBulletDamage;
                        enemy.bulletList[i].IsVisible = false;
                    }

                    if (twoPlayersMode)
                    {
                        if (this.player2.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                        {
                            this.soundManager.explodeSound.Play();
                            this.player2.health -= this.enemyBulletDamage;
                            enemy.bulletList[i].IsVisible = false;
                        }
                    }

                }

                // player bullet lists colliding with enemy ships
                for (int i = 0; i < this.player.bulletList.Count; i++)
                {
                    if (this.player.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                    {
                        this.soundManager.explodeSound.Play();
                        this.explosionList.Add(
                            new Explosion(this.Content.Load<Texture2D>("explosion"),
                                new Vector2(enemy.position.X, enemy.position.Y)));

                        this.hud.playerscore += 20;
                        this.player.bulletList[i].IsVisible = false;
                        enemy.isVisible = false;
                    }
                }

                if (twoPlayersMode)
                {
                    for (int i = 0; i < this.player2.bulletList.Count; i++)
                    {
                        if (this.player2.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                        {
                            this.soundManager.explodeSound.Play();
                            this.explosionList.Add(
                                new Explosion(this.Content.Load<Texture2D>("explosion"),
                                    new Vector2(enemy.position.X, enemy.position.Y)));
                            this.hud.player2score += 20;
                            this.player2.bulletList[i].IsVisible = false;
                            enemy.isVisible = false;
                        }
                    }
                }

                enemy.Update(gameTime);
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
                        this.player.Draw(this.spriteBatch);

                        if (twoPlayersMode)
                            this.player2.Draw(this.spriteBatch);

                        //Update explosions
                        foreach (var explosion in this.explosionList)
                        {
                            explosion.Draw(this.spriteBatch);
                        }

                        //Update asteroids and check for collisions
                        foreach (var asteroid in this.asteroids)
                        {
                            asteroid.Draw(this.spriteBatch);
                        }

                        foreach (Enemy enemy in this.enemyList)
                        {
                            enemy.Draw(this.spriteBatch);
                        }

                        this.hud.Draw(this.spriteBatch);

                        if (this.bossHasInstance)
                        {
                            this.asteroids.Clear();
                            this.enemyList.Clear();

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
            }

            this.spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LoadBoss()
        {
            // Singleton
            if (!this.bossHasInstance)
            {
                this.boss = new Boss();
                this.bossHasInstance = true;
                this.boss.LoadContent(this.Content);
            }
        }

        private void LoadAsteroids()
        {
            var newRand = new Random();
            int randomX = this.random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = this.random.Next(-700, -50) + newRand.Next(0, 100);

            if (this.asteroids.Count < 15)
            {
                this.asteroids.Add(new Asteroid(
                    this.Content.Load<Texture2D>("asteroid"),
                    new Vector2(randomX, randomY)));
            }

            for (int i = 0; i < this.asteroids.Count; i++)
            {
                if (!this.asteroids[i].IsVisible)
                {
                    this.asteroids.RemoveAt(i);
                    i--;
                }
            }
        }

        private void LoadEnemies()
        {
            var newRand = new Random();
            int randomX = this.random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = this.random.Next(-700, -50) + newRand.Next(0, 100);

            if (this.enemyList.Count < 3)
            {
                this.enemyList.Add(new Enemy(
                    this.Content.Load<Texture2D>("enemy_ship"),
                    new Vector2(randomX, randomY),
                    this.Content.Load<Texture2D>("bullet")));
            }

            for (int i = 0; i < this.enemyList.Count; i++)
            {
                if (!this.enemyList[i].isVisible)
                {
                    this.enemyList.RemoveAt(i);
                    i--;
                }
            }
        }

        private void ManageExplosions()
        {
            for (int i = 0; i < this.explosionList.Count; i++)
            {
                if (!this.explosionList[i].isVisible)
                {
                    this.explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}