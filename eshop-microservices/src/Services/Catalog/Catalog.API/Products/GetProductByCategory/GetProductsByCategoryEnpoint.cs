
namespace Catalog.API.Products.GetProductByCategory
{
        public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async ([FromQuery] string category, [FromServices] ISender sender) =>
            {
                var resutl = sender.Send(new GetProductsByCategoryQuery(category));
                var response = resutl.Adapt<GetProductsByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductsByCategory")
            .WithDescription("Get Product By Category")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
