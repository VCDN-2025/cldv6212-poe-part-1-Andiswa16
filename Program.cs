using Azure.Storage.Blobs;
using Azure.Data.Tables;
using ABC_Retail_.Services;

namespace ABC_Retail_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ✅ Azure Storage Connection String
            string azureStorageConnectionString = builder.Configuration.GetConnectionString("AzureStorage");

            // ✅ Register Azure Blob Service
            builder.Services.AddSingleton(x => new BlobServiceClient(azureStorageConnectionString));

            // ✅ Register Azure Table Service
            builder.Services.AddSingleton(x => new TableServiceClient(azureStorageConnectionString));

            // ✅ Register your custom services so DI can inject them
            builder.Services.AddSingleton<TablesStorageService>();
            builder.Services.AddSingleton<FilesStorageService>();
            builder.Services.AddSingleton<BlobStorageService>();
            builder.Services.AddSingleton<QueuesStorageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
