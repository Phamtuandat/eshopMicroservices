using BuildingBlocks.Messaging.Events;
using Discount.GRPC.Data;
using Discount.GRPC.Discounts.UpdateDiscount;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Discounts.Events
{
    public class CheckoutBasketHandler(DiscountContext context, ISender sender) : IConsumer<CheckedOutEvent>
    {
        private readonly DiscountContext _context = context;

        public async Task Consume(ConsumeContext<CheckedOutEvent> context)
        {
            var coupon = context.Message.CouponCode;
            if (string.IsNullOrEmpty(coupon)) return;
            var discount = await _context.Coupons.FirstOrDefaultAsync(x => x.Code == coupon);
            if (discount == null) throw new Exception("Discount is invalid!");
            discount.Quantity -= 1;
            var command = new UpdateDiscountCommand(discount);
            await sender.Send(command);
            
        }
    }
}
