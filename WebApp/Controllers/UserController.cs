using Microsoft.AspNetCore.Mvc;
using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Dtos.Error;
using NetBanking.Core.Application.Enums;
using NetBanking.Core.Application.Helpers;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Singelton;
using NetBanking.Core.Application.ViewModels.Users;
using WebApp.Middlewares;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthenticationResponse _userViewModel;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userViewModel = httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _userService = userService;
        }


        public IActionResult AccessDeniedAdmin()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            if (_userViewModel.Roles.Contains(RolesEnum.Admin.ToString()))
            {
                return View("AccessDeniedAdmin");
            }
            return View();
        }

        //INDEX
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(TempData["Success"]?.ToString()))
            {
                LoginViewModel vm = new();
                vm.Error = StringStorage.Instance.GetStoredString();
                StringStorage.Instance.SetStoredString("");
                return View(vm);
            }
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                if (userVm.Roles.Contains(RolesEnum.Client.ToString()))
                {
                    return RedirectToRoute(new { controller = "Client", action = "Home" });
                }
                else if (userVm.Roles.Contains(RolesEnum.Admin.ToString()))
                {
                    return RedirectToRoute(new { controller = "Admin", action = "DashBoard" });
                }
                
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }

            return View(vm);
        }

        //CONFIRM EMAIL
        public async Task<IActionResult> ConfirmEmailAsync(string UserId, string token)
        {
            string response = await _userService.ConfirmEmailAsync(UserId, token);
            return View("ConfirmEmail", response);
        }

        //LOGOUT
        public async Task<IActionResult> LogOut()
        {
            await _userService.SingOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //REGISTER
        public IActionResult Register()
        {
            return View(new SaveUserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ServiceResult response = await _userService.RegisterAsync(vm, origin, RolesEnum.Client.ToString());
            if (!response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View(vm);
            }
            StringStorage.Instance.SetStoredString(response.Error);
            return RedirectToAction("Index");
        }

        // FORGOT PASSWORD
        public IActionResult ForgotPassword()
        {
            return View(new ForgorPasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            ForgorPasswordViewModel vm = new();
            vm.Email = email;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ServiceResult response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View("Index", vm);
            }
            return RedirectToRoute(new { controller="User", action="Index" });
        }

       
        //LOGOUT
        [HttpPost]
        public async Task<IActionResult> LogOut(LoginViewModel vm)
        {
            await _userService.SingOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }


        //RESET PASSWORD
        public IActionResult ResetPassword(string Token)
        {
            return View(new ResetPasswordViewModel { Token = Token });
        }

       
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return View("ResetPassword", vm);
            }
            ServiceResult response = await _userService.ResetPasswordAsync(vm);
            if(response.HasError)
            {
                vm.Error = response.Error;
                vm.HasError = response.HasError;
                return View("ResetPassword", vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }
    }
}
