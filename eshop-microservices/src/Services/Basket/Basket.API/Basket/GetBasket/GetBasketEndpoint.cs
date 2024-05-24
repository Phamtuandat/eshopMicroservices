
using Basket.API.Services;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoint(IIdentityService identityService, ILogger<GetBasketEndpoint> logger) : CarterModule
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly ILogger<GetBasketEndpoint> _logger = logger;
        public override void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/basket/{userName}", async (string username, ISender sender) =>
            {
                var userId = _identityService.GetUserIdentity();
                
                var query = new GetBasketQuery(userId.CustomerId);
                var result = await sender.Send(query);
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("GetBasketByUsername")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get basket By username")
            .WithDescription("Get basket By username").RequireAuthorization("default");
        }
    }
}
