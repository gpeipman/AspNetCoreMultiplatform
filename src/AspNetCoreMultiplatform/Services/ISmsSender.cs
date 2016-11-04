using System.Threading.Tasks;

namespace AspNetCoreMultiplatform.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
