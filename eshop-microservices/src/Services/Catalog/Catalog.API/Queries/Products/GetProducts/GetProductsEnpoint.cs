using BuildingBlocks.Pagination;

namespace Catalog.API.Queries.Products.GetProducts
{
    public record GetProductRequest(int Page = 0, int PageSize = 10);
    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductsEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductRequest request, [FromServices] ISender sender) =>
            {
                var query = new GetProductsQuery(new PaginationRequest(request.Page, request.PageSize));
                var result = await sender.Send(query);
                var response = result.Adapt<GetProductResponse>();
                return Results.Ok(response);

            })
                .WithName("GetProducts")
                .WithDescription("Get Products")
                .Produces<GetProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get Products");
        }
    }
}
