

namespace BuildingBlocks.Messaging.Events
{
    public record RegiestedUserEvent : IntergrationEvent
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string ConfirmUrl { get; set; } = default!;

    }
}
