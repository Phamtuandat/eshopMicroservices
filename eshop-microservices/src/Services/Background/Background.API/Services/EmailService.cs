using Background.API.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Encodings.Web;

namespace Background.API.Services
{
    public class EmailService(IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment env) : IEmailService
    {
        private readonly IWebHostEnvironment _env = env;

      
        private readonly SmtpSettings _smtpSettings = smtpSettings.Value;

        public async Task<bool> SendOTPEmailAsync(string code, string email)
        {
            var message = await File.ReadAllTextAsync(Path.Combine(_env.WebRootPath, "EmailTemplates/SentOTPEmailTemplate.html"));

            message = message.Replace("[email]", email).Replace("[code]", code);
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(_smtpSettings.SenderEmail,email));
            emailMessage.Subject = "Change password request";

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
            return true;
        }

        public async Task<bool> SendOrderCheckoutEmailAsync(string toEmail, string subject)
        {

            return true;
        }

        public async Task<bool> SendUserAccountConfirmEmailAsync(string toEmail, string redirectUrl, string username)
        {
            var message = await File.ReadAllTextAsync(Path.Combine(_env.WebRootPath, "EmailTemplates/UserRegisterConfirm.html"));
            
            message = message.Replace("[email]", toEmail)
                        .Replace("[username]", username)
                        .Replace("[confirmLink]", HtmlEncoder.Default.Encode(redirectUrl));
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(username, toEmail));
            emailMessage.Subject = "Welcome to Dev Shop";

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port,SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception
                throw new InvalidOperationException(ex.Message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
            return true;
        }


    }
}
