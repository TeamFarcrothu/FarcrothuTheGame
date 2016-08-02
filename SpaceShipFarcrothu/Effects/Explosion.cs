namespace SpaceShipFartrothu.Effects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Core;
    using GameObjects;
    using Interfaces;

    public class Explosion : GameObject, IExplosion
    {
        private float timer;
        private float interval;
        private Vector2 origin;
        private int frameRow, frameCol, spriteWidth, spriteHeight;
        private Rectangle sourceRect;

        public Explosion(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            //this.Texture = texture;
            this.timer = 0f;
            this.interval = 20;
            this.frameRow = 1;
            this.spriteWidth = 100;
            this.spriteHeight = 100;
            this.IsVisible = true;
        }

       // public new Texture2D Texture { get; set; }

        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("explosion");
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


            this.sourceRect = new Rectangle(this.frameCol * this.spriteWidth, this.frameRow * this.spriteHeight, this.spriteWidth, this.spriteHeight);
            this.origin = new Vector2(this.sourceRect.Width / 2, this.sourceRect.Height / 2);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.IsVisible)
            {
                //spriteBatch.Draw(this.texture, this.position, this.sourceRect, Color.White, 0f, this.origin, 1.0f, SpriteEffects.None, 0);
                spriteBatch.Draw(this.Texture, this.Position, this.sourceRect, Color.White, 0f, this.origin, 1.0f, SpriteEffects.None, 0);
            }
        }

        public override void ReactOnColission(IGameObject target = null)
        {
            //play sound?
            this.IsVisible = false;
        }
    }
}