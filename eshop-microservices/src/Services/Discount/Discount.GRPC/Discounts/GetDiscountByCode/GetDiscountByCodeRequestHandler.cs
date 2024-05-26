using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Exceptions;
using Discount.GRPC.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.GetdDiscountByCode
{
    public record GetDiscountByCodeRequest(string Code) : IQuery<GetDiscountByCodeResult>;
    public record GetDiscountByCodeResult(Coupon Coupon);
    public class GetDiscountByCodeRequestHandler(DiscountContext context) : IRequestHandler<GetDiscountByCodeRequest, GetDiscountByCodeResult>
    {
        private readonly DiscountContext _context = context;
        public async Task<GetDiscountByCodeResult> Handle(GetDiscountByCodeRequest request, CancellationToken cancellationToken)
        {
            var result = await _context.Coupons.Where(x => x.Code == request.Code).FirstOrDefaultAsync();

            if (result == null) throw new DiscountNotFoundException(nameof(GetDiscountByCodeRequestHandler));

            return new GetDiscountByCodeResult(result);
        }
    }
}
