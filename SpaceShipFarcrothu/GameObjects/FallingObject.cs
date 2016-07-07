namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class FallingObject : GameObject
    {
        protected FallingObject(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
        }
    }
}
