﻿using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Transaction;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface ITransactionService : IGenericService<SaveTransactionViewModel , TransactionViewModel , Transaction>
    {
        public Task<List<TransactionViewModel>> GetByOwnerIdAsync(string Id);
    }
}
