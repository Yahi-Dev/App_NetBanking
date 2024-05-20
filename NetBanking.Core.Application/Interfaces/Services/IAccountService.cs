using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ServiceResult> ChangeUserStatus(RegisterRequest request);
        Task<DtoAccounts> GetByEmail(string Email);
        Task Remove(DtoAccounts account);
        Task<List<DtoAccounts>> GetAllUsers();
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<ServiceResult> RegisterUserAsync(RegisterRequest request, string origin, string UserRoles);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ServiceResult> ForgotPassswordAsync(ForgotPasswordRequest request, string origin);
        Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task SingOutAsync();
        Task<DtoAccounts> GetByIdAsync(string UserId);
        Task<ServiceResult> UpdateUserAsync(RegisterRequest request);
    }
}
