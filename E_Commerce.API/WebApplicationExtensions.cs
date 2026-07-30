using E_Commerce.Domain.Contracts;

namespace E_Commerce.API
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedAndMigrateAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await seeder.SeedDataAsync();
            await Identityseeder.SeedDataAsync();

            return app; 
        }

    }
}
