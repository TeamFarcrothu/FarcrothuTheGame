namespace SpaceShipFartrothu.Interfaces
{
    using System.Collections.Generic;

    public interface IRepository<T> where T : IGameObject
    {
        List<T> GetAll();

        void AddEntity(T entity);

        void Dispose();

        int GetCount();

        void RemoveAt(int index);
    }
}
