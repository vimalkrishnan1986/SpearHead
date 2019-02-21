using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace SpearHead.Data.Infrastructure
{
    public interface IRepository<T>
    {

        T Single(object PrimaryKey);
        T SingleOrDefault(object PrimaryKey);
        IEnumerable<T> GetAll();
        void Insert(T Entity);
        void Update(T Entity);
        IEnumerable<T> FilterBy(Func<T, bool> filter);
        IUnitOfWork UnitofWork { get; }
        void Delete(T Entity);

    }
}
