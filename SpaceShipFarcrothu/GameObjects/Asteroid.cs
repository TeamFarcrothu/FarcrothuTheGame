using System;
using System.Collections.Generic;
using SpaceShipFartrothu.Core;
using SpaceShipFartrothu.Effects;

namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Asteroid : EnemyEntity
    {
        private const int DefaultSpeed = 4;
        private const int DefaultDamage = 10;
        private const int DefaultScorePoints = 10;

        private static Random random = new Random();

        // Rotation is not working at the moment in order to get proper collision
        // public Vector2 Origin;
        //public float RotationAngle;
        //public float RandomX;
        //public float RandomY;

        public static List<GameObject> Asteroids = new List<GameObject>();

        public Asteroid(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            this.Speed = DefaultSpeed;
            this.Damage = DefaultDamage;
            this.ScorePoints = DefaultScorePoints;
            //    // Rotation is not working at the moment in order to get proper collision
            //    //this.Origin.X = Texture.Width / 2;
            //    //this.Origin.Y = Texture.Height / 2;
        }

        //public void LoadContent(ContentManager content)
        //{
        //}

        public override void Update(GameTime gameTime)
        {
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Texture.Width, this.Texture.Height);

            this.Position = new Vector2(this.Position.X, this.Position.Y + this.Speed);

            if (this.Position.Y >= Globals.Globals.MAIN_SCREEN_HEIGHT)
            {
                this.Position = new Vector2(this.Position.X, this.Position.Y - 75);
                this.IsVisible = false;
            }

            // Rotation is not working at the moment in order to get proper collision

            //float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            //RotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //RotationAngle %= circle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(this.Texture, this.Position, Color.White);

                // Rotation is not working at the moment in order to get proper collision
                /*spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.White,
                    RotationAngle,
                    Origin,
                    1.0f,
                    SpriteEffects.None,
                    0f);*/
            }
        }

        public static void LoadAsteroids()
        {
            var newRand = new Random();
            int randomX = random.Next(0, 1200) - newRand.Next(0, 20);
            int randomY = random.Next(-700, -50) + newRand.Next(0, 100);

            if (Asteroids.Count < 15)
            {
                Asteroids.Add(new Asteroid(GameEngine.asteroidTexture, new Vector2(randomX, randomY)));
            }

            for (int i = 0; i < Asteroids.Count; i++)
            {
                if (!Asteroids[i].IsVisible)
                {
                    Asteroids.RemoveAt(i);
                    i--;
                }
            }
        }
        public override void ReactOnColission(GameObject target = null)
        {
            Explosion.Explosions.Add(new Explosion(this.Position));
            this.IsVisible = false;

            //TODO

            // Sound
        }
    }
}