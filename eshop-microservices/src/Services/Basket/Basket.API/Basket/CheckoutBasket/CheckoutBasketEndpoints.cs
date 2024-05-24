using Basket.API.Services;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
public record CheckoutBasketResponse(bool IsSuccess);

public class CheckoutBasketEndpoints(IIdentityService identityService) : ICarterModule
{
    private readonly IIdentityService _identityService = identityService; 
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
        {
            var customer = _identityService.GetUserIdentity();
            request.BasketCheckoutDto.UserName = customer.UserName;
            request.BasketCheckoutDto.CustomerId = customer.CustomerId;
            var command = request.Adapt<CheckoutBasketCommand>();
            var result = await sender.Send(command);

            var response = result.Adapt<CheckoutBasketResponse>();
            
            return Results.Ok(response);
        })
        .WithName("CheckoutBasket")
        .Produces<CheckoutBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Checkout Basket")
        .WithDescription("Checkout Basket");
    }
}