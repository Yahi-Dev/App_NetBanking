using NetBanking.Core.Application.Dtos.Account;
using NetBanking.Core.Application.Helpers;

namespace WebApp.Middlewares
{
    public class ValidateUserSession
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userViewModel == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
