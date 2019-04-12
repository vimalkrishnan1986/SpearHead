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
using SpearHead.Common.Proxy;
using SpearHead.Data.Entities;
using SpearHead.DataContracts;

using FluentAssertions;

namespace SpearHead.Tests.Business
{
    [TestClass]
    public class ExcelUploadBusinessTest
    {
        private const string baseLocation = "TestData";
        private string _baseLocationKey = "storageLocation";
        private IUploadBusinessService _uploadBusinessService;
        private IFileStoreProxy _fileStoreProxy;
        private IExcelReader _excelReader;
        private IUnitOfWork _unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            _fileStoreProxy = new FileStoreProxy();
            _excelReader = new OleDbExcelReader();
            _unitOfWork = new UnitOfWork(new SchoolEntities());
            _uploadBusinessService = new UploadBusinessService(_excelReader, _unitOfWork, _fileStoreProxy);
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
        public async Task ExcelUploadBusinessTest_ValidateFile_Sucess()
        {

            var model = new ExcelUploadModel()
            {
                Name = "test",
                Content = FileHelper.ReadBytes($"{baseLocation}//Sample Excel_valid.xlsx")
            };


            var res = await _uploadBusinessService.Validate(model);
            res.Should().NotBeNull();
            res.HttpStatusCode.Should().Be(StatusCodes.BadRequest);
            var age20FailedResponse = res.ErrorMessages.Find(p => p.Row.Equals(2));
            age20FailedResponse.Content.Row.Should().Be(2);

            var age23FailedResponse = res.ErrorMessages.Find(p => p.Row.Equals(4));
            age20FailedResponse.Should().NotBeNull();
            age23FailedResponse.Should().NotBeNull();

            age20FailedResponse.Content.Should().NotBeNull();
            age23FailedResponse.Content.Should().NotBeNull();
            age20FailedResponse.Content.Row.Should().Be(2);
            age23FailedResponse.Content.Row.Should().Be(4);
            // content will have the vallues

        }

        [TestMethod]
        public async Task ExcelUploadBusinessTest_ValidateFile_Workflow_Sucess()
        {

            var model = new ExcelUploadModel()
            {
                Name = "test",
                Content = FileHelper.ReadBytes($"{baseLocation}//Sample Excel_valid.xlsx")
            };


            var res = await _uploadBusinessService.Validate_Workflow(model);
            res.Should().NotBeNull();
            res.HttpStatusCode.Should().Be(StatusCodes.BadRequest);
            var age20FailedResponse = res.ErrorMessages.Find(p => p.Row.Equals(2)).Should().NotBeNull();
            var age23FailedResponse = res.ErrorMessages.Find(p => p.Row.Equals(4)).Should().NotBeNull();



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
