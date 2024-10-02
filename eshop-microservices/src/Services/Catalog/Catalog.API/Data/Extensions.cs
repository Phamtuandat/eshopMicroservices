namespace Catalog.API.Data
{
    public static class Extensions
    {
        public static async Task InitialiseDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
            await context.Database.MigrateAsync();
        }

    }


    
}
