using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using DAL.Core.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGridAttachment = SendGrid.Helpers.Mail.Attachment;

namespace BLL.Helpers
{
    public interface ISendGridSender
    {
        Task<SendGridResponse> SendEmailAsync(EmailAddress sender, EmailAddress[] recepients, string subject, string body, bool isHtml = true);
        Task<SendGridResponse> SendEmailAsync(string recepientName, string recepientEmail, string subject, string body, bool isHtml = true);
        Task<SendGridResponse> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string subject, string body, bool isHtml = true);

    }
    public class SendGridSender : ISendGridSender
    {
        private readonly LocalSettings _settings;
        public SendGridSender(IOptions<LocalSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<SendGridResponse> SendEmailAsync(string recepientName, string recepientEmail, string subject, string body, bool isHtml = true)
        {
            var from = new EmailAddress(_settings.SendGridConfig.EmailAddress, _settings.SendGridConfig.Name);
            var to = new EmailAddress(recepientEmail, recepientName);
            return await SendEmailAsync(from, new EmailAddress[] { to }, subject, body, isHtml);
        }

        public async Task<SendGridResponse> SendEmailAsync(string senderName, string senderEmail, string recepientName, string recepientEmail, string subject, string body, bool isHtml = true)
        {
            var from = new EmailAddress(senderName, senderEmail);
            var to = new EmailAddress(recepientEmail, recepientName);
            return await SendEmailAsync(from, new EmailAddress[] { to }, subject, body, isHtml);
        }

        public async Task<SendGridResponse> SendEmailAsync(EmailAddress sender, EmailAddress[] recepients, string subject, string body, bool isHtml = true)
        {
            var sendGridClient = new SendGridClient(_settings.SendGridConfig.APIKey);

            var mailMessage = new SendGridMessage();
            mailMessage.SetFrom(sender);
            mailMessage.AddTos(recepients.ToList());
            mailMessage.SetSubject(subject);

            if (isHtml)
            {
                mailMessage.HtmlContent = body;
            }
            else
            {
                mailMessage.PlainTextContent = body;
            }

            var sendGridResponse = await sendGridClient.SendEmailAsync(mailMessage);

            var sendResponse = new SendGridResponse();

            if (IsHttpSuccess((int)sendGridResponse.StatusCode)) return sendResponse;

            sendResponse.ErrorMessages.Add($"{sendGridResponse.StatusCode}");
            var messageBodyDictionary = await sendGridResponse.DeserializeResponseBodyAsync(sendGridResponse.Body);

            if (messageBodyDictionary.ContainsKey("errors"))
            {
                var errors = messageBodyDictionary["errors"];

                foreach (var error in errors)
                {
                    sendResponse.ErrorMessages.Add($"{error}");
                }
            }

            return sendResponse;
        }

        private bool IsHttpSuccess(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }
    }
}
