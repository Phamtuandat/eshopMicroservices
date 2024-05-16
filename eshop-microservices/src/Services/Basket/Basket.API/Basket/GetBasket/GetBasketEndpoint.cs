
using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint() : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/basket/{userName}", async (string username, ISender sender) =>
            {
                var query = new GetBasketQuery(username);
                var result = await sender.Send(query);
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("GetBasketByUsername")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get basket By username")
            .WithDescription("Get basket By username");
        }
    }
}
