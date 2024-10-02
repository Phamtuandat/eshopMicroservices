using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record CheckedOutEvent : IntergrationEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public bool Success { get; set; }
        public string? CouponCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
