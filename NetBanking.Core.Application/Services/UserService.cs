using AutoMapper;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.ViewModels.SavingsAccount;
using NetBanking.Core.Application.ViewModels.Users;

namespace NetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ISavingsAccountService _savingsAccountService;

        public UserService(IMapper mapper, IAccountService accountService,
            ISavingsAccountService savingsAccountService)
        {
            _savingsAccountService = savingsAccountService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginrequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginrequest);
            return userResponse;
        }

        public async Task<ServiceResult> UpdateAsync(SaveUserViewModel vm)
        {
            var user = _mapper.Map<RegisterRequest>(vm);
            if(vm.InitialAmount != 0)
            {
                if (vm.Role == RolesEnum.Client.ToString())
                {
                    var savingsAccount = await _savingsAccountService.GetByOwnerIdAsync(user.Id);
                    var savingsAccountVm = savingsAccount.Find(x => x.IsMain == true && x.UserId == user.Id);

                    savingsAccountVm.Amount += vm.InitialAmount;
                    SaveSavingsAccountViewModel savingsAccountRequest = _mapper.Map<SaveSavingsAccountViewModel>(savingsAccountVm);
                    await _savingsAccountService.UpdateAsync(savingsAccountRequest, savingsAccountRequest.Id);
                }
            }
            var response = await _accountService.UpdateUserAsync(user);
            return response;
        }

        public async Task SingOutAsync()
        {
            await _accountService.SingOutAsync();
        }

        public async Task<ServiceResult> RegisterAsync(SaveUserViewModel vm, string origin, string userRole)
        {
            if (vm.formFile != null)
            {
                vm.ImageURL = UploadImage.UploadFile(vm.formFile, vm.IdCard, "User");
            }
            RegisterRequest resgisterRequest = _mapper.Map<RegisterRequest>(vm);
            var result = await _accountService.RegisterUserAsync(resgisterRequest, origin, userRole);
            if (userRole == RolesEnum.Client.ToString())
            {
                await _savingsAccountService.SaveUserWIthMainAccount(vm);
            }
            return result;
        }

        public async Task<string> ConfirmEmailAsync(string UserId, string token)
        {
            return await _accountService.ConfirmAccountAsync(UserId, token);
        }

        public async Task<ServiceResult> ForgotPasswordAsync(ForgorPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPassswordAsync(forgotRequest, origin);
        }

        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }

        public async Task<SaveUserViewModel> GetByIdAsync(string UserId)
        {
            var user = await _accountService.GetByIdAsync(UserId);
            SaveUserViewModel vm = _mapper.Map<SaveUserViewModel>(user);
            vm.InitialAmount = 0;
            return vm;
        }

        public async Task Remove(string Id)
        {
            var user = await GetByIdAsync(Id);
            var account = _mapper.Map<DtoAccounts>(user);
            await _accountService.Remove(account);
        }
    }
}
