using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.ViewModels
{
    public class UsageViewModel : ViewModelBase
    {
        private long _sincePreviousReading;
        public long SincePreviousReading {
            get { return _sincePreviousReading; } 
            set {
                if (value == _sincePreviousReading)
                {
                    return;
                }
                _sincePreviousReading = value;
                NotifyPropertyChanged();
            } 
        }
    }
}
