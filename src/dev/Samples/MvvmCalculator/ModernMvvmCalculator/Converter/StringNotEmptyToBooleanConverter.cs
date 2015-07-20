using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace MvvmCalculator.Converter
{
    class StringNotEmptyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !String.IsNullOrEmpty((string)value);
        }

        // That is the signature from the Windows Forms/WPF Converters. There not working out of the box
        // in Windows Runtime, but then again, migrating them is really not a big deal.

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    return !String.IsNullOrEmpty((string)value);
        //}
    }
}
