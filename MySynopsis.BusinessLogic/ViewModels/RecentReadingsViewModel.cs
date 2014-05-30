using MySynopsis.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class RecentReadingsViewModel : ViewModelBase
    {
        public RecentReadingsViewModel()
        {
            WithinAMonth = new ObservableCollection<DataReading>();
            WithinAWeek = new ObservableCollection<DataReading>();
            WithinSixMonths = new ObservableCollection<DataReading>();
            WithinAYear = new ObservableCollection<DataReading>();
        }

        private User _user;
        private UsageViewModel _usage;

        public User User
        {
            get { return _user; }
            set
            {
                if (_user == value)
                {
                    return;
                }
                _user = value;
                UpdateRecentReadings();
            }
        }

        public ObservableCollection<DataReading> WithinAWeek { get; private set; }
        public ObservableCollection<DataReading> WithinAMonth { get; private set; }
        public ObservableCollection<DataReading> WithinSixMonths { get; private set; }
        public ObservableCollection<DataReading> WithinAYear { get; private set; }

        public UsageViewModel Usage
        {
            get
            {
                return _usage;
            }
            set
            {
                if (_usage == value)
                {
                    return;
                }
                _usage = value;
                NotifyPropertyChanged();
            }
        }

        private void UpdateRecentReadings()
        {
            throw new NotImplementedException();
        }
    }
}
