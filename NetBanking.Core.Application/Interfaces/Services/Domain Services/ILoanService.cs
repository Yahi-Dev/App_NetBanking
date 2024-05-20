using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.Services.Domain_Services
{
    public interface ILoanService : IGenericService<SaveLoanViewModel, LoanViewModel, Loan>
    {
        public Task<DeleteStatus> Delete(string Id);
        public Task<List<LoanViewModel>> GetByOwnerIdAsync(string Id);
    }
}
