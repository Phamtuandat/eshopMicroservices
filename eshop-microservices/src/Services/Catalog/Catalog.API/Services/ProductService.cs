
using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Services
{
    public class ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ProductService> _logger = logger;

        public async Task<Product> CreateAsync(CreateProductDTO model, CancellationToken cancellationToken)
        {
            var categories = _unitOfWork.CategoryRepository.Find(x => model.Categories.Contains(x.Id));
            var product = new Product()
            {
                Id = model.Id,
                Name = model.Name,
                Images = model.Images,
                Categories = categories.ToList(),
                CreatedAt = DateTime.UtcNow,
                Description = model.Description,
                Price = model.Price,
                CreatedBy = model.CreateBy,

            };
            var result = _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;

        }

        public async Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;

        }

        public async Task<IEnumerable<Product>> FindAsync(PaginationRequest pagination, Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProductRepository.Find(predicate).AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProductRepository
                .GetAll()
                .AsQueryable()
                .Include(x => x.Images)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.ProductRepository.GetAsync(id);
        }

        public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;
        }
    }
}
