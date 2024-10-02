
namespace Catalog.API.Models
{
    public class Product : EntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public virtual ICollection<Category> Categories { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string[] ImageUrls  => Images?.Select(x => x.Url).ToArray() ?? [];
        public decimal Price { get; set; } = default!;

        public virtual ICollection<ProductImage> Images { get; set; } = default!;

      
    }
}
