using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Linq;
using SpearHead.FileStore.Models;
using SpearHead.BusinessServices.Models;
using SpearHead.Data.Infrastructure;
using SpearHead.Data.Entities;
using SpearHead.Data.Repositories;
using SpearHead.Common.Proxy;
using SpearHead.Common.ExcelReader;
using SpearHead.Common.Helpers;
using SpearHead.DataContracts;
using System.Activities;
using SpearHead.BusinessServices.Workflows;
using System.Threading;

namespace SpearHead.BusinessServices
{
    public class UploadBusinessService : IUploadBusinessService
    {
        private readonly IExcelReader _excelReader;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStoreProxy _fileStoreProxy;
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

        public UploadBusinessService(IExcelReader excelReader, IUnitOfWork unitOfWork, IFileStoreProxy fileStoreProxy)
        {
            _fileStoreProxy = fileStoreProxy ?? throw new ArgumentNullException(nameof(fileStoreProxy));
            _excelReader = excelReader ?? throw new ArgumentNullException(nameof(excelReader));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }

        private async Task<DataTable> GetExcelTable(ExcelUploadModel model, string tempFileName)
        {

            //saving to temprory file;
            FileHelper.CreateDirectory(app_data);
            FileHelper.WriteToFile(model.Content, tempFileName);
            //performing the validation
            return await _excelReader.ReadFromExcel(tempFileName);
        }
        public async Task<ExcelUploadResponseModel> Upload(ExcelUploadModel model)
        {
            string tempFileName = GetTemperotyFileName();
            var dataTable = await GetExcelTable(model, tempFileName);
            var validationResults = ApplyValidation(dataTable);

            if (validationResults.HttpStatusCode != StatusCodes.Sucess)
            {
                FileHelper.Delete(tempFileName);
                return validationResults;
            }

            //this is not a recoomended  approch, instead use scheduler based approach to push the files into file store
            var uploadStatus = await _fileStoreProxy.Post(new FileModel
            {
                Name = model.Name,
                FileBytes = model.Content
            });

            if (!uploadStatus.IsSuccessStatusCode)
            {
                return new ExcelUploadResponseModel(new List<ErrorMessageModel>()
                { new ErrorMessageModel(0) { ErrorMessagees = new List<string>() { "Unable to upload the file to storage" }
                }
                }
                );
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
            int count = inputData.Count();

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

            if (productDetailsList.Count() == count)
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

        public async Task<ExcelUploadResponseModelV1<SallaryModel>> Validate(ExcelUploadModel model)
        {
            string tempFileName = GetTemperotyFileName();
            var dataTable = await GetExcelTable(model, tempFileName);

            List<ErrorMessageModelV1<SallaryModel>> errorMessageModels = new List<ErrorMessageModelV1<SallaryModel>>();

            if (dataTable == null)
            {
                throw new InvalidOperationException();
            }

            // performing the validation here

            if (dataTable.Rows.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }

            List<SallaryModel> sallaryModel = dataTable.ToListof<SallaryModel>();


            List<Predicate<SallaryModel>> rules = new List<Predicate<SallaryModel>>()
            {
                //age group 20 sallary should be greater than 50

               (m) =>
                       {
                            if (m.Age == 20 && m.Sallary < 50000)
                           {

                               errorMessageModels.Add(new ErrorMessageModelV1<SallaryModel>(Convert.ToInt32(m.No),m)
                               {
                                   ErrorMessages = new List<string>()
                                   {
                                "For age group 20 , minilum sallry would be 50000 "
                                   }
                               });
                               return true;
                           };

                           return false;
                       },

               // age group 22 and 23 sallry should not be greater than 50000;
                 (m) =>
                   {
                       if ((m.Age == 22 || m.Age == 23) && m.Sallary > 5000)
                       {
                          errorMessageModels.Add(new ErrorMessageModelV1<SallaryModel>(Convert.ToInt32(m.No),m)
                           {
                               ErrorMessages = new List<string>()
                                   {
                                "For age group 20 , minilum sallry should be less than 5000"
                                   }
                           });
                           return true;
                       };

                        return false;
                   }
            };

            foreach (var rule in rules)
            {
                sallaryModel.RemoveAll(rule);
            }

            return new ExcelUploadResponseModelV1<SallaryModel>(errorMessageModels);
        }

        public async Task<ExcelUploadResponseModel> Validate_Workflow(ExcelUploadModel model)
        {
            string tempFileName = GetTemperotyFileName();
            var dataTable = await GetExcelTable(model, tempFileName);

            List<ErrorMessageModel> errorMessageModels = new List<ErrorMessageModel>();

            if (dataTable == null)
            {
                throw new InvalidOperationException();
            }

            // performing the validation here

            if (dataTable.Rows.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }


            try
            {

                WorkflowInvoker workflowInvoker = new WorkflowInvoker(new ValidationSample());
                IDictionary<string, object> arguments = new Dictionary<string, object>();
                arguments.Add("InputTable", dataTable);
                var output = workflowInvoker.Invoke(arguments);
                Thread.Sleep(1000);
                var excelUploadResponseModel = output["ResponseList"] as ExcelUploadResponseModel;


                return excelUploadResponseModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
