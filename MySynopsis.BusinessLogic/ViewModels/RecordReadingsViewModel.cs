
using MySynopsis.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RecordReadingsViewModel : ViewModelBase
    {
        private User _user;
        private DelegateCommand _recordReadings;
        private bool _isPersisting;
        private IDataReadingService _dataReadingService;
        private bool _isFaulted;
        public RecordReadingsViewModel(User user, IDataReadingService dataReadingService)
        {
            _user = user;
            MeterReadings = new ObservableCollection<DataReadingViewModel>(_user.MeterConfiguration.Select(m => new DataReadingViewModel(m, _user.Id)));
            MeterReadings.CollectionChanged += (sender, args) =>
            {
                foreach (var newItem in args.NewItems.Cast<DataReadingViewModel>())
                {
                    ReadingPropertyChanged(newItem);
                }
            };
            foreach (var reading in MeterReadings)
            {
                ReadingPropertyChanged(reading);
            }
            _dataReadingService = dataReadingService;
        }

        private void ReadingPropertyChanged(DataReadingViewModel reading)
        {
            reading.PropertyChanged += (obj, e) =>
            {
                if (e.PropertyName == "Reading")
                {
                    ((DelegateCommand)RecordReadings).RaiseCanExecuteChanged();
                }
            };
        }

        public ObservableCollection<DataReadingViewModel> MeterReadings { get; private set; }

        public ICommand RecordReadings
        {
            get
            {
                if (_recordReadings == null)
                {
                    _recordReadings = new DelegateCommand(async obj => await PersistReadings(), (obj) =>
                        {
                            return !IsPersisting && MeterReadings.All(m => m.Reading > 0);
                        });
                }
                return _recordReadings;
            }
        }

        public Action PostPersistAction { get; set; }

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
                ((DelegateCommand)_recordReadings).RaiseCanExecuteChanged();
            }
        }

        public bool IsFaulted
        {
            get
            {
                return _isFaulted;
            }
            set
            {
                if (_isFaulted == value)
                {
                    return;
                }
                _isFaulted = value;
                NotifyPropertyChanged();
            }
        }
        private async Task PersistReadings()
        {
            IsPersisting = true;
            IsFaulted = false;
            try
            {
                await _dataReadingService.PersistReadings(MeterReadings.Select(m => m.ToDataReading()));
                if (PostPersistAction != null)
                {
                    PostPersistAction();
                }
                ClearReadingValues();
            }
            catch (Exception)
            {
                IsFaulted = true;
            }
            IsPersisting = false;

        }

        private void ClearReadingValues()
        {
            foreach (var reading in MeterReadings)
            {
                reading.Reading = 0;
            }
        }
    }
}
