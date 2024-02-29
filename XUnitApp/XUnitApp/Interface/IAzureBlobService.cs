using Azure.Storage.Blobs.Models;

namespace XUnitApp.Interface
{
    public interface IAzureBlobService
    {
        Task<Azure.Response<BlobContentInfo>> UploadFiles(IFormFile files);
        Task<Azure.Response<BlobDownloadInfo>> ReadFiles(string filename);
    }
}
