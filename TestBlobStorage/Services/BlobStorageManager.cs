using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using TestBlobStorage.Utilities;

namespace TestBlobStorage.Services
{
    public class BlobStorageManager : IStorageManager
    {
        public readonly BlobStorageOptions _storageOptions;

        public BlobStorageManager(IOptions<BlobStorageOptions> options)
        {
            _storageOptions = options.Value;
        }

        public bool DeleteFile(string fileName)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return  blobClient.DeleteIfExists();
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }

        public string GetSignedUrl(string fileName)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var contaionerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = contaionerClient.GetBlobClient(fileName);

            var signedUrl = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.Now.AddHours(2)).AbsoluteUri;

            return signedUrl;
        }

        public bool UploadFile(Stream stream, string fileName, string contentType)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            
            blobClient.Upload(stream);
            return true;
        }

        public async Task<bool> UploadFileAsync(Stream stream, string fileName, string contentType)
        {
            var serviceClient = new BlobServiceClient(_storageOptions.ConnectionString);
            var containerClient = serviceClient.GetBlobContainerClient(_storageOptions.ContainerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(stream);
            return true;
        }
    }
}
