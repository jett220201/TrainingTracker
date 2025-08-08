namespace TrainingTracker.Application.Interfaces.Helpers
{
    public interface IEmailHelper
    {
        Task SendEmailAsync(string userName, string toEmail, string subject, string body);
    }
}
