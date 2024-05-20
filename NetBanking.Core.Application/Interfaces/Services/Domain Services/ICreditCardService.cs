using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services.Domain_Services
{
    public interface ICreditCardService : IGenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>
    {
        public Task<List<CreditCardViewModel>> GetByOwnerIdAsync(string Id);
        public Task<DeleteStatus> Delete(string Id);
    }
}
