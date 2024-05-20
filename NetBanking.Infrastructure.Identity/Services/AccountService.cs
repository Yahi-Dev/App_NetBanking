using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Infrastructure.Identity.Entities;
using System.Text;

namespace NetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<AppUser> userManager,
                             SignInManager<AppUser> signInManager,
                             IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        //GETBYID
        public async Task<DtoAccounts> GetByIdAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            DtoAccounts dtoaccount = new()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName,
                ImageURL = user.ImageURL,
                IdCard = user.IdCard,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
                Password = user.PasswordHash
            };
            return dtoaccount;
        }

        //GETBYID
        public async Task<DtoAccounts> GetByEmail(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            DtoAccounts dtoaccount = new()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                UserName = user.UserName,
                ImageURL = user.ImageURL,
                IdCard = user.IdCard,
                IsActive = user.IsActive,
                PhoneNumber = user.PhoneNumber,
            };
            return dtoaccount;
        }


        //DELETE USER
        public async Task Remove(DtoAccounts account)
        {
            ServiceResult response = new();

            var user = await _userManager.FindByIdAsync(account.Id);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"This user does not exist now";
            }
            await _userManager.DeleteAsync(user);
        }

        //USERS GETALL

        public async Task<List<DtoAccounts>> GetAllUsers()
        {

            var userList = await _userManager.Users.ToListAsync();
            List<DtoAccounts> DtoUserList = new();
            foreach (var user in userList)
            {
                var userDto = new DtoAccounts();

                userDto.ImageURL = user.ImageURL;
                userDto.FirstName = user.FirstName;
                userDto.LastName = user.LastName;
                userDto.IsActive = user.IsActive;
                userDto.IdCard = user.IdCard;
                userDto.Email = user.Email;
                userDto.Id = user.Id;
                userDto.Roles = _userManager.GetRolesAsync(user).Result.ToList();
                DtoUserList.Add(userDto);
            }
            return DtoUserList;
        }

        //USER AUTHENTICATION
        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No accounts registered under Email {request.Email}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid Credential for {request.Email}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account not confirmed for {request.Email}";
                return response;
            }
            if (user.IsActive == false)
            {
                response.HasError = true;
                response.Error = $"Your account user {request.Email} is not active please get in contact with a manager";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = roleList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.IdCard = user.IdCard;
            response.UserStatus = true;
            response.ImageUrl = user.ImageURL;

            return response;
        }

        //CHANGE USER STATUS
        public async Task<ServiceResult> ChangeUserStatus(RegisterRequest request)
        {
            ServiceResult response = new();
            var userget = await _userManager.FindByIdAsync(request.Id);
            {
                userget.IsActive = request.IsActive;
            }
            var result = await _userManager.UpdateAsync(userget);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"There was an error while trying to update the user{userget.UserName}";
            }
            return response;
        }

            //EDITUSER
        public async Task<ServiceResult> UpdateUserAsync(RegisterRequest request)
        {
            ServiceResult response = new();
            var userget = await _userManager.FindByIdAsync(request.Id);
            {
                userget.Id = request.Id;
                userget.PhoneNumber = request.PhoneNumber;
                userget.UserName = request.UserName;
                userget.FirstName = request.FirstName;
                userget.LastName = request.LastName;
                userget.Email = request.Email;
                userget.IdCard = request.IdCard;
                userget.ImageURL = request.ImageURL;
            }
            if (request.Password != null)
            {
                var Token = await _userManager.GeneratePasswordResetTokenAsync(userget);
                await _userManager.ResetPasswordAsync(userget, Token, request.Password);
            }
            if(request.ImageURL != null)
            {
                userget.ImageURL = UploadImage.UploadFile(request.formFile, request.Id, "User", true, request.ImageURL);
            }
            else
            {
                userget.ImageURL = UploadImage.UploadFile(request.formFile, request.Id, "User", false, userget.ImageURL);
            }
            var result = await _userManager.UpdateAsync(userget);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"There was an error while trying to update the user{userget.UserName}";
            }
            return response;
        }


        //REGISTER USER

        public async Task<ServiceResult> RegisterUserAsync(RegisterRequest request, string origin, string UserRoles)
        {
            ServiceResult response = new()
            {
                HasError = true,
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username {request.UserName} is already taken";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email {request.Email} is already registered";
                return response;
            }

            var user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                IsActive = request.IsActive,
                IdCard = request.IdCard,
                ImageURL = request.ImageURL,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles);
                var verificationURI = await SendVerificationUri(user, origin);
                await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                {
                    To = user.Email,
                    Body = $"Please confirm your account visiting this URL {verificationURI}",
                    Subject = "Confirm registration"
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }

            response.Error = "Favor confirmar la cuenta.";
            return response;
        }


        //CONFIRMACCOUNT
        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No user register under this {user.Email} account";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirm for {user.Email} you can now  use the app";
            }
            else
            {
                return $"An error occurred wgile confirming {user.Email}.";
            }
        }

        //FORGOTPASSWORD
        public async Task<ServiceResult> ForgotPassswordAsync(ForgotPasswordRequest request, string origin)
        {
            ServiceResult response = new()
            {
                HasError = false,
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            var verificationURI = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your account visiting this URL {verificationURI}",
                Subject = "reset password"
            });

            return response;

        }


        //RESETPASSWORD
        public async Task<ServiceResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ServiceResult response = new()
            {
                HasError = false,
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        //SINGOUT
        public async Task SingOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #region PrivateMethods

        //SENDFORGOTPASSWORDURI
        private async Task<string> SendForgotPasswordUri(AppUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "Token", code);

            return verificationUri;
        }


        //SEMDVERIFICATIONURI
        private async Task<string> SendVerificationUri(AppUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "Token", code);

            return verificationUri;
        }

        #endregion
    }
}
