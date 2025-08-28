using Azure;
using Azure.Data.Tables;
using ABC_Retail_.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC_Retail_.Services
{
    public class TablesStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;

        public TablesStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureStorage");

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("AzureStorage connection string is missing in appsettings.json");

            // Initialize Table Clients
            _customerTable = new TableClient(connectionString, "customers");
            _customerTable.CreateIfNotExists();

            _productTable = new TableClient(connectionString, "products");
            _productTable.CreateIfNotExists();
        }

        #region Customer Operations

        public async Task AddCustomerAsync(Customer customer)
        {
            customer.PartitionKey = "customers";
            customer.RowKey = Guid.NewGuid().ToString();
            await _customerTable.AddEntityAsync(customer);
        }

        public async Task<Customer> GetCustomerAsync(string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey)) return null;

            try
            {
                var response = await _customerTable.GetEntityAsync<Customer>("customers", rowKey);
                return response?.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();
            await foreach (var c in _customerTable.QueryAsync<Customer>(c => c.PartitionKey == "customers"))
                customers.Add(c);

            return customers;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (customer == null) throw new ArgumentNullException(nameof(customer));
            await _customerTable.UpdateEntityAsync(customer, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteCustomerAsync(string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey)) return;
            await _customerTable.DeleteEntityAsync("customers", rowKey);
        }

        #endregion

        #region Product Operations

        public async Task AddProductAsync(Product product)
        {
            product.PartitionKey = "products";
            product.RowKey = Guid.NewGuid().ToString();
            await _productTable.AddEntityAsync(product);
        }

        public async Task<Product> GetProductAsync(string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey)) return null;

            try
            {
                var response = await _productTable.GetEntityAsync<Product>("products", rowKey);
                return response?.Value;
            }
            catch (RequestFailedException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            await foreach (var p in _productTable.QueryAsync<Product>(p => p.PartitionKey == "products"))
                products.Add(p);

            return products;
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await _productTable.UpdateEntityAsync(product, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteProductAsync(string rowKey)
        {
            if (string.IsNullOrEmpty(rowKey)) return;
            await _productTable.DeleteEntityAsync("products", rowKey);
        }

        #endregion
    }
}
