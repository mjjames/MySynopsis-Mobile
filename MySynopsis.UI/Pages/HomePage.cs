using MySynopsis.BusinessLogic.Services;
using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MySynopsis.UI.Pages
{
    public class HomePage : ContentPage
    {
        
        public HomePage(IUserService userService)
        {
            Title = "Home";
            var layout = new StackLayout();
            layout.Children.Add(new Label{
                Text = "No Content"
            });
            Content = layout;
            
            var login = PageLocator.Get<LoginPage>(this);
            login.ViewModel.PropertyChanged += loginViewModelPropertyChanged;
            Navigation.PushModalAsync(login);
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
                viewModel.SelectedProvider = null;
                return;
            }
            if (viewModel.LoginResult.RequiresRegistration)
            {
                var register = await DisplayAlert(SystemMessages.RequiresRegistrationTitle, SystemMessages.RequiresRegistrationMessage, SystemMessages.Signup, SystemMessages.NoThanks);
                if (register)
                {
                    await Navigation.PopModalAsync();
                    var page = PageLocator.Get<RegistrationPage>(viewModel.LoginResult.UserDetails);
                    page.ViewModel.PostPersistAction = async (user) =>
                    {
                        await PopulateRecordReadingsTab(user);
                    };
                    await Navigation.PushModalAsync(page);
                    return;
                }
                else
                {
                    viewModel.SelectedProvider = null;
                    viewModel.LoginResult = null;
                    return;
                }
            }
            await Navigation.PopModalAsync();
            var populationTasks = new[]{
                PopulateRecordReadingsTab(viewModel.LoginResult.UserDetails),
                PopulateRecentUsageTab(viewModel.LoginResult.UserDetails)
            };
            await Task.WhenAll(populationTasks);
        }

        private async Task PopulateRecordReadingsTab(BusinessLogic.User user)
        {
            var newPage = PageLocator.Get<RecordReadingsPage>(user);

            var tabs = Parent.Parent as TabbedPage;
            var navPage = tabs.Children.OfType<NavigationPage>().First(p => p.ClassId == "RecordingReading");
            await navPage.PushAsync(newPage);
            
        }
        private async Task PopulateRecentUsageTab(BusinessLogic.User user)
        {
            var newPage = PageLocator.Get<RecentUsagePage>(user);

            var tabs = Parent.Parent as TabbedPage;
            var navPage = tabs.Children.OfType<NavigationPage>().First(p => p.ClassId == "RecentUsage");
            await navPage.PushAsync(newPage);
        }
    }
}
