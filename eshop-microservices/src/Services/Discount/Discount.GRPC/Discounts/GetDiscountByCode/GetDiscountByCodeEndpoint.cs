using Carter;
using Discount.GRPC.Models;
using Mapster;
using MediatR;

namespace Discount.GRPC.Discounts.GetdDiscountByCode
{
    public class GetDiscountByCodeEndpoint : ICarterModule
    {
        public record GetDiscountByCodeResponse(Coupon Coupon);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("discount/{code}", async (string code, ISender sender) =>
            {
                var request = new GetDiscountByCodeRequest(code);
                var result = await sender.Send(request);
                var response = result.Adapt<GetDiscountByCodeResponse>();

                return response;
            }).WithName("GetDiscountByCode")
            .Produces<GetDiscountByCodeResponse>()
            .WithDescription("GetDiscountByCode");
        }
    }
}
