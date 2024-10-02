using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Repositories
{

    public abstract class GenericRepository<T>(CatalogDbContext context) : IRepository<T> where T : class
    {
        protected CatalogDbContext _context = context;

        public virtual T Create(T entity)
        {
            return _context.Add(entity).Entity;
        }

        public virtual bool Delete(T entity)
        {
            var entityEntry = _context.Remove(entity);
            return entityEntry.Entity != null;
        }

        public virtual IEnumerable<T> Find( Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>()
                .Where(predicate);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public virtual  async Task<T?> GetAsync(Guid id)
        {
            return await _context.FindAsync<T>(id);
        }

        public virtual T Update(T entity)
        {
            return _context.Update(entity).Entity;
        }
        public virtual async Task SaveChangesAsync(CancellationToken token)
        {
            await _context.SaveChangesAsync(token);
        }
    }
}
