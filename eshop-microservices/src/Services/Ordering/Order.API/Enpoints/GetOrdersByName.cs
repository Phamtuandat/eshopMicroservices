
using Ordering.Application.Orders.Queries.GetOrdersByName;
using static Ordering.API.Enpoints.GetOrders;

namespace Ordering.API.Enpoints
{
    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{name}", async (string name, ISender sender) => {

                var result = await sender.Send(new GetOrdersByNameQuery(name));
                return Results.Ok(result.Orders);
            })
            .WithName("GetOrdersByName")
            .Produces<IEnumerable<OrderDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders by Name")
            .WithDescription("Get Orders by Name"); 
        }
    }
}
