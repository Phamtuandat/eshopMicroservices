using Carter;
using Mapster;
using MediatR;

namespace Discount.GRPC.Discounts.DeleteDiscount
{
    public record DeleteDiscountResponse(bool IsSuccess);
    public class DeleteDiscountEnpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)    
        {
            app.MapDelete("discount", async (Guid id, ISender sender) =>
            {
                var commnad = new DeleteDiscountCommand(id);
                var result = await sender.Send(commnad);

                var response = result.Adapt<DeleteDiscountResponse>();
                return response;
            }).RequireAuthorization("admin")
              .ProducesProblem(StatusCodes.Status500InternalServerError).Produces<DeleteDiscountResponse>()
              .WithName("DeleteDiscount");
        }

    }
}
