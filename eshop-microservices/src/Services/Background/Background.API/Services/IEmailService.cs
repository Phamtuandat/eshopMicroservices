using MimeKit;

namespace Background.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendOrderCheckoutEmail(string toEmail, string subject);

        Task<bool> SendUserAccountConfirmEmailAsync(string userEmail, string redirectUrl, string username);

        Task<bool> SendOTPEmailAsync( string code, string email);
    }
}
