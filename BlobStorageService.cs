using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail_.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            // Get the connection string and container name from appsettings
            string connectionString = configuration.GetConnectionString("AzureBlobStorage");
            string containerName = configuration["AzureBlobStorageSettings:ContainerName"];

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString), "Azure Blob Storage connection string is missing.");

            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException(nameof(containerName), "Blob container name is missing.");

            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
            _blobContainerClient.CreateIfNotExists();
        }

        /// <summary>
        /// Uploads an image to Blob Storage and returns the URL
        /// </summary>
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string blobName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

            using (var stream = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Deletes an image from Blob Storage
        /// </summary>
        public async Task DeleteImageAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
                return;

            var blobName = Path.GetFileName(new Uri(blobUrl).AbsolutePath);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Gets the URL of an existing blob (optional helper)
        /// </summary>
        public string GetBlobUrl(string blobName)
        {
            return _blobContainerClient.GetBlobClient(blobName).Uri.ToString();
        }
    }
}
