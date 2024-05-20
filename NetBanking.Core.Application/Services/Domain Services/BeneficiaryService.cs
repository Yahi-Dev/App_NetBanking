using AutoMapper;
using Microsoft.AspNetCore.Http;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.Beneficiary;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IBeneficiaryRepository _repository;
        private readonly IAccountService _accountService;
        public BeneficiaryService(
            IMapper mapper,
            IBeneficiaryRepository repository,
            IAccountService accountService) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
            _accountService = accountService;
        }

        public override async Task<SaveBeneficiaryViewModel> AddAsync(SaveBeneficiaryViewModel vm)
        {
            Beneficiary entity = _mapper.Map<Beneficiary>(vm);
            string candidateId = "";
            do
            {
                candidateId = CodeGeneratorHelper.GenerateCode(typeof(Beneficiary));
            }
            while ((await _repository.FindAllAsync(x => x.Id == candidateId)).Count != 0);
            entity.Id = candidateId;
            entity = await _repository.AddAsync(entity);

            SaveBeneficiaryViewModel svm = _mapper.Map<SaveBeneficiaryViewModel>(entity);
            return svm;
        }

        public async Task<List<BeneficiaryViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            List<BeneficiaryViewModel> vm = new();
            foreach (var item in list)
            {
                BeneficiaryViewModel element = new()
                {
                    Id = item.Id,
                    UserId = item.UserId,
                    UserAccount = await _accountService.GetByIdAsync(item.UserId),
                    BeneficiaryId = item.BeneficiaryId,
                    BeneficiaryUser = await _accountService.GetByIdAsync(item.BeneficiaryId),
                    BeneficiaryAccountId = item.BeneficiaryAccountId
                };
                vm.Add(element);
            }
            return vm;
        }
    }
}