using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace MvvmCalculator.Converter
{
    class StringNotEmptyToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (String.IsNullOrEmpty((string)value))
                return System.Drawing.SystemColors.Control;
            else
                return System.Drawing.Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ViewWithoutUI
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string myImaginaryLabel;

        public ViewWithoutUI()
        {
            SomeCommand = () =>
                {
                    ImaginaryLabel = "This is the result, world.";
                };
        }

        public Action SomeCommand { get; set; }

        public string ImaginaryLabel
        {
            get
            { return myImaginaryLabel; }
            
            set
            {
                if (!object.Equals(myImaginaryLabel, value))
                { 
                    myImaginaryLabel = value;
                    OnImaginaryLabelChanged(new PropertyChangedEventArgs(nameof(ImaginaryLabel)));
                }
            }
        }

        protected virtual void OnImaginaryLabelChanged(PropertyChangedEventArgs eArgs)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, eArgs);
            }
        }
    }

    public class SomeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
