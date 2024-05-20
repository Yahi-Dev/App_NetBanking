using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Application.ViewModels.SavingsAccount;

namespace NetBanking.Core.Application.ViewModels.Client
{
    public class GetAllProductsByClientViewModel
    {
        public List<CreditCardViewModel> CreditCards { get; set;}
        public List<LoanViewModel> Loans { get; set;}
        public List<SavingsAccountViewModel> SavingsAccounts { get; set;}
    }
}
