using System.Threading.Tasks;

namespace AspNetCoreMultiplatform.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
