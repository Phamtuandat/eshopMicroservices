namespace Catalog.API.Commands.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<int> Categories, string Description, decimal Price, IFormFileCollection Images);
    public record UpdateProductResponse(bool IsSuccess);
    public class UpdateProductEnpont : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapPut("/products", async ([FromBody] UpdateProductRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product");

        }
    }
}
