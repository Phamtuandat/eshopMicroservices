
namespace BuildingBlocks.Messaging.Events
{
    public record SentOTPEvent : IntergrationEvent
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
