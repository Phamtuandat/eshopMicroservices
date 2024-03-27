﻿

namespace Catalog.API.Model.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
    : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductHandler(IDocumentSession session) : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Category = request.Category,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,

            };
            session.Store<Product>(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(Guid.NewGuid());
        }
    }
}
