using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpearHead.BusinessServices;
using SpearHead.ServiceContracts;
using SpearHead.Contracts.Implementation;
using SpearHead.DataContracts;
using Moq;
using FluentAssertions;
using System.ServiceModel;

namespace SpearHead.Tests.Service
{
    [TestClass]
    public class ExcelUploadServiceTest
    {
        private Mock<IUploadBusinessService> _mockUploadBusinessService;


        [TestInitialize]

        public void Initilize()
        {
            _mockUploadBusinessService = new Mock<IUploadBusinessService>();
        }

        [TestMethod]
        public async Task ExcelUploadServiceTest_Upload_Success()
        {
            // Assemble
            _mockUploadBusinessService.Setup(p => p.Upload(It.IsAny<ExcelUploadModel>())).
                Returns(Task.FromResult(new ExcelUploadResponseModel(null)));
            IExcelUploadService excelUploadService = new ExcelUploadService(_mockUploadBusinessService.Object);

            //Act
            var result = await excelUploadService.Upload(new ExcelUploadModel
            {
                Name = "test",
                Content = It.IsAny<byte[]>()
            });

            ///Assert
            result.HttpStatusCode.Should().Be(StatusCodes.Sucess);

        }

        [TestMethod]
        public async Task ExcelUploadServiceTest_Upload_Failure()
        {
            // Assemble
            _mockUploadBusinessService.Setup(p => p.Upload(It.IsAny<ExcelUploadModel>())).
                Returns(Task.FromResult(new ExcelUploadResponseModel(new List<ErrorMessageModel>() { new ErrorMessageModel(0) })));
            IExcelUploadService excelUploadService = new ExcelUploadService(_mockUploadBusinessService.Object);

            //Act
            var result = await excelUploadService.Upload(new ExcelUploadModel
            {
                Name = "test",
                Content = It.IsAny<byte[]>()
            });

            ///Assert
            result.HttpStatusCode.Should().Be(StatusCodes.BadRequest);

        }

        [TestMethod]
        [ExpectedException(typeof(FaultException))]
        public async Task ExcelUploadServiceTest_Upload_Exception()
        {
            // Assemble
            _mockUploadBusinessService.Setup(p => p.Upload(It.IsAny<ExcelUploadModel>())).Throws<Exception>();

            IExcelUploadService excelUploadService = new ExcelUploadService(_mockUploadBusinessService.Object);

            //Act
            var result = await excelUploadService.Upload(new ExcelUploadModel
            {
                Name = "test",
                Content = It.IsAny<byte[]>()
            });

            ///Assert
            result.HttpStatusCode.Should().Be(StatusCodes.Error);

        }
    }
}
