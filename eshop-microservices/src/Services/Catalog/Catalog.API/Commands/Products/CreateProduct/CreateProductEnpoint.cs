namespace Catalog.API.Commands.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<int> Categories, string Description, decimal Price, IFormFileCollection Images) { }
    public record CreateProductResponse(Guid Id);
    public class CreateProductEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async ([FromForm] CreateProductRequest request, [FromServices] ISender sender) =>
            {
                var command = new CreateProductCommand(request.Name, request.Categories, request.Description, request.Price, request.Images);
                var result = await sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/Products/{response.Id}", response);
            })
            .WithName("CreateProdut")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product")
            .DisableAntiforgery();
        }
    }
}
