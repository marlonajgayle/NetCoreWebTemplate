using MailKit.Net.Smtp;
using MimeKit;
using NetCoreWebTemplate.Application.Common.Interfaces;
using NetCoreWebTemplate.Application.Notifications.Models;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreWebTemplate.Infrastructure.Notifications.Email
{
    public class MailService : IMailService
    {
        private readonly EmailConfigurations emailConfigurations;

        public MailService(EmailConfigurations emailConfigurations)
        {
            this.emailConfigurations = emailConfigurations;
        }

        /// <summary>
        ///  Create and Send Email message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMailAsync(MessageDto message)
        {
            var emailMessage = CreateMailMessage(message);

            await SendEmailAsync(emailMessage);
        }

        /// <summary>
        /// This method is resposible for building a MimeMessage
        /// object to sent.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>emailMessage</returns>
        public MimeMessage CreateMailMessage(MessageDto message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailConfigurations.FromAddress));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Body };

            return emailMessage;
        }

        private static StringBuilder GetTemplate(string templatePath)
        {
            var builder = new StringBuilder();
            var newPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), templatePath);

            using (var reader = File.OpenText(newPath))
            {
                builder.Append(reader.ReadToEnd());
            }

            return builder;
        }

        /// <summary>
        /// This method is responsible for sending email message
        /// using smtp client.
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        private async Task SendEmailAsync(MimeMessage emailMessage)
        {
            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(emailConfigurations.SmtpServer, emailConfigurations.Port, emailConfigurations.IsRequireSsl);
                smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtpClient.AuthenticateAsync(emailConfigurations.Username, emailConfigurations.Password);
                await smtpClient.SendAsync(emailMessage);
            }
            catch
            {
                // log error
                // throw custom exception
            }
            finally
            {
                await smtpClient.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
