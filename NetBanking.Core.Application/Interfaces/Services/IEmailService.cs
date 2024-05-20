using NetBanking.Core.Application.Dtos.Email;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
