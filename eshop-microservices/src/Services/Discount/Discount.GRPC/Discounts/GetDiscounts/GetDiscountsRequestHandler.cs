using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.GetDiscounts
{
    public record GetDiscountsQuery(int Page = 0, int PageSize = 10) : IQuery<GetDiscountsResult>;
    public record GetDiscountsResult(IEnumerable<Coupon> Coupons);
    internal class GetDiscountsRequestHandler(DiscountContext context, ILogger<GetDiscountsRequestHandler> logger) : IRequestHandler<GetDiscountsQuery, GetDiscountsResult>
    {
        private readonly DiscountContext _context = context;
        private readonly ILogger<GetDiscountsRequestHandler> _logger = logger;
        public async Task<GetDiscountsResult> Handle(GetDiscountsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Coupons.Skip(request.PageSize*request.Page).Take(request.PageSize).ToListAsync(cancellationToken);

            return new GetDiscountsResult(result);
            
        }
    }
}
