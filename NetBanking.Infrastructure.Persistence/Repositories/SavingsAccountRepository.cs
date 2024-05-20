using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class SavingsAccountRepository : GenericRepository<SavingsAccount>, ISavingsAccountRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<SavingsAccount> _entities;
        public SavingsAccountRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<SavingsAccount>();
        }
    }
}
