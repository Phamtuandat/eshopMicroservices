using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Catalog.API.Data
{
    public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<ProductImage>().HasKey(p => p.Id);


            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images);

            modelBuilder.Entity<Category>().HasData(
                    new Category {
                        Id = 1,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Dat",
                        Description = "",
                        Name = "Book",
                        Products = new List<Product>(),
                    },
                     new Category
                     {
                         Id = 2,
                         CreatedAt = DateTime.UtcNow,  
                         CreatedBy = "Dat",
                         Description = "",
                         Name = "Shoe",
                         Products = new List<Product>(),
                     }
                );
        }
    }
}
