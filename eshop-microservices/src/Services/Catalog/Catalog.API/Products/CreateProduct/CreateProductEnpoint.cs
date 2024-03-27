
namespace Catalog.API.Model.CreateProduct
{
    public record CreateProductRequest(string name, List<string> categories, string description) { }
    public record CreateProductResponse() { public Guid Id; }
    public class CreateProductEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async ([FromBody] CreateProductRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                var result = sender.Send(command);
                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/Products/{response.Id}", response);

            })
            .WithName("CreateProdut")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product"); 
        }
    }
}
