using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.Transaction
{
    public class TransactionStatusViewModel
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
    }
}
