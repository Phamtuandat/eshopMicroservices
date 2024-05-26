
using BuildingBlocks.CQRS;
using Discount.GRPC.Data;
using Discount.GRPC.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.CreateDiscount
{
    public record CreateDiscountCommand(string Code, string Description, int Quantity, string Amount) : ICommand<CreateDiscontResult>;
    public record CreateDiscontResult(bool IsSuccess);
    public class CreateDiscountCommandHandler(DiscountContext context) : ICommandHandler<CreateDiscountCommand, CreateDiscontResult>
    {
        private readonly DiscountContext _context = context;
        public async Task<CreateDiscontResult> Handle([FromBody] CreateDiscountCommand command, CancellationToken cancellationToken)
        {
            var coupon = command.Adapt<Coupon>();
            coupon.Id = Guid.NewGuid();
            await _context.AddAsync(coupon, cancellationToken);
             await _context.SaveChangesAsync(cancellationToken);

            return new CreateDiscontResult(true);
        }
    }
}
