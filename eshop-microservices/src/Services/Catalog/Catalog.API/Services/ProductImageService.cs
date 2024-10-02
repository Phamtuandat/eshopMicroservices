
using BuildingBlocks.Pagination;
using System.Text.RegularExpressions;

namespace Catalog.API.Services
{
    public class ProductImageService(IUnitOfWork unitOfWork, ILogger<ProductImageService> logger, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : IProductImageService
    {
        private readonly ILogger<ProductImageService> _logger = logger;
        private readonly IUnitOfWork _unitOfWorl = unitOfWork;
        private readonly IWebHostEnvironment _env = env;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;

        public async Task<bool> DeleteImageAsync(Guid imageId, CancellationToken cancellationToken)
        {
            var image = await _unitOfWorl.ImageRepository.GetAsync(imageId);
            if (image == null) throw new ImageNotfountException(imageId.ToString());
            File.Delete(image.FilePath);
            var result = _unitOfWorl.ImageRepository.Delete(image);
            await _unitOfWorl.CompeleteAsync(cancellationToken);
            return result;
        }

        public async Task<IEnumerable<ProductImage>> GetImageByProductId(PaginationRequest pagination, Guid productId, CancellationToken cancellationToken)
        {
            return await _unitOfWorl.ImageRepository.Find(x => x.ProductId == productId).AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProductImage>> GetImagesAsync(PaginationRequest pagination, CancellationToken cancellationToken)
        {

            return await _unitOfWorl.ImageRepository.GetAll().AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<StoreImageResultDTO> StoreAsync(StoreImageDTO image, CancellationToken cancellationToken)
        {
            var staticPath = string.Empty;
            if(_env.IsDevelopment())
            {
                staticPath = "";
            }
            else
            {
                staticPath = _env.WebRootPath;
            }
            try
            {
                var request = (_contextAccessor.HttpContext?.Request) ?? throw new InvalidOperationException("Cannot access the current HTTP context.");
                var file = image.ImageFile;
                var fileExtension = Path.GetExtension(image.ImageFile.FileName);
                var imageId = Guid.NewGuid();
                var newFileName = $"{imageId}{fileExtension}";
                newFileName = _RemoveSpecialCharacters(newFileName);
                var path = Path.Combine(_env.WebRootPath, "Images/Product", newFileName);
                using var stream = new FileStream(path, FileMode.Create);
                string filePath = Path.Combine("Images", "Product", newFileName);
                await file.CopyToAsync(stream, cancellationToken);
                string url = $"{request.Scheme}://{request.Host}/{filePath}";
                _logger.LogInformation(path);

                return new StoreImageResultDTO(path, url);

            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task<ProductImage> UpdateAsync(StoreImageDTO imageDTO, CancellationToken cancellationToken)
        {
            var image = await GetAsync(imageDTO.Id);
            if (image == null) throw new ImageNotfountException(imageDTO.ToString());

            var result = _unitOfWorl.ImageRepository.Update(image);
            await _unitOfWorl.CompeleteAsync(cancellationToken);
            return result;
        }


        public async Task<ProductImage?> GetAsync(Guid imageId)
        {
            var image = await _unitOfWorl.ImageRepository.GetAsync(imageId);
            return image;
        }

        private string _RemoveSpecialCharacters(string input)
        {
            // Regex pattern to match all characters except letters, digits, hyphens, underscores, periods, and slashes
            string pattern = @"[^a-zA-Z0-9-_.]";

            // Replace all matched characters with an empty string
            return Regex.Replace(input, pattern, "");
        }
    }
}
