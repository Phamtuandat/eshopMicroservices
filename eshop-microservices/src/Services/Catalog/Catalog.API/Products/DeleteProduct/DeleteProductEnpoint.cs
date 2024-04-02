
using static Catalog.API.Model.CreateProduct.CreateProductEnpoint;

namespace Catalog.API.Products.DeleteProduct
{
    public class DeleteProductEnpoint : ICarterModule
    {
        public record DeleteProductRequest( int Id) { };
        public record DeleteProductResponse(bool Deleted);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, [FromServices] ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));

                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
                .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product"); 
        }
    }
}
