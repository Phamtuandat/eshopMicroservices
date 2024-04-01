
using static Catalog.API.Model.CreateProduct.CreateProductEnpoint;

namespace Catalog.API.Products.DeleteProduct
{
    public class DeleteProductEnpoint : ICarterModule
    {
        public record DeleteProductRequest( int Id) { };
        public record DeleteProductResponse(bool Deleted);

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products", async ([FromBody] DeleteProductRequest request, [FromServices] ISender sender) =>
            {
                var command = request.Adapt<DeleteProductCommand>();
                var result = sender.Send(command);
                var response = result.Adapt<DeleteProductResponse>();
                return response;
            })
                .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status202Accepted)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("Delete Product"); 
        }
    }
}
