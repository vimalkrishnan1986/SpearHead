using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace SpearHead.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly DbContext _dbContext;
        bool disposed = false;

        public DbContext DbContext
        {
            get { return _dbContext; }
        }


        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
