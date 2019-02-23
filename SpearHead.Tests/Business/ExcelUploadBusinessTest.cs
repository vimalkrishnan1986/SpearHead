using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpearHead.BusinessServices;
using SpearHead.BusinessServices.Repositories;
using SpearHead.Common.ExcelReader;
using SpearHead.Data.Infrastructure;
using SpearHead.Common.Helpers;
using SpearHead.Data.Entities;
using SpearHead.DataContracts;
using FluentAssertions;
using System.Transactions;

namespace SpearHead.Tests.Business
{
    [TestClass]
    public class ExcelUploadBusinessTest
    {
        private const string baseLocation = "TestData";
        private string _baseLocationKey = "storageLocation";
        private IUploadBusinessService _uploadBusinessService;
        private IStorageRepsitory _storageRepsitory;
        private IExcelReader _excelReader;
        private IUnitOfWork _unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            _storageRepsitory = new FileRepository();
            _storageRepsitory.Configure(ConfigHelper.GetAppSettingValue<string>(_baseLocationKey));
            _excelReader = new OleDbExcelReader();
            _unitOfWork = new UnitOfWork(new SchoolEntities());
            _uploadBusinessService = new UploadBusinessService(_storageRepsitory, _excelReader, _unitOfWork);
        }


        [TestMethod]
        public async Task ExcelUploadBusinessTest_UploadFiles_Sucess()
        {

            var model = new ExcelUploadModel()
            {
                Name = "test",
                Content = FileHelper.ReadBytes($"{baseLocation}//Sample Excel_valid.xlsx")
            };


            var res = await _uploadBusinessService.Upload(model);
            res.Should().NotBeNull();
            res.HttpStatusCode.Should().Be(StatusCodes.Sucess);


        }

        [TestMethod]
        public async Task ExcelUploadBusinessTest_UploadFiles_Failure()
        {

            var model = new ExcelUploadModel()
            {
                Name = "test",
                Content = FileHelper.ReadBytes($"{baseLocation}//Sample Excel_Invalid.xlsx")
            };


            var res = await _uploadBusinessService.Upload(model);
            res.Should().NotBeNull();
            res.HttpStatusCode.Should().Be(StatusCodes.BadRequest);
            res.ErrorMessages.Count().Should().Be(2);


        }
    }
}
