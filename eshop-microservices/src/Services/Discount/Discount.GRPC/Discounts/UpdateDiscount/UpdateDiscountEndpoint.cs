using Carter;
using Discount.GRPC.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Discount.GRPC.Discounts.UpdateDiscount
{
    public record UpdateDiscountRequest(Coupon Coupon);
    public record UpdateDiscountResponse(bool IsSuccess);
    public class UpdateDiscountEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("discount", async ([FromBody] UpdateDiscountRequest req, ISender sender) =>
            {
                var command = req.Adapt<UpdateDiscountCommand>();
                var result = await sender.Send(command);

                var response = result.Adapt<UpdateDiscountResponse>();


            }).WithName("UpdateDiscount")
            .WithDescription("Update Discount")
            .Produces<UpdateDiscountResponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("admin");
            
        }

    }
}
