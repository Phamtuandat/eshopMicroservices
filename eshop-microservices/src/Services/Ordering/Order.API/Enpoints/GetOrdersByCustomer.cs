
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Enpoints
{
    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
                var response = result.Orders;

                return Results.Ok(response);
            })
            .WithDescription("Get Orders by customer")
            .WithName("GetOrdersByCustomer")
            .WithSummary("Get Orders by customer")
            .Produces<IEnumerable<OrderDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
