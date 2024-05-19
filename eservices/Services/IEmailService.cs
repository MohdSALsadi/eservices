
using System.Threading.Tasks;

namespace Pattern_of_life
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
}
