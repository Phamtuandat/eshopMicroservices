

namespace BuildingBlocks.Messaging.Events
{
    public record RegiestedUserEvent : IntergrationEvent
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ConfirmUrl { get; set; }
        
    }
}
