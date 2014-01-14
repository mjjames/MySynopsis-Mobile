using MySynopsis.BusinessLogic.Services;
using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.Pages
{
    public class HomePage : MultiPage<Page>
    {
        
        public HomePage(IUserService userService)
        {
            Title = "Home";

            var login = PageLocator.Get<LoginPage>(this);
            login.ViewModel.PropertyChanged += loginViewModelPropertyChanged;
            Navigation.PushModal(login);

        }

        private async void loginViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = (LoginViewModel)sender;
            switch (e.PropertyName)
            {
                case "LoginResult":
                    await LoginResultChanged(vm);
                    vm.PropertyChanged -= loginViewModelPropertyChanged;
                    break;
            }
        }

        private async Task LoginResultChanged(LoginViewModel viewModel)
        {
            if (viewModel.LoginResult == null)
            {
                return;
            }

            if (viewModel.LoginResult.AuthenticationFailed)
            {
                await DisplayAlert(SystemMessages.AuthenticationFailedTitle, SystemMessages.AuthenticationFailedMessage, SystemMessages.OK, "");
                return;
            }
            if (viewModel.LoginResult.RequiresRegistration)
            {
                var register = await DisplayAlert(SystemMessages.RequiresRegistrationTitle, SystemMessages.RequiresRegistrationMessage, SystemMessages.Signup, SystemMessages.NoThanks);
                if (register)
                {
                    Navigation.PopModal();
                    var page = PageLocator.Get<RegistrationPage>(viewModel.LoginResult.UserDetails);
                    page.ViewModel.PostPersistAction = (user) =>
                    {
                        var newPage = PageLocator.Get<RecordReadingsPage>(user);
                        Add(newPage);
                        Navigation.PopModal();
                    };
                    Navigation.PushModal(page);
                    return;
                }
                else
                {
                    viewModel.SelectedProvider = null;
                    viewModel.LoginResult = null;
                    return;
                }
            }
            Navigation.PopModal();
        }
    }
}
