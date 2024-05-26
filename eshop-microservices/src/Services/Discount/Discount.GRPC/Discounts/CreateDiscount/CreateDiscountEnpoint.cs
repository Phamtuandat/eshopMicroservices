using BuildingBlocks.CQRS;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Discount.GRPC.Discounts.CreateDiscount
{
    public record CreateDiscountRequest(string Code, string Description, decimal Amount, int Quantity);
    public record CreateDiscountResponse(bool IsSuccess);
    public class CreateDiscountEnpoint : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("discount", async ([FromBody] CreateDiscountRequest request, ISender sender) => { var command = request.Adapt<CreateDiscountCommand>(); var result = await sender.Send(command); var response = result.Adapt<CreateDiscountResponse>(); return response; })
                .RequireAuthorization("admin")
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .Produces<CreateDiscountResponse>()
                .WithName("CreateDiscount");
        }
    }
}
