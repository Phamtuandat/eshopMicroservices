using MediatR;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandler(IApplicationDbContext context) : IRequestHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            var customerId = request.CustomerId;
            var orders = await context.Orders
            .Include(o => o.OrderItems)
                         .AsNoTracking()
                         .Where(o => o.CustomerId == request.CustomerId)
                         .OrderBy(o => o.OrderName.Value)
                         .ToListAsync(cancellationToken);
            return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
