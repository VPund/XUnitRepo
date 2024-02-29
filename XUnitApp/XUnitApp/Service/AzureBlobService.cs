using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using XUnitApp.Interface;

namespace XUnitApp.Service
{
    public class AzureBlobService : IAzureBlobService
    {
        BlobServiceClient _blobClient;
        private readonly IConfiguration _configuration;
        BlobContainerClient _containerClient;
        public AzureBlobService(IConfiguration Configuration)
        {
            _configuration = Configuration;
            _blobClient = new BlobServiceClient(_configuration.GetSection("BlobStorgaeInfo:ConnectionStrings").Value.ToString());
            _containerClient = _blobClient.GetBlobContainerClient(_configuration.GetSection("BlobStorgaeInfo:StorageName").Value.ToString());
        }

        public async Task<Azure.Response<BlobContentInfo>> UploadFiles(IFormFile file)
        {

            var azureResponse = new List<Azure.Response<BlobContentInfo>>();

            string fileName = file.FileName;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Position = 0;
                var client = await _containerClient.UploadBlobAsync(fileName, memoryStream, default);
                azureResponse.Add(client);
            }

            return azureResponse.FirstOrDefault();
        }

        public async Task<Azure.Response<BlobDownloadInfo>> ReadFiles(string filename)
        {

            BlobClient blobClient = _containerClient.GetBlobClient(filename);
            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
               
                return response;
            }
            return null;      
        }
    }
}
