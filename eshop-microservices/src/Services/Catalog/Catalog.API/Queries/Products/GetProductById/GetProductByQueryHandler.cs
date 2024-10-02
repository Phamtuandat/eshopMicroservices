namespace Catalog.API.Queries.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product? Product);

    internal class GetProductByIdQueryHandler(IProductService productService)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        private readonly IProductService _productService = productService;
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(query.Id);

            return new GetProductByIdResult(product);
        }
    }
}
