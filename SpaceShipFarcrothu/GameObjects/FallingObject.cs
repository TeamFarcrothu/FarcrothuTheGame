namespace SpaceShipFartrothu.GameObjects
{
    using Microsoft.Xna.Framework;

    public abstract class FallingObject : GameObject
    {
        protected FallingObject(Vector2 position)
            : base(position)
        {
        }
    }
}
