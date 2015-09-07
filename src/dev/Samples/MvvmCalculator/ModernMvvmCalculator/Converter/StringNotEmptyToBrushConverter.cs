using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MvvmCalculator.Converter
{
    class StringNotEmptyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (String.IsNullOrEmpty((string)value))
                return new SolidColorBrush(Colors.DarkSlateGray);
            else
                return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        // That was the signature from the Windows Forms/WPF Converters. There not working out of the box
        // in Windows Runtime, but then again, migrating them is really not a big deal.

        //public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    if (String.IsNullOrEmpty((string)value))
        //        return System.Drawing.SystemColors.Control;
        //    else
        //        return System.Drawing.Color.Red;
        //}

        //public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
