using System;
namespace MySynopsis.BusinessLogic.Services
{
    public interface IUserLoginService
    {
        System.Threading.Tasks.Task<MySynopsis.BusinessLogic.Models.UserLoginResult> Login(Microsoft.WindowsAzure.MobileServices.MobileServiceAuthenticationProvider authenticationProvider);
        System.Collections.Generic.IEnumerable<MySynopsis.BusinessLogic.Models.LoginProvider> LoginProviders { get; }
    }
}
