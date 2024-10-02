using BuildingBlocks.Pagination;

namespace Catalog.API.Services
{
    public interface IProductImageService
    {
        Task<StoreImageResultDTO> StoreAsync(StoreImageDTO image, CancellationToken cancellationToken);
        Task<IEnumerable<ProductImage>> GetImagesAsync(PaginationRequest pagination,CancellationToken cancellationToken);
        Task<bool> DeleteImageAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<ProductImage>> GetImageByProductId(PaginationRequest? pagination,Guid  productId, CancellationToken cancellationToken);
        Task<ProductImage> UpdateAsync(StoreImageDTO image, CancellationToken cancellationToken);

        Task<ProductImage?> GetAsync(Guid id );

    }
}
