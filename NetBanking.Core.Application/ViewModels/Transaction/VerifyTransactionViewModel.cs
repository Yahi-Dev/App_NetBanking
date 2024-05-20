using NetBanking.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class VerifyTransactionViewModel
    {
        public DtoAccounts Destinatary { get; set; }
        public SaveTransactionViewModel Transaction { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
