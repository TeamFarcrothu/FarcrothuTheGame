namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Players;
    
    public class GameEngine : Game
    {
        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;
        private readonly Random random = new Random();
        private readonly Player player = new Player();
        private readonly Player2 player2 = new Player2();
        private readonly StarField starfield = new StarField();
        private readonly List<Asteroid> asteroids = new List<Asteroid>();
        private readonly List<Enemy> enemyList = new List<Enemy>();

        public int enemyBulletDamage;

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
            enemyBulletDamage = 10;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.player2.LoadContent(this.Content);
            this.player.LoadContent(this.Content);
            this.starfield.LoadContent(this.Content);
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

            // if one of the players is alive, keep going
            if(player.isAlive || player2.isAlive)
            {
                foreach(Enemy enemy in this.enemyList)
                {
                    if (enemy.boundingBox.Intersects(player.BoundingBox))
                    {
                        player.Health -= 40;
                        enemy.isVisible = false;
                    }
                    if (enemy.boundingBox.Intersects(player2.BoundingBox))
                    {
                        player2.Health -= 40;
                        enemy.isVisible = false;
                    }

                    for (int i = 0; i < enemy.bulletList.Count; i++)
                    {
                        if (player.BoundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                        {
                            player.Health -= enemyBulletDamage;
                            enemy.bulletList[i].IsVisible = false;
                        }
                        if (player2.BoundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                        {
                            player2.Health -= enemyBulletDamage;
                            enemy.bulletList[i].IsVisible = false;
                        }
                    }
                    // player bullet lists colliding with enemy ships
                    for (int i = 0; i < player.BulletList.Count; i++)
                    {
                        if (player.BulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                        {
                            player.BulletList[i].IsVisible = false;
                            enemy.isVisible = false;
                        }
                        if (player2.BulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                        {
                            player2.BulletList[i].IsVisible = false;
                            enemy.isVisible = false;
                        }
                    }
                    enemy.Update(gameTime);
                }

                foreach (var asteroid in this.asteroids)
                {
                    if (asteroid.BoundingBox.Intersects(this.player.BoundingBox))
                    {
                        // if the player has health left, subtract
                        if (this.player.Health >= 20)
                        {
                            this.player.Health -= 20;
                        }
                        // else set the health to 0, set the player state to dead
                        // and position him at the bottom left
                        else
                        {
                            this.player.Health = 0;
                            this.player.isAlive = false;
                            this.player.Position = new Vector2(200, 600);
                        }
                        asteroid.IsVisible = false;
                    }

                    if (asteroid.BoundingBox.Intersects(this.player2.BoundingBox))
                    {
                        // if the player2 has health left, subtract
                        if (this.player2.Health >= 20)
                        {
                            this.player2.Health -= 20;
                        }
                        // else set the health to 0, set the player2 state to dead
                        // and position him at the bottom left
                        else
                        {
                            this.player2.Health = 0;
                            this.player2.isAlive = false;
                            this.player2.Position = new Vector2(1000, 600);
                        }
                        asteroid.IsVisible = false;
                    }

                    foreach (var bullet in this.player.BulletList)
                    {
                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                        {
                            asteroid.IsVisible = false;
                            bullet.IsVisible = false;
                        }
                    }

                    foreach (var bullet in this.player2.BulletList)
                    {
                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                        {
                            asteroid.IsVisible = false;
                            bullet.IsVisible = false;
                        }
                    }

                    asteroid.Update(gameTime);
                }
                // if player2 is still alive - update
                if (this.player2.isAlive)
                {
                    this.player2.Update(gameTime);
                }
                // if player is still alive - update
                if (this.player.isAlive)
                {
                    this.player.Update(gameTime);
                }
                this.starfield.Update(gameTime);
                this.LoadAsteroids();
                this.LoadEnemies();
            //[end of] if one of the players is alive, keep going
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            this.starfield.Draw(this.spriteBatch);
            this.player.Draw(this.spriteBatch);
            this.player2.Draw(this.spriteBatch);

            foreach (var asteroid in this.asteroids)
            {
                asteroid.Draw(this.spriteBatch);
            }

            foreach(Enemy enemy in this.enemyList)
            {
                enemy.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LoadAsteroids()
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

        public void LoadEnemies()
        {
            var newRand = new Random();
            int randomX = this.random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = this.random.Next(-700, -50) + newRand.Next(0, 100);

            if (this.enemyList.Count < 3)
            {
                enemyList.Add(new Enemy(
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
    }
}
