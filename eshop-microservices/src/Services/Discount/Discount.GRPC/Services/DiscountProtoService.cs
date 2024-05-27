using Discount.GRPC.Data;
using Discount.GRPC.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountProtoService
    (DiscountContext dbContext, ILogger<DiscountProtoService> logger)
    : Grpc.DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.Code == request.Code );

        if (coupon is null)
            coupon = new Coupon { Code = "No Discount", DiscountPercentage = 0, Description = "No Discount Desc" };

        if (coupon.Quantity == 0)
            coupon = new Coupon { Code = "Coupon is out of stock", DiscountPercentage = 0, Description = "Coupon is out of stock" };

        logger.LogInformation("Discount is retrieved for Code  : {productName}, Amount : {amount}", coupon.Code, coupon.DiscountPercentage);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

}