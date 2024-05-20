using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Loan> _entities;
        public LoanRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Loan>();
        }
    }
}
