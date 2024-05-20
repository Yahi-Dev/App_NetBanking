using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services.Domain_Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        public Task<List<BeneficiaryViewModel>> GetByOwnerIdAsync(string Id);
    }
}
