using SpearHead.DataContracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SpearHead.BusinessServices
{
    public interface IUploadBusinessService
    {
        Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model);

        Task<ExcelUploadResponseModel> Validate(ExcelUploadModel model);
    }
}
