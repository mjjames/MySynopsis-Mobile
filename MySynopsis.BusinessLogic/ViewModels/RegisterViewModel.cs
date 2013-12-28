using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private DelegateCommand _optionThreeCommand;
        private DelegateCommand _optionOneCommand;
        private DelegateCommand _optionTwoCommand;
        private DelegateCommand _confirmSetup;
        private IUserService _userService;
        private User _user;
        private TextInfo _textService;
        private bool _isPersisting;
        private QuickMeterConfiguration _configuration;

        public RegisterViewModel(IUserService userService, User user)
        {
            _userService = userService;
            _user = user;

            MeterConfigurations = new ObservableCollection<QuickMeterConfiguration>(new[]{
                new QuickMeterConfiguration{
                    Command = ConfigureOptionOne,
                    Name = "Elec, Gas, Water" //todo: resource this
                },
                new QuickMeterConfiguration{
                    Command = ConfigureOptionTwo,
                    Name = "Elec, Gas" //todo: resource this
                },
                new QuickMeterConfiguration{
                    Command = ConfigureOptionThree,
                    Name = "Elec" //todo: resource this
                },
                new QuickMeterConfiguration{
                    Command = CustomSetup,
                    Name = "Custom" //todo: resource this
                }
            });

            PropertyChanged += (o, e) =>
            {
                if(_confirmSetup != null){
                    _confirmSetup.RaiseCanExecuteChanged();
                }
            };
        }

        public ICommand ConfigureOptionOne
        {
            get
            {
                if (_optionOneCommand == null)
                {
                    _optionOneCommand = new DelegateCommand(obj => ConfigureOption(ConfigurationOption.ElecGasWater), (user) => !_user.MeterConfiguration.Any());
                }
                return _optionOneCommand;
            }
        }

        public ICommand ConfigureOptionTwo
        {
            get
            {
                if (_optionTwoCommand == null)
                {
                    _optionTwoCommand = new DelegateCommand(obj => ConfigureOption(ConfigurationOption.ElecGas), (user) => !_user.MeterConfiguration.Any());
                }
                return _optionTwoCommand;
            }
        }

        public ICommand ConfigureOptionThree
        {
            get
            {
                if (_optionThreeCommand == null)
                {
                    _optionThreeCommand = new DelegateCommand(obj => ConfigureOption(ConfigurationOption.ElecWater), (user) => !_user.MeterConfiguration.Any());
                }
                return _optionThreeCommand;
            }
        }

        public string Name
        {
            get
            {
                return _user.Name;
            }
            set
            {
                if (value == _user.Name)
                {
                    return;
                }
                _user.Name = value;
                NotifyPropertyChanged();
            }
        }

        private void ConfigureOption(ConfigurationOption configurationOption)
        {
            _user.MeterConfiguration.Clear();
            switch (configurationOption)
            {
                case ConfigurationOption.ElecGas:
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Gas",
                        Type = MeterType.Gas
                    });
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Elec",
                        Type = MeterType.Electricity
                    });
                    break;
                case ConfigurationOption.ElecGasWater:
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Gas",
                        Type = MeterType.Gas
                    });
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Elec",
                        Type = MeterType.Electricity
                    });
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Water",
                        Type = MeterType.Water
                    });
                    break;
                case ConfigurationOption.ElecWater:
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Elec",
                        Type = MeterType.Electricity
                    });
                    _user.MeterConfiguration.Add(new Meter
                    {
                        Name = "Water",
                        Type = MeterType.Water
                    });
                    break;

            }
            _confirmSetup.RaiseCanExecuteChanged();
            _optionOneCommand.RaiseCanExecuteChanged();
            _optionTwoCommand.RaiseCanExecuteChanged();
            _optionThreeCommand.RaiseCanExecuteChanged();
        }

        public ICommand CustomSetup
        {
            get
            {
                return null;
            }
        }

        public ICommand ConfirmSetup
        {
            get
            {
                if (_confirmSetup == null)
                {
                    _confirmSetup = new DelegateCommand(async obj => await PersistUserSetup(obj), obj => _user.IsValid);
                }
                return _confirmSetup;
            }
        }

        public Action<User> PostPersistAction { get; set; }

        public bool IsPersisting
        {
            get
            {
                return _isPersisting;
            }
            set
            {
                if (_isPersisting == value)
                {
                    return;
                }
                _isPersisting = value;
                NotifyPropertyChanged();
            }
        }

        private async Task PersistUserSetup(object obj)
        {
            IsPersisting = true;
            var user = await _userService.Persist(_user);
            _user = user;
            IsPersisting = false;
            if (PostPersistAction != null)
            {
                PostPersistAction(user);
            }
        }

        public string EmailAddress
        {
            get {
                return _user.EmailAddress;
            }
            set
            {
                if (value == _user.EmailAddress)
                {
                    return;
                }
                _user.EmailAddress = value;
                NotifyPropertyChanged();
            }
        }

        public QuickMeterConfiguration SelectedConfiguration
        {
            get { return _configuration; }
            set
            {
                if (_configuration == value)
                {
                    return;
                }
                _configuration = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<QuickMeterConfiguration> MeterConfigurations { get; private set; }
    }

    public class QuickMeterConfiguration
    {
        public string Name { get; set; }
        public ICommand Command { get; set; }
    }

    public enum ConfigurationOption
    {
        ElecGasWater,
        ElecGas,
        ElecWater,
        Custom
    }
}
