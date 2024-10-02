namespace Catalog.API.Models
{
    public class ProductImage : EntityBase
    {
        public Guid Id { get; set; } = default!;
        public Guid ProductId { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string FilePath { get; set; } = default!;
    }
}
