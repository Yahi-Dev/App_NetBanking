using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Repositories;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.CreditCard;
using NetBanking.Core.Application.ViewModels.Delete;
using NetBanking.Core.Application.ViewModels.Users;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Core.Application.Services.Domain_Services
{
    public class CreditCardService : GenericService<SaveCreditCardViewModel, CreditCardViewModel, CreditCard>, ICreditCardService
    {
        private readonly IMapper _mapper;
        private readonly ICreditCardRepository _repository;
        private readonly AuthenticationResponse user;
        public CreditCardService(ICreditCardRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<List<CreditCardViewModel>> GetByOwnerIdAsync(string Id)
        {
            var list = await _repository.FindAllAsync(x => x.UserId == Id);
            return _mapper.Map<List<CreditCardViewModel>>(list);
        }

        public override async Task<SaveCreditCardViewModel> AddAsync(SaveCreditCardViewModel vm)
        {
            CreditCard entity = _mapper.Map<CreditCard>(vm);
            string candidateId = "";
            do
            {
                candidateId = CodeGeneratorHelper.GenerateCode(typeof(CreditCard));
            }
            while ((await _repository.FindAllAsync(x => x.Id == candidateId)).Count != 0);
            entity.Id = candidateId;
            entity = await _repository.AddAsync(entity);

            SaveCreditCardViewModel svm = _mapper.Map<SaveCreditCardViewModel>(entity);
            return svm;
        }


        public async Task<DeleteStatus> Delete(string Id)
        {
            var creditCard = await _repository.GeEntityByIDAsync(Id);
            DeleteStatus vm = new();
            if (creditCard.Amount >= 0)
            {
                vm.Error = "Este usuario tiene una deuda pendiente";
                vm.HasError = true;
            }
            else
            {
                vm.Error = "Se ha borrado la tarjeta de credito";
                await _repository.DeleteAsync(creditCard);
            }
            return vm;
        }
    }
}
