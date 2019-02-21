using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpearHead.Common.ExcelReader;
using SpearHead.Common.Helpers;
using SpearHead.BusinessServices.Repositories;
using SpearHead.DataContracts;
using System.Data;
using System.Net;
using System.IO;
using System.Linq;
using SpearHead.BusinessServices.Models;
using SpearHead.Data.Infrastructure;
using SpearHead.Data.Entities;
using SpearHead.Data.Repositories;

namespace SpearHead.BusinessServices
{
    public class UploadBusinessService : IUploadBusinessService
    {
        private readonly IStorageRepsitory _storageRepsitory;
        private readonly IExcelReader _excelReader;
        private readonly IUnitOfWork _unitOfWork;
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

        public UploadBusinessService(IStorageRepsitory storageRepsitory, IExcelReader excelReader, IUnitOfWork unitOfWork)
        {
            _storageRepsitory = storageRepsitory ?? throw new ArgumentNullException(nameof(storageRepsitory));
            _excelReader = excelReader ?? throw new ArgumentNullException(nameof(excelReader));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

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
            IRepository<Product> productRepository = new ProductRepository(_unitOfWork);
            IRepository<Packet> packetRepsitory = new PacketRepository(_unitOfWork);

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

            var products = productRepository.GetAll();
            var packets = packetRepsitory.GetAll();

            var inputData = data.ToListof<InputDataModelV1>();
            int row = 0;

            List<ProductDetail> productDetailsList = new List<ProductDetail>();

            inputData.RemoveAll((inputModel) =>
        {
            bool canDelete = false;
            var errorMessageModel = new ErrorMessageModel(row);
            var packet = packets.FirstOrDefault(p => p.PacketCode.Equals(inputModel.Packet, StringComparison.InvariantCultureIgnoreCase));
            if (packet == null)
            {
                errorMessageModel.ErrorMessagees.Add($" Row {row}, packet code {inputModel.Packet} does not exists");
                canDelete = true;
            }
            var product = products.FirstOrDefault(p => p.ProductCode.Equals(inputModel.Product, StringComparison.InvariantCultureIgnoreCase));
            if (product == null)
            {
                errorMessageModel.ErrorMessagees.Add($" Row {row}, product code {inputModel.Packet} does not exists");
                canDelete = true;
            }

            row++;
            if (canDelete)
            {
                errorMessageModels.Add(errorMessageModel);
            }
            else
            {
                productDetailsList.Add(new ProductDetail
                {
                    ProductId = product.ProductId,
                    PacketId = packet.PacketId
                    //add the other values over here
                });
            }

            return canDelete;
        });

            if (productDetailsList.Count() == inputData.Count())
            {
                IRepository<ProductDetail> productDetailrepository = new ProductDetailRepository(_unitOfWork);

                productDetailsList.ForEach(detaiils =>
                {
                    productDetailrepository.Insert(detaiils);
                });
                _unitOfWork.SaveChanges();
                return new ExcelUploadResponseModel(null);
            }

            return new ExcelUploadResponseModel(errorMessageModels);
            //save functionality here 

        }
    }
}
