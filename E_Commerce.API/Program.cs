
using E_Commerce.Application;
using E_Commerce.Application.Profiles;
using E_Commerce.Infrastucture;
using Microsoft.Extensions.FileProviders;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfraStructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.Configure <UrlSetting>(builder.Configuration.GetSection("UrlSettings"));
            var app = builder.Build();
            await app.SeedAndMigrateAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Files")),
                RequestPath = "/Files"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            Console.WriteLine(builder.Environment.ContentRootPath);
        }
    }
}
