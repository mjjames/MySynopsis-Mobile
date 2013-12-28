using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.Pages
{
    public class RegistrationPage : ContentPage
    {
        private RegisterViewModel _viewModel;
        private ActivityIndicator _loading;
        public RegistrationPage(RegisterViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Title = PageResources.RegistrationTitle;
            BindingContext = _viewModel;

            SetBinding(TitleProperty, new Binding("Title", BindingMode.OneWay));

            _loading = new ActivityIndicator();
            _loading.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsAuthenticating", BindingMode.OneWay));
            SetBinding(IsBusyProperty, new Binding("IsAuthenticating", BindingMode.OneWay));

            var layout = new GridLayout
            {
                DefaultColumnSpacing = 10,
                DefaultRowSpacing = 5
            };

            var intro = new Label
                {
                    Text = PageResources.RegistrationIntro,
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, true),
                };
            layout.AddVertical(new[]{
                intro, 
                new Label
                {
                    Text = PageResources.Name
                },
                new Label
                {
                    Text = PageResources.EmailAddress
                }
            });
            GridLayout.SetColumnSpan(intro, 2);
            var nameEntry = new Entry
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true)
            };
            nameEntry.SetBinding<RegisterViewModel>(Entry.TextProperty, vm => vm.Name);
            layout.Add(nameEntry, 1, 1);

            var emailEntry = new Entry
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.Fill, true)
            };
            emailEntry.SetBinding<RegisterViewModel>(Entry.TextProperty, vm => vm.EmailAddress);
            layout.Add(emailEntry, 1, 2);


            var meterConfigurationList = new ListView
            {
                //todo: change this to use a custom cell which has the images etc
                ItemTemplate = new DataTemplate(() => new TextCell())
            };
            //todo: itemsource binding errors
            //_providerList.SetBinding(ListView.ItemSourceProperty, new Binding("Providers", BindingMode.OneWay));
            meterConfigurationList.ItemSource = _viewModel.MeterConfigurations;
            meterConfigurationList.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedConfiguration", BindingMode.TwoWay));
            meterConfigurationList.ItemTemplate.SetBinding(TextCell.TextProperty, new Binding("Name"));
            meterConfigurationList.ItemTemplate.SetBinding(TextCell.CommandProperty, new Binding("Command"));
            layout.AddVertical(new View[]{ new Label{ Text="Choose Your Meter Setup"}, meterConfigurationList});

            //var option1 = new Button
            //{
            //    Text = PageResources.QuickSet1
            //};
            //option1.SetBinding<RegisterViewModel>(Button.CommandProperty, vm => vm.ConfigureOptionOne);
            //layout.AddVertical(option1);

            //var option2 = new Button
            //{
            //    Text = PageResources.QuickSet2
            //};
            //option2.SetBinding<RegisterViewModel>(Button.CommandProperty, vm => vm.ConfigureOptionTwo);
            //layout.AddVertical(option2);

            //var option3 = new Button
            //{
            //    Text = PageResources.QuickSet3
            //};
            //option3.SetBinding<RegisterViewModel>(Button.CommandProperty, vm => vm.ConfigureOptionThree);
            //layout.AddVertical(option3);

            var confirmSetup = new Button
            {
                Text = PageResources.ConfirmSetup
            };
            confirmSetup.SetBinding<RegisterViewModel>(Button.CommandProperty, vm => vm.ConfirmSetup);
            layout.AddVertical(confirmSetup);

            _viewModel.PostPersistAction = (user) =>
            {
                Navigation.PopModal();
            };

            Content = layout;
        }
    }
}
