using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync(PaginationRequest pagination ,CancellationToken cancellationToken);
        Task<Product> CreateAsync(CreateProductDTO model, CancellationToken cancellationToken);
        Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Product product,  CancellationToken cancellationToken);
        Task<Product?> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> FindAsync(PaginationRequest pagination ,Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken);

    }
}
