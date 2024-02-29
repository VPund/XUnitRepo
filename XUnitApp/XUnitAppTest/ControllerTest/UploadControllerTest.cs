using Azure;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Text;
using XUnitApp.Controllers;
using XUnitApp.Interface;

namespace XUnitAppTest.ControllerTest
{

    public class UploadControllerTest
    {
        private UploadFileController _controller;
        private IAzureBlobService AzureBlobService = Substitute.For<IAzureBlobService>();

        public UploadControllerTest()
        {
            _controller = new UploadFileController(AzureBlobService);
        }

        /// <summary>
        /// Upload File on the storage a/c
        /// </summary>
        [Fact]
        public async void UploadFile_ReturnOKStatus()
        {
            //Arrange
            var formFile = CreateTestFormFile("Testfile", "this is test data");
            ETag eTag;
            byte[] data = Encoding.UTF8.GetBytes("test");
            var t = BlobsModelFactory.BlobContentInfo(eTag,DateTime.Now,data,"1","test","all",324 );
            var val = Response.FromValue(t, Substitute.For<Response>());

            AzureBlobService.UploadFiles(formFile).Returns(val);

            //Act
            var result = await _controller.UploadFile(formFile);

            //Assert
            Assert.NotNull(result);
            Assert.True(result is ObjectResult { StatusCode: 200 });
            
        }

        private IFormFile CreateTestFormFile(string fileName, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            return new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
        }

        /// <summary>
        /// Read File from the storage a/c
        /// </summary>
        [Fact]
        public async void ReadFile_ReturnOKStatus()
        {
            //Arrange
            var filetName = "testfile.txt";
            var t = BlobsModelFactory.BlobDownloadInfo(DateTime.Now);
            var val = Response.FromValue(t, Substitute.For<Response>());

            AzureBlobService.ReadFiles(filetName).Returns(val);

            //Act
            var result = await _controller.ReadFile(filetName);

            //Assert
            Assert.NotNull(result);
            Assert.True(result is ObjectResult { StatusCode: 200 });
        }

        /// <summary>
        /// Read File from the storage a/c
        /// </summary>
        [Fact]
        public async void ReadFile_ReturnBadrequestStatus()
        {
            //Arrange
            var filetName = "";
            var t = BlobsModelFactory.BlobDownloadInfo(DateTime.Now);
            var val = Response.FromValue(t, Substitute.For<Response>());

            AzureBlobService.ReadFiles(filetName).Returns(val);

            //Act
            var result = await _controller.ReadFile(filetName);

            //Assert
            Assert.NotNull(result);
            Assert.True(result is ObjectResult { StatusCode: 400 });
        }
    }
}
