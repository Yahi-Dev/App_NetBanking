using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Domain.Common;
using NetBanking.Core.Application.Helpers;

namespace NetBanking.Infrastructure.Persistence.Interceptor
{
    public class UpdateAuditableEntitiesInterceptor
    : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private AuthenticationResponse? userViewModel;
        public UpdateAuditableEntitiesInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            DbContext? dbContext = eventData.Context;
            if (dbContext == null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }
            var entries = dbContext.ChangeTracker
                .Entries<IAuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                { 
                    entry.Property(a => a.CreatedDate).CurrentValue = DateTime.UtcNow;
                    if (userViewModel == null)
                        entry.Property(a => a.CreatedById).CurrentValue = "Default";
                    else
                        entry.Property(a => a.CreatedById).CurrentValue = userViewModel.Id;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(a => a.ModifiedDate).CurrentValue = DateTime.UtcNow;
                    entry.Property(a => a.ModifiedById).CurrentValue = userViewModel.Id;
                }
                if (entry.State == EntityState.Deleted)
                {
                    entry.Property(a => a.DeletedDate).CurrentValue = DateTime.UtcNow;
                    entry.Property(a => a.DeletedById).CurrentValue = userViewModel.Id;
                }
            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
