
namespace Catalog.API.Repositories
{
    public class UnitOfwork(CatalogDbContext context) : IUnitOfWork
    {
        private readonly CatalogDbContext _context = context;

        private IRepository<Product> productRepository = default!;
        private IRepository<Category> categoryRepository = default!;
        private IRepository<ProductImage> imageRepository = default!;

        public IRepository<Product> ProductRepository
        {
            get
            {
                productRepository ??= new ProductRepository(_context);
                return productRepository;
            }
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                categoryRepository ??= new CategoryRepository(_context);
                return categoryRepository;
            }
        }

        public IRepository<ProductImage> ImageRepository
        {
            get
            {
                imageRepository ??= new ProductImageRepository(_context);
                return imageRepository;
            }
        }


        public async Task CompeleteAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
