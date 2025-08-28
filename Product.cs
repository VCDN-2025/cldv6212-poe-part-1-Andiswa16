using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail_.Models
{
    public class Product : ITableEntity
    {
        public string PartitionKey { get; set; } = "products";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Category { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
        public string ImageUrl { get; set; } = string.Empty;
    }
}