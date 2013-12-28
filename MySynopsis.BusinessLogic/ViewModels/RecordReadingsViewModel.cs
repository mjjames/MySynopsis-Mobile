
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
        public RecordReadingsViewModel(User user, IDataReadingService dataReadingService)
        {
            _user = user;
            MeterReadings = new ObservableCollection<DataReadingViewModel>(_user.MeterConfiguration.Select(m => new DataReadingViewModel(m, _user.Id)));
            _dataReadingService = dataReadingService;
        }

        public ObservableCollection<DataReadingViewModel> MeterReadings { get; private set; }

        public ICommand RecordReadings
        {
            get
            {
                if (_recordReadings == null)
                {
                    _recordReadings = new DelegateCommand(async obj => await PersistReadings(), (obj) => MeterReadings.All(m => m.Reading > 0));
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
            }
        }

        private async Task PersistReadings()
        {
            IsPersisting = true;
            await _dataReadingService.PersistReadings(MeterReadings.Select(m => m.ToDataReading()));
            IsPersisting = false;
            if (PostPersistAction != null)
            {
                PostPersistAction();
            }
        }
    }
}
