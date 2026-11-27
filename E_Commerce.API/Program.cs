
using E_Commerce.Application;
using E_Commerce.Application.Profiles;
using E_Commerce.Application.Services;
using E_Commerce.Infrastucture;
using E_Commerce.Infrastucture.Identity.Services;
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
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.Configure<PaymentGatewaySettings>(builder.Configuration.GetSection("Stripe"));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ReactPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            await app.SeedAndMigrateAsync();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            };
            var filesPath = Path.Combine(builder.Environment.ContentRootPath, "Files");

            if (!Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(filesPath),
                RequestPath = "/Files"
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseHttpsRedirection();

            app.UseCors("ReactPolicy");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapFallbackToFile("index.html");
            app.Run();
            Console.WriteLine(builder.Environment.ContentRootPath);

        }
    }
}
