using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBanking.Core.Application.Interfaces.Services;
using NetBanking.Core.Application.Interfaces.Services.Domain_Services;
using NetBanking.Core.Application.Services;
using NetBanking.Core.Application.Services.Domain_Services;
using System.Reflection;

namespace NetBanking.Core.Application
{
    public static class ServiceRegistration
    {
        public static void ApplicationLayerRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IClientService, ClientService>();

            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<ICreditCardService, CreditCardService>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<ISavingsAccountService, SavingsAccountService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }
    }
}
