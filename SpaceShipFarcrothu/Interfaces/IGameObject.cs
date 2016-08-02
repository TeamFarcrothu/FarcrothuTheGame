namespace SpaceShipFartrothu.Interfaces
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IGameObject
    {
        Texture2D Texture { get; }

        Rectangle BoundingBox { get; }

        Vector2 Position { get; }

        bool IsVisible { get; }

        int Speed { get; }

        int Damage { get; }

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);

        void ReactOnColission(IGameObject target = null);
    }
}
