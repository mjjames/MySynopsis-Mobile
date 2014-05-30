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
using Xamarin.Forms;

namespace MySynopsis.Android
{
    public static class Factory
    {

        static Factory()
        {
            ServiceClient = new MobileServiceClient("https://mysynopsis.azure-mobile.net/");
        }
        public static IMobileServiceClient ServiceClient { get; private set; }

        public static LoginViewModel GetLoginViewModel(Context context)
        {
            return new LoginViewModel(GetLoginService(context));
        }

        public static IUserLoginService GetLoginService(Context context)
        {
            Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> authenticate = async (provider) => await ServiceClient.LoginAsync(context, provider);
            return new UserLoginService(GetUserService(), authenticate);
            // var mockService = new MockUserService();
            // mockService.SetExpectedUserId(MySynopsis.BusinessLogic.Mocks.MockUserService.ExpectedUserStatus.UnregisteredUser);
            // Func<MobileServiceAuthenticationProvider, Task<MobileServiceUser>> login = (provider) =>
            //{
            //    return Task.FromResult(new MobileServiceUser("56565467546757"));
            //};
            // return new UserLoginService(mockService, login);
        }

        private static IUserService GetUserService()
        {
            //var mockService = new MockUserService();
            //mockService.PersistAction = (User user) =>
            //{
            //    user.Id = 6;
            //    return user;
            //};
            //return mockService;
            return new UserService(ServiceClient);
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

        internal static void RegisterPages(Context context)
        {

            PageLocator.Register<HomePage>(delegate
            {
                return new HomePage(GetUserService());
            });

            PageLocator.Register<LoginPage>(delegate(object state)
            {
                return new LoginPage(GetLoginViewModel(context));
            });

            PageLocator.Register<RegistrationPage>(delegate(object state)
            {
                return new RegistrationPage(GetRegistrationViewModel(state as User));
            });

            PageLocator.Register<RecordReadingsPage>(delegate(object state)
            {
                var vm = GetRecordReadingsViewModel(state as User);
                var page = new RecordReadingsPage(vm);
                vm.PostPersistAction = () => page.DisplayAlert("Persist Reading", "Reading Persisted", "OK", "");
                return page;
            });

            PageLocator.Register<RecentUsagePage>(delegate(object state)
            {
                return new RecentUsagePage("file:///android_asset/", state as User);
            });

            PageLocator.Register<TabbedPage>(delegate
            {
                return new TabbedPage
                {
                    Children = {
                        new NavigationPage(PageLocator.Get<HomePage>()){ Title = "Home"},
                        new NavigationPage(){Title = "New Reading", ClassId = "RecordingReading"},
                        new NavigationPage(){Title = "Recent Usage", ClassId = "RecentUsage"},
                        new NavigationPage(){Title = "Settings", ClassId = "Settings"}
                    }
                };
            });
        }

        private static RecordReadingsViewModel GetRecordReadingsViewModel(User user)
        {
            return new RecordReadingsViewModel(user, GetDataReadingService());
        }

        private static IDataReadingService GetDataReadingService()
        {
            return new DataReadingService(ServiceClient);
        }

        private static RegisterViewModel GetRegistrationViewModel(User user)
        {
            return new RegisterViewModel(GetUserService(), user);
        }
    }
}