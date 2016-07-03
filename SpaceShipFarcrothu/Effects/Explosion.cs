using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShipFartrothu.Effects
{
    public class Explosion
    {
        private Texture2D texture;
        private Vector2 position;
        private float timer;
        private float interval;
        private Vector2 origin;
        private int frameRow, frameCol, spriteWidth, spriteHeight;
        private Rectangle sourceRect;
        public bool isVisible;

        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            this.position = newPosition;
            this.texture = newTexture;
            this.timer = 0f;
            this.interval = 20;
            this.frameRow = 1;
            this.spriteWidth = 100;
            this.spriteHeight = 100;
            this.isVisible = true;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            this.timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (this.timer > this.interval)
            {
                this.frameCol++;
                this.timer = 0f;
            }
            if (this.frameCol == 8)
            {
                if (this.frameRow == 8)
                {
                    this.frameRow = 0;
                    this.isVisible = false;
                }
                this.frameRow++;
                this.frameCol = 0;
            }
            this.sourceRect = new Rectangle(this.frameCol *this.spriteWidth, this.frameRow *this.spriteHeight, this.spriteWidth, this.spriteHeight);
            this.origin = new Vector2(this.sourceRect.Width / 2, this.sourceRect.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.isVisible == true)
            {
                spriteBatch.Draw(this.texture, this.position, this.sourceRect, Color.White, 0f, this.origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}