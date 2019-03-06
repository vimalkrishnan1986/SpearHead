using System.ServiceModel;
using Ninject.Extensions.Wcf;
using Ninject.Modules;

namespace SpearHead.Host.NinjectModules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ServiceHost>().To<NinjectServiceHost>();
        }
    }
}
