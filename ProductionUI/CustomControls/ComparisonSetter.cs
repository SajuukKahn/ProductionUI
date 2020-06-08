using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace ProductionUI.CustomControls
{
    public class ComparisonSetter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if(!string.IsNullOrEmpty(parameterString))
            {
                string[] parameters = parameterString.Split('|');


            }


            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
