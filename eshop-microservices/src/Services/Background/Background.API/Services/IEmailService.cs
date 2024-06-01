﻿using MimeKit;

namespace Background.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendOrderCheckoutEmailAsync(string toEmail, string subject);

        Task<bool> SendUserAccountConfirmEmailAsync(string userEmail, string redirectUrl, string username);
    }
}