using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Domain.Entities;
using NetBanking.Infrastructure.Persistence.Contexts;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Beneficiary> _entities;
        public BeneficiaryRepository(ApplicationContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Beneficiary>();
        }
    }
}
