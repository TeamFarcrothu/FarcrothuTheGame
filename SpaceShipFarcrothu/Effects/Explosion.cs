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
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20;
            frameRow = 1;
            spriteWidth = 100;
            spriteHeight = 100;
            isVisible = true;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer > interval)
            {
                frameCol++;
                timer = 0f;
            }
            if (frameCol == 8)
            {
                if (frameRow == 8)
                {
                    frameRow = 0;
                    isVisible = false;
                }
                frameRow++;
                frameCol = 0;
            }
            sourceRect = new Rectangle(frameCol * spriteWidth, frameRow * spriteHeight, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible == true)
            {
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }
        }
    }
}