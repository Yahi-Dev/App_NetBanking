using NetBanking.Core.Application.ViewModels.Dashboard;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<string> ChangeAccountStatus(string Id);
        Task<List<UserViewModel>> GetAllAsync();
        Task<DashboardViewModel> GetDashboard();
        //Task<SaveUserViewModel> GetByIdWithAmountAsync(string UserId);
        //Task<string> ChangeAccountStatus(UserViewModel vm);
    }
}
