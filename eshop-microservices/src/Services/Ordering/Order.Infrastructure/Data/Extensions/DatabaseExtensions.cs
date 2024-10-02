﻿

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class  DatabaseExtensions 
    {
        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
        }

    }
}
