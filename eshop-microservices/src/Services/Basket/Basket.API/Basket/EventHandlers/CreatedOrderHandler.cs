using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.EventHandlers
{
    public class CreatedOrderHandler : IConsumer<CheckedOutEvent>
    {
        public Task Consume(ConsumeContext<CheckedOutEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
