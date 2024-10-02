using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(PaginationRequest pagination,CancellationToken token);
        Task<Category> CreateAsync(Category category, CancellationToken cancellationToken);
        Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken);
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> FindAsync(PaginationRequest pagination, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken);

    }
}
