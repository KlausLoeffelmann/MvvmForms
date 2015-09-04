using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MvvmCalculator.Converter
{
    public class NullableDoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if (value == null)
                return null;

            return double.Parse((string) value);
        }
    }
}
