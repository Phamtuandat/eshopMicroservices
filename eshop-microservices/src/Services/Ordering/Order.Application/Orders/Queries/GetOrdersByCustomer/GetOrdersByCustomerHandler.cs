using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public class GetOrdersByCustomerHandler(IApplicationDbContext context) : IRequestHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            var customerId = CustomerId.Of(request.CustomerId);
            var orders = await context.Orders
            .Include(o => o.OrderItems)
                         .AsNoTracking()
                         .Where(o => o.CustomerId == CustomerId.Of(request.CustomerId))
                         .OrderBy(o => o.OrderName.Value)
                         .ToListAsync(cancellationToken);
            return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
