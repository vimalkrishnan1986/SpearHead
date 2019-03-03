using System;
using System.ServiceModel;
using System.Threading.Tasks;
using SpearHead.ServiceContracts;
using SpearHead.BusinessServices;
using SpearHead.Common.ExcelReader;
using SpearHead.DataContracts;
using SpearHead.Data.Infrastructure;
using SpearHead.Data.Entities;
using SpearHead.Common.Proxy;

namespace SpearHead.Contracts.Implementation
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public sealed class ExcelUploadService : IExcelUploadServicecs
    {
        private readonly IUploadBusinessService _businessService;
        private readonly IExcelReader _excelReader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStoreProxy _fileStoreProxy;
        

        public ExcelUploadService()
        {
            _excelReader = Activator.CreateInstance<OleDbExcelReader>();
            _unitOfWork = new UnitOfWork(new SchoolEntities());
            _fileStoreProxy = Activator.CreateInstance<FileStoreProxy>();
            _businessService = new UploadBusinessService(_excelReader, _unitOfWork,_fileStoreProxy);
        }

        public async Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model)
        {
            try
            {
                return await _businessService.Upload(model);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultModel>(new FaultModel
                { Exception = ex.InnerException, Message = ex.Message });
            }
        }
    }
}
