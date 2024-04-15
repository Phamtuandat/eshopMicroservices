using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{

    public class GetOrdersByNameHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery request, CancellationToken cancellationToken)
        {
            var orders = await context.Orders.Include(x => x.OrderItems).AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(request.Name))
                .OrderBy(o => o.OrderName.Value).ToListAsync();

            return new GetOrdersByNameResult(orders.ToOrderDtoList());
        }
    }
}
