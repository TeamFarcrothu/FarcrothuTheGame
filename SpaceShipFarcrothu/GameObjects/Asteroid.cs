namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Asteroid
    {
        public Rectangle BoundingBox;
        private Texture2D Texture;
        public Vector2 Position;

        // Rotation is not working at the moment in order to get proper collision
        // public Vector2 Origin;
        //public float RotationAngle;
        private int Speed;
        public bool IsVisible;

        //public float RandomX;
        //public float RandomY;

        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            this.Position = newPosition;
            this.Texture = newTexture;
            this.Speed = 4;
            this.IsVisible = true;

            // Rotation is not working at the moment in order to get proper collision
            //this.Origin.X = Texture.Width / 2;
            //this.Origin.Y = Texture.Height / 2;
        }

        //public void LoadContent(ContentManager content)
        //{
        //}

        public void Update(GameTime gemeTime)
        {
            this.BoundingBox = new Rectangle((int) this.Position.X, (int) this.Position.Y, this.Texture.Width, this.Texture.Height);

            this.Position.Y += this.Speed;
            if (this.Position.Y >= 768)
            {
                this.Position.Y = -75;
            }

            // Rotation is not working at the moment in order to get proper collision

            //float elapsed = (float)gemeTime.ElapsedGameTime.TotalSeconds;
            //RotationAngle += elapsed;
            //float circle = MathHelper.Pi * 2;
            //RotationAngle %= circle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                spriteBatch.Draw(this.Texture, this.Position,Color.White);

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
    }
}