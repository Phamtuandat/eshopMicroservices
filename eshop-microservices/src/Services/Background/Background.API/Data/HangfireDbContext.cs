using Microsoft.EntityFrameworkCore;

namespace Background.API.Data
{
    public class HangfireDbContext(DbContextOptions<HangfireDbContext> options) : DbContext(options)
    {

        // Define any DbSets if needed
    }
}
