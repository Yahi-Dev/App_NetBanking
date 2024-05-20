using System.Linq.Expressions;

namespace NetBanking.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        public Task<SaveViewModel> AddAsync(SaveViewModel vm);
        public Task<List<ViewModel>> GetAllAsync();
        public Task<ViewModel> GetByIdAsync(string Id);
        public Task<SaveViewModel> GetByIdSaveViewModelAsync(string Id);
        public Task UpdateAsync(SaveViewModel vm, string Id);
        public Task Delete(string Id);
        Task<List<ViewModel>> FindAllAsync(Expression<Func<Entity, bool>> filter);
        //public Task Delete(int Id);
    }
}
