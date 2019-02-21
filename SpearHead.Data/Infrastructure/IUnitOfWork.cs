using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace SpearHead.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext DbContext { get; }
        void SaveChanges();


    }
}
