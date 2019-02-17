using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpearHead.Common.ExcelReader;
using SpearHead.Common.Helpers;
using SpearHead.BusinessServices.Repositories;
using SpearHead.DataContracts;
using System.Reflection;
using System.Data;
using System.Net;
using System.IO;

namespace SpearHead.BusinessServices
{
    public class UploadBusinessService : IUploadBusinessService
    {
        private readonly IStorageRepsitory _storageRepsitory;
        private readonly IExcelReader _excelReader;
        const string app_data = "App_Data";

        private string GetTemperotyFileName()
        {
            string extension1 = ".xls";
            string extension2 = ".xlsx";
            string fileName = Path.GetRandomFileName();
            if (!fileName.EndsWith(extension1) && !fileName.EndsWith(extension2))
            {
                fileName = $"{fileName}{extension2}";
            }
            return $"{app_data}\\{fileName}";

        }

        public UploadBusinessService(IStorageRepsitory storageRepsitory, IExcelReader excelReader)
        {
            _storageRepsitory = storageRepsitory ?? throw new ArgumentNullException(nameof(storageRepsitory));
            _excelReader = excelReader ?? throw new ArgumentNullException(nameof(excelReader));

        }

        public async Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model)
        {
            string tempFileName = GetTemperotyFileName();
            //saving to temprory file;
            FileHelper.CreateDirectory(app_data);
            FileHelper.WriteToFile(model.Content, tempFileName);
            //performing the validation

            var dataTable = await _excelReader.ReadFromExcel(tempFileName);
            var validationResults = ApplyValidation(dataTable);
            if (validationResults.HttpStatusCode != StatusCodes.Sucess)
            {
                FileHelper.Delete(tempFileName);
                return validationResults;
            }
            if (!await _storageRepsitory.Move(tempFileName))
            {
                throw new OperationCanceledException("Could not copy the files");
            }

            return new ExcelUploadResponseModel(null);
        }


        /// <summary>
        /// apply your validations inside the method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ExcelUploadResponseModel ApplyValidation(DataTable data)
        {

            List<ErrorMessageModel> errorMessageModels = new List<ErrorMessageModel>();

            if (data == null)
            {
                throw new InvalidOperationException();
            }

            // performing the validation here

            if (data.Rows.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }

            int row = 0;

            foreach (DataRow dataRow in data.Rows)
            {
                bool isPassed = true;
                var errorMessageModel = new ErrorMessageModel(row);

                if (dataRow[0].ToString() != "vimal")
                {
                    errorMessageModel.ErrorMessagees.Add("Coulm 0 should have value vimal");
                    isPassed = false;
                }

                /// contine with other checks over here;
                row++;
                if (isPassed)
                {
                    continue;
                }
                errorMessageModels.Add(errorMessageModel);
            }

            return new ExcelUploadResponseModel(errorMessageModels);
        }
    }
}
