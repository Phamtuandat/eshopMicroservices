
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Enpoints
{
    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async ([FromBody] CreateOrderRequest request, ISender sender) =>
            {
                var createOrderCommand = request.Adapt<CreateOrderCommand>();
                var result = await sender.Send(createOrderCommand);

                var response = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/orders/{response.Id}", response);
            });
        }
    }
}
