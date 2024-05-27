using Basket.API.Basket.DeleteBasket;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.EventHandlers
{
    public class CreatedOrderEventHandler(ISender sender, ILogger<CreatedOrderEventHandler> logger) : IConsumer<CheckedOutEvent>
    {
        private readonly ILogger<CreatedOrderEventHandler> _logger = logger;
        private readonly ISender _sender = sender;
        public async Task Consume(ConsumeContext<CheckedOutEvent> context)
        {
            var message = context.Message;
            if(message.Success)
            {
                var command = new DeleteBasketCommand(message.CustomerId);
                var result  = await _sender.Send(command);
              
                _logger.LogDebug("Create order is successfully!");
              
            }
        }
    }
}
