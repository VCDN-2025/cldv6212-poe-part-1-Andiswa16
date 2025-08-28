using Azure.Storage.Files.Shares;
using System.Text.Json;
using System.Text;
using ABC_Retail_.Models;

namespace ABC_Retail_.Services
{
    public class FilesStorageService
    {
        private readonly ShareClient _shareClient;
        private readonly string _directoryName;

        public FilesStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorage");
            string shareName = configuration["AzureFileShareSettings:ShareName"];
            _directoryName = configuration["AzureFileShareSettings:DirectoryName"];

            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        private ShareDirectoryClient GetDirectoryClient()
        {
            var directory = _shareClient.GetDirectoryClient(_directoryName);
            directory.CreateIfNotExists();
            return directory;
        }

        public async Task SaveOrderAsync(Order order)
        {
            order.PartitionKey = Guid.NewGuid().ToString();
            string fileName = $"Order_{order.PartitionKey}_{DateTime.UtcNow.Ticks}.json";
            string json = JsonSerializer.Serialize(order);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            var directory = GetDirectoryClient();
            var fileClient = directory.GetFileClient(fileName);
            using var stream = new MemoryStream(bytes);
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, stream.Length), stream);
        }

        public async Task<List<string>> ListOrdersAsync()
        {
            var directory = GetDirectoryClient();
            var files = new List<string>();
            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
                if (!item.IsDirectory) files.Add(item.Name);
            return files;
        }

        public async Task<Order> GetOrderAsync(string fileName)
        {
            var directory = GetDirectoryClient();
            var fileClient = directory.GetFileClient(fileName);

            if (await fileClient.ExistsAsync())
            {
                var download = await fileClient.DownloadAsync();
                using var reader = new StreamReader(download.Value.Content);
                string content = await reader.ReadToEndAsync();
                return JsonSerializer.Deserialize<Order>(content);
            }
            return null;
        }

        public async Task DeleteOrderAsync(string fileName)
        {
            var fileClient = GetDirectoryClient().GetFileClient(fileName);
            if (await fileClient.ExistsAsync()) await fileClient.DeleteAsync();
        }

        public async Task UpdateOrderAsync(string fileName, Order order)
        {
            await DeleteOrderAsync(fileName);
            await SaveOrderAsync(order);
        }
    }
}
