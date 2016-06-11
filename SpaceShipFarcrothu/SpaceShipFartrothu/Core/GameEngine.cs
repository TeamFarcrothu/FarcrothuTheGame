namespace SpaceShipFartrothu.Core
{
    using System;
    using System.Collections.Generic;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Players;
    using Effects;
    public class GameEngine : Game
    {
        private SpriteBatch spriteBatch;
        private readonly GraphicsDeviceManager graphics;
        private readonly Random random = new Random();
        //private readonly Player player = new Player();
        private readonly PlayerNew player = new PlayerNew("ship_p1", new Vector2(600, 600), 1);
        //private readonly Player2 player2 = new Player2();
        private readonly PlayerNew player2 = new PlayerNew("ship_p2", new Vector2(700, 600), 2);
        private readonly StarField starfield = new StarField();
        private readonly List<Asteroid> asteroids = new List<Asteroid>();
        private readonly List<Enemy> enemyList = new List<Enemy>();
        private readonly HUD hud = new HUD();
        private readonly List<Explosion> explosionList = new List<Explosion>();

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

            hud.LoadContent(this.Content);
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
                    if (enemy.boundingBox.Intersects(player.boundingBox))
                    {
                        player.health -= 40;
                        hud.playerscore += 20;
                        enemy.isVisible = false;
                    }
                    if (enemy.boundingBox.Intersects(player2.boundingBox))
                    {
                        player2.health -= 40;
                        hud.player2score += 20;
                        enemy.isVisible = false;
                    }

                    for (int i = 0; i < enemy.bulletList.Count; i++)
                    {
                        if (player.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                        {
                            player.health -= enemyBulletDamage;
                            enemy.bulletList[i].IsVisible = false;
                        }
                        if (player2.boundingBox.Intersects(enemy.bulletList[i].BoundingBox))
                        {
                            player2.health -= enemyBulletDamage;
                            enemy.bulletList[i].IsVisible = false;
                        }
                    }
                    // player bullet lists colliding with enemy ships
                    for (int i = 0; i < player.bulletList.Count; i++)
                    {
                        if (player.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(enemy.position.X, enemy.position.Y)));
                            hud.playerscore += 20;
                            player.bulletList[i].IsVisible = false;
                            enemy.isVisible = false;
                        }
                    }
                    for (int i = 0; i < player2.bulletList.Count; i++)
                    {
                        if (player2.bulletList[i].BoundingBox.Intersects(enemy.boundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(enemy.position.X, enemy.position.Y)));
                            hud.player2score += 20;
                            player2.bulletList[i].IsVisible = false;
                            enemy.isVisible = false;
                        }
                    }
                    enemy.Update(gameTime);
                }
                foreach (var explosion in explosionList)
                {
                    explosion.Update(gameTime);
                }
                foreach (var asteroid in asteroids)
                {
                    if (asteroid.BoundingBox.Intersects(player.boundingBox))
                    {
                        hud.playerscore += 5;
                        asteroid.IsVisible = false;
                        player.health -= 20;
                    }

                    if (asteroid.BoundingBox.Intersects(this.player2.boundingBox))
                    {
                        asteroid.IsVisible = false;
                        player2.health -= 20;
                        hud.player2score += 5;
                    }

                    foreach (var bullet in this.player.bulletList)
                    {
                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                            hud.playerscore += 5;
                            asteroid.IsVisible = false;
                            bullet.IsVisible = false;
                        }
                    }

                    foreach (var bullet in this.player2.bulletList)
                    {
                        if (asteroid.BoundingBox.Intersects(bullet.BoundingBox))
                        {
                            explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion"), new Vector2(asteroid.Position.X, asteroid.Position.Y)));
                            hud.player2score += 5;
                            asteroid.IsVisible = false;
                            bullet.IsVisible = false;
                        }
                    }

                    asteroid.Update(gameTime);
                }
                this.hud.Update(gameTime);
                this.player2.Update(gameTime);
                this.player.Update(gameTime);
                this.starfield.Update(gameTime);
                this.ManageExplosions();
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
            foreach (var explosion in explosionList)
            {
                explosion.Draw(spriteBatch);
            }
            foreach (var asteroid in this.asteroids)
            {
                asteroid.Draw(this.spriteBatch);
            }

            foreach(Enemy enemy in this.enemyList)
            {
                enemy.Draw(this.spriteBatch);
            }

            this.hud.Draw(this.spriteBatch);
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
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisible)
                {
                    explosionList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
