
namespace BuildingBlocks.Messaging.Events
{
    public record SentOTPEvent : IntergrationEvent
    {
        public string Code { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
