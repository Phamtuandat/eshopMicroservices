using BuildingBlocks.Pagination;

namespace Catalog.API.Queries.Products.GetProductByCategory
{
    public record GetProductsByCategoryQuery(string Category, PaginationRequest Pagination) : IQuery<GetProductsByCategoryResult>;
    public record GetProductsByCategoryResult(IEnumerable<Product> Products);
    internal class GetProductsByCategoryHandler(IProductService productService) : IRequestHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        private readonly IProductService _productService = productService;
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
        {
            var products = await _productService.FindAsync(query.Pagination, x => x.Categories.Select(x => x.Name).Contains(query.Category), cancellationToken);
            return new GetProductsByCategoryResult(products);
        }

    }
}
