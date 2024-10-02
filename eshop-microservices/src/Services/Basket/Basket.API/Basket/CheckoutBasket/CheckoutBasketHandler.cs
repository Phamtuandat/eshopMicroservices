
using BuildingBlocks.Messaging.Events;
using FluentValidation;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto)
    : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler
    (IBasketRepository repository, IPublishEndpoint publishEndpoint, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {

        var basket = await repository.GetBasket(command.BasketCheckoutDto.CustomerId, cancellationToken);
        if (basket == null) return new CheckoutBasketResult(false);
        

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.BasketCheckOutItems = basket.Items.Adapt<List<BasketCheckOutItem>>();
        if (command.BasketCheckoutDto.CouponCode != null)
            await _DeductDiscount(eventMessage, cancellationToken);
        eventMessage.BasketCheckOutItems = basket.Items.Adapt<List<BasketCheckOutItem>>();
        await publishEndpoint.Publish(eventMessage, cancellationToken);



        return new CheckoutBasketResult(true);
    }

    private async Task _DeductDiscount(BasketCheckoutEvent cart, CancellationToken cancellationToken)
    {
        var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { Code = cart.CouponCode }, cancellationToken: cancellationToken);
        if(coupon.Code == "No Discount") throw new Exception("Coupon is invalid!");
        if(coupon.Code == "Coupon is out of stock") throw new Exception($"{coupon.Code} is out of stock");
        decimal discountPercentage = (decimal)coupon.DiscountPercentage / 100;
        foreach (var item in cart.BasketCheckOutItems)
        {
            decimal discountAmount = discountPercentage * item.Price;
            item.Price -= discountAmount;   
        }
    }
}

