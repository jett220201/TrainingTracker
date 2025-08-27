using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using SendGrid.Helpers.Mail;
using TrainingTracker.Application.Interfaces.Helpers;
using TrainingTracker.Localization.Resources.Shared;

namespace TrainingTracker.Infrastructure.Helpers
{
    public class SendGridEmailHelper : IEmailHelper
    {
        private readonly string _apiKey;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SendGridEmailHelper(IConfiguration configuration, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _configuration = configuration;
            _localizer = stringLocalizer;
            _apiKey = configuration["SendGrid:ApiKey"]
                        ?? throw new ArgumentNullException("Email:ApiKey", "API key for email service is not configured.");
        }

        public async Task SendEmailAsync(string userName, string toEmail, string subject, string body)
        {
            var client = new SendGrid.SendGridClient(_apiKey);
            var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, GetHTMLBody(userName, subject, body));
            await client.SendEmailAsync(msg);
        }

        private string GetHTMLBody(string userName, string subject, string body)
        {
            return $@"
                <body style=""font-family: 'Poppins', Arial, sans-serif"">
                    <table width=""100%"" style=""display: grid; place-items:center"" cellspacing=""0"" cellpadding=""0"">
                        <tr>
                            <td style=""padding: 20px;"">
                                <table class=""content"" width=""600"" cellspacing=""0"" cellpadding=""0"" style=""border-collapse: collapse; border: 1px solid #cccccc;"">
                                    <!-- Header -->
                                    <tr>
                                        <td class=""header"" style=""background-color: #00a6f4; padding: 40px; text-align: center; color: white; font-size: 24px;"">
                                            {subject}
                                        </td>
                                    </tr>
                                    <!-- Body -->
                                    <tr>
                                        <td class=""body"" style=""padding: 40px; text-align: left; font-size: 16px; line-height: 1.6;"">
                                            {_localizer["Greetings"]} {userName} 
                                            <br>
                                            {body}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class=""body"" style=""padding: 40px; text-align: left; font-size: 16px; line-height: 1.6;"">
                                            {_localizer["Slogan"]} 💪🧠             
                                        </td>
                                    </tr>
                                    <!-- Footer -->
                                    <tr>
                                        <td class=""footer"" style=""background-color: #00a6f4; padding: 40px; text-align: center; color: white; font-size: 14px;"">
                                            Copyright &copy; {DateTime.Now.Year.ToString()} | {_configuration["SendGrid:FromName"]}
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>";
        }
    }
}
