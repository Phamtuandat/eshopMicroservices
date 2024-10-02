

using BuildingBlocks.Pagination;

namespace Catalog.API.Queries.Products.GetProducts
{
    public record GetProductsQuery(PaginationRequest Pagination) : IQuery<GetProductResult>;

    public record GetProductResult(IEnumerable<Product> Products);
    internal class GetProductsQueryHandler(IProductService productService) : IRequestHandler<GetProductsQuery, GetProductResult>
    {
        private readonly IProductService _productService = productService;
        public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync(query.Pagination, cancellationToken);
            return new GetProductResult(products);
        }
    }
}
