using Background.API.Services;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Background.API.EventHandlers.cs
{
    public class CreatedUserEvent(IEmailService emailService) : IConsumer<RegiestedUserEvent>
    {
        private readonly IEmailService _emailService = emailService;
        public async Task Consume(ConsumeContext<RegiestedUserEvent> context)
        {
            var userEmail = context.Message.Email;
            var username = context.Message.UserName;
            var redirectUrl = context.Message.ConfirmUrl;
            await _emailService.SendUserAccountConfirmEmailAsync(userEmail, redirectUrl,  username);
        }
    }
}
