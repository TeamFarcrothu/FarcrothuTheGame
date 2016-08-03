namespace SpaceShipFartrothu.Factories
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Effects;
    using Interfaces;

    public static class ExplosionFactory
    {
        public static IExplosion CreateExplosion(Vector2 position)
        {
            return new Explosion(position);
        }
    }
}
