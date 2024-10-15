using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Service.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendRegistrationEmail(string fullName, string userEmail, string otp);
        Task ResendVerificationEmail(string fullName, string userEmail, string otp);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task ResendVerificationEmail(string fullName, string userEmail, string otpCode)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse($"{userEmail}"));
            email.Subject = "[Koi Farm Shop] - Resend OTP Verification Code";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
             <!DOCTYPE html>
             <html lang='en'>
             <head>
                 <meta charset='UTF-8'>
                 <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                 <title>Resend OTP Verification Code</title>
             </head>
             <body style='font-family: Arial, sans-serif; background-color: #f0f0f0; color: #333333;'>
                 <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4CAF50;'>
                     <h1 style='color: #ffffff;'>Resend OTP Verification Code</h1>
                     <p style='color: #ffffff;'>Hi {fullName},</p>
                     <p style='color: #ffffff;'>It seems like you missed the first verification of your account with Koi Farm Shop. To complete your registration, please use the following One-Time Password (OTP):</p>
                     <h2 style='color: #ffffff; text-align: center;'>{otpCode}</h2>
                     <p style='color: #ffffff;'>Enter this code in the verification section of the website to activate your account.</p>
                     <p style='color: #ffffff;'>If you did not request this, please ignore this email.</p>
                     <p style='color: #ffffff;'>Thank you,</p>
                     <p style='color: #ffffff;'>The Koi Farm Shop Team</p>
                 </div>
             </body>
             </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendRegistrationEmail(string fullName, string userEmail, string otpCode)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse($"{userEmail}"));
            email.Subject = "[Koi Farm Shop] - Welcome to Koi Farm Shop!";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
             <!DOCTYPE html>
             <html lang='en'>
             <head>
                 <meta charset='UTF-8'>
                 <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                 <title>Welcome to Koi Farm Shop</title>
             </head>
             <body style='font-family: Arial, sans-serif; background-color: #f0f0f0; color: #333333;'>
                 <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4CAF50;'>
                     <h1 style='color: #ffffff;'>Welcome to Koi Farm Shop!</h1>
                     <p style='color: #ffffff;'>Hi {fullName},</p>
                     <p style='color: #ffffff;'>Thank you for registering with Koi Farm Shop. We are thrilled to have you as part of our community! Explore our platform for the best Koi fish, supplies, and expert advice on caring for your Koi.</p>
                     <p style='color: #ffffff;'>To complete your registration and verify your account, please use the following One-Time Password (OTP):</p>
                     <h2 style='color: #ffffff; text-align: center;'>{otpCode}</h2>
                     <p style='color: #ffffff;'>Please enter this code in the verification section of the website to activate your account.</p>
                     <p style='color: #ffffff;'>If you did not sign up for an account, please ignore this email.</p>
                     <p style='color: #ffffff;'>Thank you,</p>
                     <p style='color: #ffffff;'>The Koi Farm Shop Team</p>
                 </div>
             </body>
             </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
