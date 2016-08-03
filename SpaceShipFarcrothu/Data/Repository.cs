namespace SpaceShipFartrothu.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class Repository<T> : IRepository<T> where T : IGameObject
    {
        // private IDictionary<string, IList<IGameObject>> allGameObjects;

        public Repository()
        {
            this.ItemsByEntity = new List<T>();
        }

        protected IList<T> ItemsByEntity { get; set; }

        public List<T> GetAll()
        {
            return this.ItemsByEntity.ToList();
        }

        public void AddEntity(T entity)
        {
            this.ItemsByEntity.Add(entity);
        }

        public void Dispose()
        {
           this.ItemsByEntity.Clear();
        }

        public int GetCount()
        {
            return this.ItemsByEntity.Count;
        }

        public void RemoveAt(int index)
        {
            this.ItemsByEntity.RemoveAt(index);
        }

        public void RemoveGameObject(IGameObject gameObjectToRemove)
        {
            throw new System.NotImplementedException();
        }
    }


}
