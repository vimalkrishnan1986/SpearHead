using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using SpearHead.ServiceContracts;
using SpearHead.BusinessServices;
using SpearHead.BusinessServices.Repositories;
using SpearHead.Common.ExcelReader;
using SpearHead.Common.Helpers;
using SpearHead.DataContracts;

namespace SpearHead.Contracts.Implementation
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public sealed class ExcelUploadService : IExcelUploadServicecs
    {
        private readonly IUploadBusinessService _businessService;
        private readonly IExcelReader _excelReader;
        private readonly IStorageRepsitory _storageRepository;
        const string _baseLocationKey = "storageLocation";

        public ExcelUploadService()
        {
            _excelReader = Activator.CreateInstance<OleDbExcelReader>();
            _storageRepository = Activator.CreateInstance<FileRepository>();
            _storageRepository.Configure(ConfigHelper.GetAppSettingValue<string>(_baseLocationKey));
            _businessService = new UploadBusinessService(_storageRepository, _excelReader);
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
