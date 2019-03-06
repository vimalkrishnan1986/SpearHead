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
    public sealed class ExcelUploadService : IExcelUploadService
    {
        private readonly IUploadBusinessService _businessService;


        public ExcelUploadService(IUploadBusinessService uploadBusinessService)
        {
            _businessService = uploadBusinessService ?? throw new ArgumentNullException(nameof(uploadBusinessService));
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
