
namespace Catalog.API.Repositories
{
    public class ProductRepository(CatalogDbContext context) : GenericRepository<Product>(context)
    {

    }
}
