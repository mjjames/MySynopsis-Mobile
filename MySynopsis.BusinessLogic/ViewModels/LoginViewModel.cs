using MySynopsis.BusinessLogic.Models;
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MySynopsis.BusinessLogic.ViewModels
{
	public class LoginViewModel :ViewModelBase
	{
		private readonly IUserLoginService _loginService;
		private LoginProvider _selectedProvider;
		private bool _isAuthenticating;
		private ICommand _loginCommand;
		private UserLoginResult _loginResult;

		public LoginViewModel(IUserLoginService loginService) :base()
		{
			_loginService = loginService;
			Providers = new ObservableCollection<LoginProvider>(_loginService.LoginProviders);
		}

		public ObservableCollection<LoginProvider> Providers { get; private set; }

		public LoginProvider SelectedProvider
		{
			get
			{
				return _selectedProvider;
			}
			set
			{
				if (_selectedProvider == value)
				{
					return;
				}
				_selectedProvider = value;
				NotifyPropertyChanged();
			}
		}

		public bool IsAuthenticating
		{
			get
			{
				return _isAuthenticating;
			}
			set
			{
				if (_isAuthenticating == value)
				{
					return;
				}
				_isAuthenticating = value;
				NotifyPropertyChanged();
			}
		}

		public UserLoginResult LoginResult
		{
			get
			{
				return _loginResult;
			}
			set
			{
				if (_loginResult == value)
				{
					return;
				}
				_loginResult = value;
				NotifyPropertyChanged();
			}
		}

	  
		private async Task LoginTask()
		{
			IsAuthenticating = true;
			if (SelectedProvider == null)
			{
				IsAuthenticating = false;
				LoginResult = new UserLoginResult(new Exception("No Provider Selected"));
				return;
			}
			var result = await _loginService.Login(SelectedProvider.Provider);
			IsAuthenticating = false;
			LoginResult = result;
		}

		public ICommand Login
		{
			get
			{
				if(_loginCommand == null)
				{
					_loginCommand = new DelegateCommand (async obj =>  await LoginTask(), obj => this.SelectedProvider != null);
				}
				return _loginCommand;
			
			}
		}

	   
		
	}
}
