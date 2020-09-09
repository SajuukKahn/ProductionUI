using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace ProductionUI.CustomControls
{
    class InvertBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value.GetType();
            if (type.Name == "Boolean")
            {
                if (value.Equals(false))
                {
                    return true;
                }
                return false;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
