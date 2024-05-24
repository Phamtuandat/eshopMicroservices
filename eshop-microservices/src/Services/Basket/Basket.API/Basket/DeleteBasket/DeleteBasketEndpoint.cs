
using Basket.API.Services;

namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndpoint(IIdentityService identityService) : ICarterModule
    {
        private readonly IIdentityService _identityService = identityService; 
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket, {userName}", async (ISender sender, string userName) =>
            {
                var customer = _identityService.GetUserIdentity();
                var result = await sender.Send(new DeleteBasketCommand(customer.CustomerId));

                var response = result.Adapt<DeleteBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Basket")
            .WithDescription("Delete Basket");
        }
    }
}
