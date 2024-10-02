namespace Catalog.API.Commands.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<int> Categories, string Description, decimal Price, IFormFileCollection Images) : ICommand<UpdateProductResult> { };
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

            RuleFor(command => command.Name)
                .NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

            RuleFor(command => command.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class UpdateProductHandler(IProductService productService) : IRequestHandler<UpdateProductCommand, UpdateProductResult>
    {
        private readonly IProductService _productService = productService;
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(command.Id);
            if (product == null) throw new ProductNotFoundException(command.Id);
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;

            var result = await _productService.UpdateAsync(product, cancellationToken);

            return new UpdateProductResult(result != null);
        }
    }



}
