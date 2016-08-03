namespace SpaceShipFartrothu.Interfaces
{
    using System.Collections.Generic;

    public interface IRepository<T> where T : IGameObject
    {
        //IDictionary<string, IList<IGameObject>> GetAllGameobjects();

        // List<T> ItemsByEntity { get; }

        List<T> GetAll();

        void AddEntity(T entity);

        void Dispose();

        int GetCount();

        void RemoveAt(int index);
        //void RemoveGameObject(IGameObject gameObjectToRemove);
    }
}
