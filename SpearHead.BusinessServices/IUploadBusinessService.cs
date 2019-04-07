using SpearHead.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpearHead.BusinessServices.Models;

namespace SpearHead.BusinessServices
{
    public interface IUploadBusinessService
    {
        Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model);

        Task<ExcelUploadResponseModelV1<SallaryModel>> Validate(ExcelUploadModel model);

        Task<ExcelUploadResponseModel> Validate_Workflow(ExcelUploadModel model);
    }
}
