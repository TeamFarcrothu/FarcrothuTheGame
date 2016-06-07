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
                foreach (var asteroid in this.asteroids)
                {
                    if (asteroid.BoundingBox.Intersects(this.player.BoundingBox))
                    {
                        // if the player has helth left, subtract
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
                        // if the player has helth left, subtract
                        if (this.player2.Health >= 20)
                        {
                            this.player2.Health -= 20;
                        }
                        // else set the health to 0, set the player state to dead
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

                this.player2.Update(gameTime);
                this.player.Update(gameTime);
                this.starfield.Update(gameTime);
                this.LoadAsteroids();
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
    }
}
