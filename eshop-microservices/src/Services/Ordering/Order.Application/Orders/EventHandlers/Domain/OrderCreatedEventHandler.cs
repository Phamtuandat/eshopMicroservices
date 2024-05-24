using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Ordering.Domain.Abstractions;
using Ordering.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderCreatedEventHandler(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger) : INotificationHandler<OrderCreatedEvent>
    {
        public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
            var isEnable = await featureManager.IsEnabledAsync("OrderFullfilment");
            if (await featureManager.IsEnabledAsync("OrderFullfilment"))
            {
                var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
                var checkedOutEvent = new CheckedOutEvent() { Success = true, 
                    CustomerId = orderCreatedIntegrationEvent.CustomerId,
                    Id = orderCreatedIntegrationEvent.Id,
                    ErrorMessage = null,
                    
                };
                await publishEndpoint.Publish(checkedOutEvent, cancellationToken);
            }
        }
    }
}
