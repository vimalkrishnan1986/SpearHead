using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using SpearHead.Data.Infrastructure;

namespace SpearHead.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;


        public IUnitOfWork UnitofWork
        {
            get; private set;
        }

        public T Single(object PrimaryKey)
        {
            return dbSet.Find(PrimaryKey);
        }


        public T SingleOrDefault(object PrimaryKey)
        {
            return dbSet.Find(PrimaryKey);

        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList<T>();

        }

        public void Insert(T Entity)
        {
            dbSet.Add(Entity);
        }

        public void Update(T Entity)
        {
            dbSet.Attach(Entity);
            UnitofWork.DbContext.Entry(Entity).State = EntityState.Modified;
        }

        public IEnumerable<T> FilterBy(Func<T, bool> filter)
        {
            return dbSet.Where(filter).AsEnumerable<T>();
        }

        public BaseRepository(IUnitOfWork unitofWork)
        {
            UnitofWork = unitofWork ?? throw new ArgumentNullException(nameof(unitofWork));
            dbSet = UnitofWork.DbContext.Set<T>();

        }

        public void Delete(T Entity)
        {
            dbSet.Remove(Entity);
        }
    }
}
