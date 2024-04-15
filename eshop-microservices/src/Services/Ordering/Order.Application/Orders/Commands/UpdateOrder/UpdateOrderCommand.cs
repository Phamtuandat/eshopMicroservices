using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderResult>;

    public record UpdateOrderResult(bool IsSuccess);

    public class UpdatOrderValidator : AbstractValidator<UpdateOrderCommand> 
    {
        public UpdatOrderValidator()
        {
            
            RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Order.CustomerId).NotNull().WithMessage("CustomerId is required");

        }
    }
    
}
