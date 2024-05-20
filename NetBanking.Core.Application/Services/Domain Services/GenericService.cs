using AutoMapper;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using System.Linq.Expressions;

namespace NetBanking.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity> : IGenericService<SaveViewModel, ViewModel, Entity>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity> _repository;
        private readonly IMapper _mapper;
        public GenericService(IGenericRepository<Entity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<SaveViewModel> AddAsync(SaveViewModel vm)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            //Recordar que el profesor la devolvía para atrás la entidad
            entity = await _repository.AddAsync(entity);

            SaveViewModel svm = _mapper.Map<SaveViewModel>(entity);
            return svm;
        }

        public virtual async Task<List<ViewModel>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            var newList = _mapper.Map<List<ViewModel>>(list);
            //Recordar revisar que devuelva los que su delete sea false.
            return newList;
        }

        public virtual async Task<ViewModel> GetByIdAsync(string Id)
        {
            var entity = await _repository.GeEntityByIDAsync(Id);
            return _mapper.Map<ViewModel>(entity);
        }

        public virtual async Task UpdateAsync(SaveViewModel vm, string Id)
        {
            Entity entity = _mapper.Map<Entity>(vm);
            await _repository.UpdateAsync(entity, Id);
        }
        public virtual async Task Delete(string Id)
        {
            var entity = await _repository.GeEntityByIDAsync(Id);
            await _repository.DeleteAsync(entity);
        }

        public async Task<SaveViewModel> GetByIdSaveViewModelAsync(string Id)
        {
            var entity = await _repository.GeEntityByIDAsync(Id);
            return _mapper.Map<SaveViewModel>(entity);
        }

        public async Task<List<ViewModel>> FindAllAsync(Expression<Func<Entity, bool>> filter)
        {
            var query = await _repository.FindAllAsync(filter);
            return _mapper.Map<List<ViewModel>>(query);
        }
    }
}
