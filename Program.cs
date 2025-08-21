using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ABC.Retail.Data;
using ABC.Retail.Services;
namespace ABC.Retail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ABCRetailContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ABCRetailContext") ?? throw new InvalidOperationException("Connection string 'ABCRetailContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
                return new BlobService(connectionString);
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
