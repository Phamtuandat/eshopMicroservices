

namespace Catalog.API.Repositories
{
    public class CategoryRepository(CatalogDbContext context) : GenericRepository<Category>(context)
    {
        public override async Task<Category?> GetAsync(Guid id)
        {
            return await base.GetAsync(id);
        }
    }

}
