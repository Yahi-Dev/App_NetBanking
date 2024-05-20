using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.ViewModels.Dashboard
{
    public class ListUserRegisterAndStateViewModel
    {
        public string? Id { get; set; }
        List<UserViewModel> userRegister {  get; set; }
    }
}
