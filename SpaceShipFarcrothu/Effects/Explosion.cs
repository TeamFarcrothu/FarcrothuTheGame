using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceShipFartrothu.Core;
using SpaceShipFartrothu.GameObjects;

namespace SpaceShipFartrothu.Effects
{
    public class Explosion : GameObject
    {
        private float timer;
        private float interval;
        private Vector2 origin;
        private int frameRow, frameCol, spriteWidth, spriteHeight;
        private Rectangle sourceRect;

        public static List<Explosion> Explosions = new List<Explosion>();
        //public static Texture2D explosionTexture;

        public Explosion(Vector2 position)
            :base(null, position)
        {
            this.timer = 0f;
            this.interval = 20;
            this.frameRow = 1;
            this.spriteWidth = 100;
            this.spriteHeight = 100;
            this.isVisible = true;

            Explosions.Add(this);
        }

        public void LoadContent(ContentManager Content)
        {
          
        }

        public override void Update(GameTime gameTime)
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

            //Clear invisible explosions
            for (int i = 0; i < Explosions.Count; i++)
            {
                if (!Explosions[i].IsVisible)
                {
                    Explosions.RemoveAt(i);
                    i--;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.isVisible)
            {
                //spriteBatch.Draw(this.texture, this.position, this.sourceRect, Color.White, 0f, this.origin, 1.0f, SpriteEffects.None, 0);
                spriteBatch.Draw(GameEngine.explosionTexture, this.Position, this.sourceRect, Color.White, 0f, this.origin, 1.0f, SpriteEffects.None, 0);
            }
        }

        public override void ReactOnColission(GameObject target = null)
        {           
        }
    }
}