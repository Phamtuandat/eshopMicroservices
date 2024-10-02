using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Repositories
{
    public interface IRepository<T>
    {
        T Create(T entity);
        T Update(T entity);
        bool Delete(T entity);
        Task<T?> GetAsync(Guid id);
        IEnumerable<T>  GetAll();
        Task SaveChangesAsync(CancellationToken token);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    }
}
