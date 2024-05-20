using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services.Domain_Services
{
    public interface ISavingsAccountService : IGenericService<SaveSavingsAccountViewModel, SavingsAccountViewModel, SavingsAccount>
    {
        Task SaveUserWIthAccount(SaveUserViewModel vm);
        Task SaveUserWIthMainAccount(SaveUserViewModel vm);
        public Task<DeleteStatus> Delete(string Id);
        public Task<List<SavingsAccountViewModel>> GetByOwnerIdAsync(string Id);
    }
}
