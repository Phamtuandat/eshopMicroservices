namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDbContext context) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var pageIdx = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;

            var totalCount = await context.Orders.LongCountAsync(cancellationToken);

            var orders = await context.Orders
                           .Include(o => o.OrderItems)
                           .OrderBy(o => o.OrderName.Value)
                           .Skip(pageSize * pageIdx)
                           .Take(pageSize)
                           .ToListAsync(cancellationToken);

            return new GetOrdersResult(
                new PaginationResult<OrderDto>(
                    pageIdx,
                    pageSize,
                    totalCount,
                    orders.ToOrderDtoList()));
        }
    }
}
