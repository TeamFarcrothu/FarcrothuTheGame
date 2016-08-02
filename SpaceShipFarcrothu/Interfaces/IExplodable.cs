namespace SpaceShipFartrothu.Interfaces
{
    using System.Collections.Generic;

    public interface IExplodable
    {
        void ColideAndExplode(IList<IExplosion> explosions);
    }
}
