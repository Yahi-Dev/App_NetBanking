using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Transaction> _entities;
        public TransactionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Transaction>();
        }

        public override async Task DeleteAsync(Transaction entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

    }
}
