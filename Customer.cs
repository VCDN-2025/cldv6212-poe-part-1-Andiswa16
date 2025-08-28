using Azure;
using Azure.Data.Tables;
using System;

namespace ABC_Retail_.Models
{
    // ------------------- Customer -------------------
    public class Customer : ITableEntity
    {
        public string PartitionKey { get; set; } = "customers";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}