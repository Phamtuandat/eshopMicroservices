using FluentValidation;

namespace Catalog.API.Commands.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult> { }
    public record DeleteProductResult(bool Deleted);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
        }
    }
    internal class DeleteProductHandler(IProductService productService, IProductImageService imageService) : IRequestHandler<DeleteProductCommand, DeleteProductResult>
    {
        private readonly IProductImageService _imageService = imageService;
        private readonly IProductService _productService = productService;
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            var product = await _productService.GetByIdAsync(command.Id);
            if (product == null) throw new ProductNotFoundException(command.Id);
            var images = await _imageService.GetImageByProductId(null, product.Id, cancellationToken);
            foreach (var item in images)
            {
                await _imageService.DeleteImageAsync(item.Id, cancellationToken);
            }
            var result = await _productService.DeleteAsync(product, cancellationToken);
            return new DeleteProductResult(result);

        }
    }

}
