using MySynopsis.BusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.Pages
{
    public class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;
        private ListView _providerList;
        private ActivityIndicator _loading;
        public LoginPage(LoginViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Title = PageResources.LoginTitle;
            BindingContext = _viewModel;

            SetBinding(TitleProperty, new Binding("Title", BindingMode.OneWay));

            _providerList = new ListView
            {
                //todo: change this to use a custom cell which has the images etc
                ItemTemplate = new DataTemplate(() =>
                {
                    var cell = new TextCell
                    {
                        Command = _viewModel.Login
                    };
                    return (Cell)cell;
                })
            };
            //todo: itemsource binding errors
            //_providerList.SetBinding(ListView.ItemSourceProperty, new Binding("Providers", BindingMode.OneWay));
            _providerList.ItemSource = _viewModel.Providers;
            _providerList.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedProvider", BindingMode.TwoWay));
            _providerList.ItemTemplate.SetBinding(TextCell.TextProperty, new Binding("Name"));

            _loading = new ActivityIndicator();
            _loading.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsAuthenticating", BindingMode.OneWay));
            SetBinding(IsBusyProperty, new Binding("IsAuthenticating", BindingMode.OneWay));

            var layout = new StackLayout
            {
                Orientation = StackLayout.StackOrientation.Vertical
            };
            layout.Add(_loading);
            layout.Add(_providerList);

            Content = layout;
            _viewModel.IsAuthenticating = true;
        }

        public LoginViewModel ViewModel
        {
            get { return _viewModel; }
        }

      
    }
}
