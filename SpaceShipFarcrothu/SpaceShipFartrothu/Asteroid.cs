namespace SpaceShipFartrothu
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;

        public float rotationAngle;
        public int speed;
        public bool isVisible;

        Random randomSpawn = new Random();
        public float randomX;
        public float randomY;

        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            this.position = newPosition;
            this.texture = newTexture;
            this.speed = 4;
            this.randomX = this.randomSpawn.Next(0, 2000);
            this.randomY = this.randomSpawn.Next(-1000, -50);
            this.isVisible = true;
        }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("asteroid");
            this.origin.X = this.texture.Width / 2;
            this.origin.Y = this.texture.Height / 2;
        }

        public void Update(GameTime gemeTime)
        {
            this.boundingBox = new Rectangle((int)this.position.X, (int)this.position.Y, 45, 45);

            this.position.Y += this.speed;
            if (this.position.Y >= 950)
            {
                this.position.Y = -50;
            }

            float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            this.rotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            this.rotationAngle %= circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.isVisible)
            {
                spriteBatch.Draw(
                    this.texture,
                    this.position,
                    null,
                    null,
                    this.origin,
                    this.rotationAngle,
                    null,
                    Color.White,
                    SpriteEffects.FlipVertically);
            }
        }
    }
}