namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class StarField
    {
        private Texture2D Texture;
        private Vector2 BackGroundPosition1, BackGroundPosition2;
        public int Speed;

        public StarField()
        {
            this.Texture = null;
            this.BackGroundPosition1 = new Vector2(0, 0);
            this.BackGroundPosition2 = new Vector2(0, -768);
            this.Speed = 1;
        }

        public void LoadContent(ContentManager content)
        {
            this.Texture = content.Load<Texture2D>("space");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.BackGroundPosition1, Color.White);
            spriteBatch.Draw(this.Texture, this.BackGroundPosition2, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            this.BackGroundPosition1.Y = this.BackGroundPosition1.Y + this.Speed;
            this.BackGroundPosition2.Y = this.BackGroundPosition2.Y + this.Speed;

            if (this.BackGroundPosition1.Y >= 768)
            {
                this.BackGroundPosition1.Y = 0;
                this.BackGroundPosition2.Y = -768;
            }
        }
    }
}