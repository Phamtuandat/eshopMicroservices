using MassTransit;
using MassTransit.Testing;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderUpdatedEventHandler(IPublishEndpoint endpoint, IFeatureManager manager, ILogger<OrderUpdatedEventHandler> logger) : INotificationHandler<OrderUpdatedEvent>
    {
        public async Task Handle(OrderUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
            if (await manager.IsEnabledAsync("OrderFullfilment"))
            {
                var updatedOrderIntergrationEvent = domainEvent.Order.ToOrderDto();
                await endpoint.Publish(updatedOrderIntergrationEvent, cancellationToken);
            }
        }
    }
}
