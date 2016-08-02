namespace SpaceShipFartrothu.Factories
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Effects;
    using Interfaces;

    public static class ExplosionFactory
    {
        public static IExplosion CreateExplosion(Texture2D texture, Vector2 position)
        {
            return new Explosion(texture, position);
        }
    }
}
