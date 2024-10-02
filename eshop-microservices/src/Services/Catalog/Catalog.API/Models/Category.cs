namespace Catalog.API.Models
{
    public class Category : EntityBase
    { 
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public virtual List<Product> Products { get; set; } = default!;
    }
}
