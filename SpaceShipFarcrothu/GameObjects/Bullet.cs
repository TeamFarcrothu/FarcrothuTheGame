namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Bullet
    {
        public Rectangle BoundingBox;
        public Texture2D Texture;
        public Vector2 Origin;
        public Vector2 Position;
        public bool IsVisible;
        public float Speed;

        public Bullet(Texture2D newTexture)
        {
            this.Speed = 10;
            this.Texture = newTexture;
            this.IsVisible = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, Color.White);
        }
    }
}
