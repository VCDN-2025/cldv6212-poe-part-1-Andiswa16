using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ABC_Retail_.Services
{
    public class QueuesStorageService
    {
        private readonly QueueClient _queueClient;

        public QueuesStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorage");
            string queueName = configuration["AzureQueueSettings:QueueName"]; // Get from appsettings
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists(); // creates queue if missing
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _queueClient.SendMessageAsync(message);
            }
        }
    }
}
