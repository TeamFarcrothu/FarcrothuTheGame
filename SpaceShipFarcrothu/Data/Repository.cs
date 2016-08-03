namespace SpaceShipFartrothu.Data
{
    using System.Collections.Generic;
    using Interfaces;

    public class Repository : IRepository
    {
        private IDictionary<string, IList<IGameObject>> allGameObjects;

        public Repository()
        {
            this.allGameObjects = new Dictionary<string, IList<IGameObject>>();
        }


        public IDictionary<string, IList<IGameObject>> GetAllGameobjects()
        {
            return new Dictionary<string, IList<IGameObject>>(this.allGameObjects);
        }

        public IList<IPlayer> GetPlayers()
        {
            throw new System.NotImplementedException();
        }

        public IList<IEnemy> GetEnemies()
        {
            throw new System.NotImplementedException();
        }

        public IList<IAsteroid> GetAsteroids()
        {
            throw new System.NotImplementedException();
        }

        public IList<IBullet> GetBullets()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveGameObject(IGameObject gameObjectToRemove)
        {
            throw new System.NotImplementedException();
        }
    }


}
