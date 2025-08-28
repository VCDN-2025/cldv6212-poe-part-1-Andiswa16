using Azure.Storage.Blobs;
using Azure.Data.Tables;
using ABC_Retail_.Services;
using static System.Collections.Specialized.BitVector32;

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

//Disclosure of AI Usage in my Assessment:
//	Section which generative AI was used: POE Part 1 
//	Name of AI tool used: Chatgpt
//	Purpose/Intention behind use: -To provide guides when fixing my issues and the cause of the issues
//	-offer step-by-step support in building the interface/UI of the webisite

//	Date in which generative AI was used: 20 August 2025
//	Link to the generative AI: https://chatgpt.com/c/67e1a2d1-16f8-800f-b35e-d8f6d4f3a1ab

