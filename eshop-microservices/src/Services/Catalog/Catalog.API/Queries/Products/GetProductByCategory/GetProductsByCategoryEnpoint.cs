using BuildingBlocks.Pagination;

namespace Catalog.API.Queries.Products.GetProductByCategory
{
    public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async ([FromQuery] string category, [FromQuery] int pageSize, [FromQuery] int page, [FromServices] ISender sender) =>
            {
                var pagination = new PaginationRequest(page, pageSize);
                var resutl = await sender.Send(new GetProductsByCategoryQuery(category, pagination));
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
