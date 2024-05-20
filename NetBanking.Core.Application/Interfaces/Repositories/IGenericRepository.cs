using System.Linq.Expressions;

namespace NetBanking.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task UpdateAsync(Entity entity, string Id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GeEntityByIDAsync(string Id);
        Task<List<Entity>> FindAllAsync(Expression<Func<Entity, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<Entity, bool>> filter);
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
    }
}
