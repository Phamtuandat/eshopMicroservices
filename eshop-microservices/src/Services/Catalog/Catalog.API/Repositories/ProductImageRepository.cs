namespace Catalog.API.Repositories
{
    public class ProductImageRepository(CatalogDbContext context) : GenericRepository<ProductImage>(context)
    {
    }
}
