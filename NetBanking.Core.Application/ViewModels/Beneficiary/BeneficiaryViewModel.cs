using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DtoAccounts UserAccount { get; set; }
        public string BeneficiaryId { get; set; }
        public DtoAccounts BeneficiaryUser { get; set; }
        public string BeneficiaryAccountId { get; set; }
    }
}
