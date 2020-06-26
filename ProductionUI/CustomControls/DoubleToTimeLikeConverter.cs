using Accessibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace ProductionUI.CustomControls
{
    class DoubleToTimeLikeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var type = value.GetType();
            if (type.Name == "Double")
            {
                Double.TryParse(value.ToString(), out double seconds);
                object timeDisplay = "";
                int minutes = (int)(seconds / 60);
                int hours = minutes / 60;
                int days = hours / 24;
                seconds %= 60;

                if (days > 0)
                {
                    timeDisplay += string.Format("{00:N}:", days); //days.ToString() + ":";
                }
                if(hours > 0)
                {
                    timeDisplay += string.Format("{00:N}:", hours); //hours.ToString() + ":";
                }
                if(minutes > 0)
                {
                    timeDisplay += string.Format("{00:N}:", minutes); //minutes.ToString() + ":";
                }
                timeDisplay += string.Format("{00:N2}", Math.Round(seconds, 2)); //Math.Round(seconds,2).ToString();

                return timeDisplay;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
