namespace SpaceShipFartrothu.Interfaces
{
    using System.Collections.Generic;

    public interface IRepository
    {
        IDictionary<string, IList<IGameObject>> GetAllGameobjects();

        IList<IPlayer> GetPlayers();

        IList<IEnemy> GetEnemies();

        IList<IAsteroid> GetAsteroids();

        IList<IBullet> GetBullets();




        void RemoveGameObject(IGameObject gameObjectToRemove);
    }
}
