using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Exceptions;
using Discount.GRPC.Models;

namespace Discount.GRPC.Discounts.UpdateDiscount
{   public record UpdateDiscountCommand(Coupon Coupon) : ICommand<UpdateDiscountResult>;
    public record UpdateDiscountResult(bool IssSucces);
    public class UpdateDiscountCommandHandler(DiscountContext context) : ICommandHandler<UpdateDiscountCommand, UpdateDiscountResult>
    {
        private readonly DiscountContext _context = context;
        public async Task<UpdateDiscountResult> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _context.Coupons.FirstOrDefault(c => c.Id == request.Coupon.Id);
            if (coupon != null)
            {
                coupon.Quantity = request.Coupon.Quantity;
                coupon.DiscountPercentage = request.Coupon.DiscountPercentage;
                coupon.Description = request.Coupon.Description;
                coupon.Code = request.Coupon.Code;
                _context.Update(coupon);
                await _context.SaveChangesAsync(cancellationToken);
                return new UpdateDiscountResult(true);
            }

            throw new DiscountNotFoundException(nameof(UpdateDiscountCommandHandler));

        }
    }
}
