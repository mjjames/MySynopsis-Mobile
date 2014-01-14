using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.QuickUI;

namespace MySynopsis.UI.ValueConverters
{
    public class LongConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            if (value is long) 
                return ((long)value).ToString();

            //** Is it ok to put Exception here or should I return "something" here? **
            throw new Exception("Can't convert from " + value.GetType().Name + ". Expected type if float.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long longValue;
            long.TryParse(value.ToString(), out longValue);
            return longValue;
        }
    }
}
