using Catalog.API.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Catalog.API.Commands.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<int> Categories, string Description, decimal Price, IFormFileCollection Images)
        : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Images).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductCommandHandler
        (IProductService productService, IProductImageService productImageService)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductImageService _imageService = productImageService;
        private readonly IProductService _productService = productService;
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
                var imageList = new List<ProductImage>();
            try
            {
                foreach (var item in command.Images)
                {
                    var id = Guid.NewGuid();
                    StoreImageDTO newImage = new(id, item, command.Name);
                    var image = await _imageService.StoreAsync(newImage, cancellationToken);
                    imageList.Add(new ProductImage()
                    {
                        Id = id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = "Dat",
                        FilePath = image.FilePath,
                        Url = image.ImgageUrl,
                        Title = command.Name
                    });
                }
                var product = new CreateProductDTO(Guid.NewGuid(), command.Name, command.Description, imageList, command.Categories, "dat", command.Price);
                var result = await _productService.CreateAsync(product, cancellationToken);

                return new CreateProductResult(result.Id);
            }
            catch (Exception)
            {
                foreach (var item in imageList)
                {
                    File.Delete(item.FilePath);
                }
                throw;
            }
        }
    }
}
