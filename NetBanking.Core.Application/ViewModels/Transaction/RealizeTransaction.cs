using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class RealizeTransaction
    {
        public SaveTransactionViewModel SaveTransactionViewModel { get; set; }
        public GetAllProductsByClientViewModel AllProducts { get; set; }
        public List<BeneficiaryViewModel> Beneficiaries { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatusViewModel TransactionStatus { get; set; } = new TransactionStatusViewModel();

    }
}
