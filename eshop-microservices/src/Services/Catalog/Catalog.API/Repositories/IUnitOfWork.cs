namespace Catalog.API.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<ProductImage> ImageRepository { get; }

        Task CompeleteAsync(CancellationToken cancellationToken);
    }
}
