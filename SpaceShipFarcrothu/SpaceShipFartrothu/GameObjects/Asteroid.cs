namespace SpaceShipFartrothu.GameObjects
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Asteroid
    {
        public Rectangle BoundingBox;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Origin;

        public float RotationAngle;
        public int Speed;
        public bool IsVisible;

        readonly Random randomSpawn = new Random();

        public float RandomX;
        public float RandomY;

        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            this.Position = newPosition;
            this.Texture = newTexture;
            this.Speed = 4;
            this.RandomX = this.randomSpawn.Next(0, 1200);
            this.RandomY = this.randomSpawn.Next(-700, -50);
            this.IsVisible = true;
        }

        public void LoadContent(ContentManager content)
        {
            this.Texture = content.Load<Texture2D>("asteroid");
            this.Origin.X = this.Texture.Width / 2;
            this.Origin.Y = this.Texture.Height / 2;
        }

        public void Update(GameTime gemeTime)
        {
            this.BoundingBox = new Rectangle((int)this.Position.X, (int)this.Position.Y, 45, 45);

            this.Position.Y += this.Speed;
            if (this.Position.Y >= 950)
            {
                this.Position.Y = -50;
            }

            float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            this.RotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            this.RotationAngle %= circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(
                    this.Texture,
                    this.Position,
                    null,
                    null,
                    this.Origin,
                    this.RotationAngle,
                    null,
                    Color.White,
                    SpriteEffects.FlipVertically);
            }
        }
    }
}