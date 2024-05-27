using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public record CheckedOutEvent : IntergrationEvent
    {
        public Guid Id { get; set; } = default!;
        public string CustomerId { get; set; } = default!;
        public bool Success { get; set; }
        public string? CouponCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
