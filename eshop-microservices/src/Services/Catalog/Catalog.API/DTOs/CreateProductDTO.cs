namespace Catalog.API.DTOs
{
    public record CreateProductDTO
    {
        public string Name { get; } = default!;
        public string Description { get; } = default!;
        public Guid Id { get; }
        public List<ProductImage> Images { get; }

        public List<int> Categories { get; }
        public decimal Price { get;  }
        public string CreateBy { get;}

        public CreateProductDTO(Guid id, string name, string desc, List<ProductImage> images, List<int> categories, string createBy, decimal price)
        {
            Id = id;
            Name = name;
            Description = desc;
            Images = images;
            Categories = categories;
            Price = price;
            CreateBy = createBy;
        }
    }
}
