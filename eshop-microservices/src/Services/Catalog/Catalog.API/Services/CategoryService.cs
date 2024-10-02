
using BuildingBlocks.Pagination;
using System.Linq.Expressions;

namespace Catalog.API.Services
{
    public class CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoryService> _logger = logger;


        public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.CategoryRepository.Create(category);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;
        }

        public async Task<bool> DeleteAsync(Category category, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;
        }

        public async Task<IEnumerable<Category>> FindAsync(PaginationRequest pagination, Expression<Func<Category, bool>> predicate, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.CategoryRepository.Find(predicate).AsQueryable().ToListAsync(cancellationToken);
            return result;
        }

        public async Task<IEnumerable<Category>> GetAllAsync( PaginationRequest pagination ,CancellationToken token)
        {
            return await _unitOfWork.CategoryRepository.GetAll().AsQueryable().ToListAsync(token);
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.CategoryRepository.GetAsync(id);
        }

        public async Task<Category> UpdateAsync(Category category, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.CompeleteAsync(cancellationToken);
            return result;
        }
    }
}
