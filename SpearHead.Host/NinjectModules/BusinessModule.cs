using SpearHead.BusinessServices;
using SpearHead.Common.ExcelReader;
using SpearHead.Common.Proxy;
using Ninject.Modules;

namespace SpearHead.Host.NinjectModules
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IUploadBusinessService>().To<UploadBusinessService>();
            Kernel.Bind<IFileStoreProxy>().To<FileStoreProxy>();
            Kernel.Bind<IExcelReader>().To<OleDbExcelReader>();
        }
    }
}
