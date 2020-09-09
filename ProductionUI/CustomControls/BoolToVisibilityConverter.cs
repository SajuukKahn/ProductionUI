using System;
using System.Windows;
using System.Windows.Data;

namespace ProductionUI.CustomControls
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value.GetType();
            if (type.Name == "Boolean")
            {
                if (value.Equals(false))
                {
                    return Visibility.Collapsed;
                }
                return Visibility.Visible;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
