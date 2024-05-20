using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.Loan;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class LoanService : GenericService<SaveLoanViewModel, LoanViewModel, Loan>, ILoanService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILoanRepository _repository;
        public LoanService(
            IMapper mapper,
            ILoanRepository repository) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public override async Task<SaveLoanViewModel> AddAsync(SaveLoanViewModel vm)
        {
            Loan entity = _mapper.Map<Loan>(vm);
            string candidateId = "";
            do
            {
                candidateId = CodeGeneratorHelper.GenerateCode(typeof(Loan));
            }
            while ((await _repository.FindAllAsync(x => x.Id == candidateId)).Count != 0);
            entity.Id = candidateId;
            entity = await _repository.AddAsync(entity);

            SaveLoanViewModel svm = _mapper.Map<SaveLoanViewModel>(entity);
            return svm;
        }

        public async Task<List<LoanViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<LoanViewModel>>(list);
        }

        public async Task<DeleteStatus> Delete(string Id)
        {
            var loan = await _repository.GeEntityByIDAsync(Id);
            DeleteStatus vm = new();

            if (loan.Amount > 0)
            {
                vm.Error = $"Este usuario tiene una deuda pendiente de {loan.Debt}.";
                vm.HasError = true;
            }
            else
            {
                vm.Error = "Se ha borrado el prestamo";
                await _repository.DeleteAsync(loan);
            }
             return vm;
        }
    }
}
