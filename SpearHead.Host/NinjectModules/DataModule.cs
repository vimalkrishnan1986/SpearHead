using Ninject.Modules;
using System;
using SpearHead.Data.Infrastructure;
using SpearHead.Data.Entities;

namespace SpearHead.Host.NinjectModules
{
    public class DataModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IUnitOfWork>().ToConstant(new UnitOfWork(new SchoolEntities()));
        }
    }
}
