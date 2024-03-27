
namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(int Id) : ICommand<DeleteProductResult> { }
    public record DeleteProductResult(bool deleted);
    public class DeleteProductHandler(IDocumentSession session) : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
                session.Delete<Product>(command.Id);
                await session.SaveChangesAsync(cancellationToken);
                return new DeleteProductResult(true);

        }
    }

}
