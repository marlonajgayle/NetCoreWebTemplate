using MimeKit;
using NetCoreWebTemplate.Application.Notifications.Models;
using NetCoreWebTemplate.Infrastructure.Notifications.Email;
using Xunit;

namespace NetCoreWebTemplate.UnitTests.Infrastructure.Notifications
{
    public class MailServiceTests
    {
        private readonly MessageDto message;
        private readonly EmailConfigurations configuration;

        public MailServiceTests()
        {
            configuration = new EmailConfigurations()
            {
                FromAddress = "noreply@test.com",
                SmtpServer = "smtp.gmail.com",
                Port = 465,
                Username = "test_user",
                Password = "test_password",
                IsRequireSsl = false
            };

            message = new MessageDto()
            {
                From = "noreply@test.com",
                To = "jdoe@gmail.com",
                Subject = "Test Email Notifaction",
                Body = "Email Contents"
            };
        }

        [Fact]
        public void EmailConfigurationTest()
        {

            Assert.NotNull(configuration);
            Assert.Equal("noreply@test.com", configuration.FromAddress);
            Assert.Equal("smtp.gmail.com", configuration.SmtpServer);
            Assert.Equal(465, configuration.Port);
            Assert.Equal("test_user", configuration.Username);
            Assert.Equal("test_password", configuration.Password);
            Assert.False(configuration.IsRequireSsl);
        }

        [Fact]
        public void CreateMailMessageTest()
        {
            // Arrange
            var mailService = new MailService(configuration);

            // Act
            var emailMessage = mailService.CreateMailMessage(message);

            // Assert
            Assert.NotNull(message);
            Assert.Contains(new MailboxAddress("noreply@test.com"), emailMessage.From);
            Assert.Contains(new MailboxAddress("jdoe@gmail.com"), emailMessage.To);
            Assert.Equal("Test Email Notifaction", emailMessage.Subject);
            Assert.Equal("Email Contents", emailMessage.HtmlBody);
        }
    }
}
