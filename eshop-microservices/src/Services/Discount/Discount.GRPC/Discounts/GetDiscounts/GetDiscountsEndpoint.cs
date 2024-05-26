using Carter;
using Discount.GRPC.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.GRPC.Discounts.GetDiscounts
{
    public record GetDiscountsRequest(int Page = 0, int PageSize = 10);
    public record GetDiscountsReponse(List<Coupon> Coupons);
    public class GetDiscountsEndpoint() : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("discount", async ([AsParameters] GetDiscountsRequest req, ISender sender) =>
            {
                var query = req.Adapt<GetDiscountsQuery>();

                var result = await sender.Send(query);

                return result.Adapt<GetDiscountsReponse>();


            }).WithName("GetDiscounts")
            .WithDescription("Get discounts")
            .Produces<GetDiscountsReponse>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
