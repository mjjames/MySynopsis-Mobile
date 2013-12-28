using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using MySynopsis.BusinessLogic.ViewModels;
using MySynopsis.BusinessLogic.Services;
using MySynopsis.BusinessLogic.Mocks;
using MySynopsis.UI;
using MySynopsis.UI.Pages;
using MySynopsis.BusinessLogic;

namespace MySynopsis.Android
{
    public static class Factory
    {

        static Factory()
        {
            ServiceClient = new MobileServiceClient("https://synopsis.azure-mobile.net/");
        }
        public static IMobileServiceClient ServiceClient { get; private set; }

        public static LoginViewModel GetLoginViewModel(Context context)
        {
            return new LoginViewModel(GetLoginService(context));
        }

        public static IUserLoginService GetLoginService(Context context)
        {
            //Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> authenticate = async (provider) => await ServiceClient.LoginAsync(context, provider);
            //return new UserLoginService(GetUserService(), authenticate);
            var mockService = new MockUserService();
            mockService.SetExpectedUserId(MySynopsis.BusinessLogic.Mocks.MockUserService.ExpectedUserStatus.UnregisteredUser);
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
           {
               return Task.FromResult(new MobileServiceUser("56565467546757"));
           };
            return new UserLoginService(mockService, login);
        }

        private static IUserService GetUserService()
        {
            var mockService = new MockUserService();
            mockService.PersistAction = (User user) =>
            {
                user.Id = 6;
                return user;
            };
            return mockService;
            //return new UserService(ServiceClient);
        }


        public static void Dispose()
        {
            //see if service client is disposible and clean up as required
            var disposibleClient = ServiceClient as IDisposable;
            if (disposibleClient != null)
            {
                disposibleClient.Dispose();
            }
        }

        internal static void RegisterPages()
        {

            PageLocator.Register<HomePage>(delegate
            {
                return new HomePage();
            });

            PageLocator.Register<LoginPage>(delegate(object context)
            {
                return new LoginPage(GetLoginViewModel(context as Context));
            });

            PageLocator.Register<RegistrationPage>(delegate(object state)
            {
                return new RegistrationPage(GetRegistrationViewModel(state as User));
            });
        }

        private static RegisterViewModel GetRegistrationViewModel(User user)
        {
            return new RegisterViewModel(GetUserService(), user);
        }
    }
}