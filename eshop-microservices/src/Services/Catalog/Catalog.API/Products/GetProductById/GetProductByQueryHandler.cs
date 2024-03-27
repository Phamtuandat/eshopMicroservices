namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product? Product);

    internal class GetProductByIdQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await session.Query<Product>().FirstOrDefaultAsync(x => x.Id == query.Id);

            return new GetProductByIdResult(product);
        }
    }
}
