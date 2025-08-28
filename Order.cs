using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail_.Models
{
    public class Order : ITableEntity
    {
        public string PartitionKey { get; set; } = "orders";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string CustomerId { get; set; } = string.Empty;  // RowKey of Customer
        public string ProductId { get; set; } = string.Empty;   // RowKey of Product
        public int Quantity { get; set; } = 1;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";        // e.g., "Pending", "Shipped"
    }
}