using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using SpearHead.DataContracts;

namespace SpearHead.ServiceContracts
{
    [ServiceContract]
    public interface IExcelUploadService
    {
        [OperationContract(Name = "Upload")]
        [FaultContract(typeof(FaultModel))]
        Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model);
    }
}
