using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    public record GetOrdersByCustomerQuery(string CustomerId) : IQuery<GetOrdersByCustomerResult>;    

    public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);
}
