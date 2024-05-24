using Discount.GRPC.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } 

        public DiscountContext(DbContextOptions<DiscountContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Coupon>().HasKey(x => x.Id);
            modelBuilder.Entity<Coupon>().Property(x => x.Code).IsUnicode(true);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = Guid.NewGuid(), Description = "Newbie Discount", Amount = 150, Code = "NEWBIE24", Quantity= 100 },
                new Coupon { Id = Guid.NewGuid(), Description = "Freeship Discount", Amount = 100, Code = "FREESHIP05", Quantity=100 }

                );
        }
    }
}
