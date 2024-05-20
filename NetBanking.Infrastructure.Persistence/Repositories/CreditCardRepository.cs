using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class CreditCardRepository : GenericRepository<CreditCard>, ICreditCardRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<CreditCard> _entities;
        public CreditCardRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<CreditCard>();
        }
    }
}
