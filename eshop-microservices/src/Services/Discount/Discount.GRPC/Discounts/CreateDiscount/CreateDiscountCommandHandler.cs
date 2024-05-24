
using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.CreateDiscount
{
    public record CreateDiscountCommand(string Code, string Description, int Quantity, string Amount) : ICommand<CreateDiscontResult>;
    public record CreateDiscontResult(Coupon Coupon);
    public class CreateDiscountCommandHandler(DiscountContext context) : ICommandHandler<CreateDiscountCommand, CreateDiscontResult>
    {
        private readonly DiscountContext _context = context;
        public async Task<CreateDiscontResult> Handle(CreateDiscountCommand command, CancellationToken cancellationToken)
        {
            var coupon = command.Adapt<Coupon>();
            coupon.Id = Guid.NewGuid();
            await _context.AddAsync(coupon);
            await _context.SaveChangesAsync();

            return new CreateDiscontResult(coupon);
        }
    }
}
