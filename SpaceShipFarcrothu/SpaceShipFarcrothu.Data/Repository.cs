namespace SpaceShipFarcrothu.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using Interfaces;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> set;

        public Repository(DbSet<TEntity> set)
        {
            this.set = set;
        }

        public void Add(TEntity entity)
        {
            this.set.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this.set.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            this.set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this.set.RemoveRange(entities);
        }

        public TEntity Find(int id)
        {
            return this.set.Find(id);
        }

        public TEntity First(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.FirstOrDefault(expression);
        }

        public TEntity First()
        {
            return this.set.FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return this.set;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.Where(expression);
        }

        public int Count()
        {
            return this.set.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> expression)
        {
            return this.set.Count(expression);
        }

        public void Clear()
        {
            this.set.Local.Clear();
        }
    }
}