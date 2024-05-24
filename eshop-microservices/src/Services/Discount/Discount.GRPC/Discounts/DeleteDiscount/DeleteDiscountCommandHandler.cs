using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.DeleteDiscount
{
    public record DeleteDiscountCommand(Guid Id) : ICommand<DeleteDiscountResult>;
    public record DeleteDiscountResult(bool IsSuccess);

    public class DeleteDiscountCommandHandler(DiscountContext context) : ICommandHandler<DeleteDiscountCommand, DeleteDiscountResult>
    {
        private readonly DiscountContext _context = context;
        public async Task<DeleteDiscountResult> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _context.Coupons.FirstOrDefault(x => x.Id == request.Id);
            if (coupon == null) throw new DiscountNotFoundException(request.Id.ToString());
            _context.Remove(coupon);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteDiscountResult(true);
        }
    }
}
