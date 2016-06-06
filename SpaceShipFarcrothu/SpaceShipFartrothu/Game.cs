using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShipFartrothu;

namespace Fartrothu
{
    using System;
    using System.Collections.Generic;

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();
        Player2 p2 = new Player2();
        Player p = new Player();
        StarField sf = new StarField();
        List<Asteroid> asteroids = new List<Asteroid>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            this.Window.Title = "Traveling to FARCROTHU";
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            p2.LoadContent(Content);
            p.LoadContent(Content);
            sf.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            foreach (var asteroid in this.asteroids)
            {
                if (asteroid.boundingBox.Intersects(this.p.boundingBox))
                {
                    this.p.health -= 20;
                    asteroid.isVisible = false;
                }

                if (asteroid.boundingBox.Intersects(this.p2.boundingBox))
                {
                    this.p2.health -= 20;
                    asteroid.isVisible = false;
                }

                foreach (var bullet in this.p.bulletList)
                {
                    if (asteroid.boundingBox.Intersects(bullet.boundingBox))
                    {
                        asteroid.isVisible = false;
                        bullet.isVisible = false;
                    }
                }

                foreach (var bullet in this.p2.bulletList)
                {
                    if (asteroid.boundingBox.Intersects(bullet.boundingBox))
                    {
                        asteroid.isVisible = false;
                        bullet.isVisible = false;
                    }
                }

                asteroid.Update(gameTime);
            }

            p2.Update(gameTime);
            p.Update(gameTime);
            sf.Update(gameTime);
            this.LoadAsteroids();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            sf.Draw(spriteBatch);
            p.Draw(spriteBatch);
            p2.Draw(spriteBatch);

            foreach (var asteroid in this.asteroids)
            {
                asteroid.Draw(this.spriteBatch);
            }

            spriteBatch.End();

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
                if (!this.asteroids[i].isVisible)
                {
                    this.asteroids.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
