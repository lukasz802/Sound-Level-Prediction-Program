using System;
using System.Globalization;
using System.Windows.Data;

namespace SoundLevelCalculator.Converters
{
    public class IntToFrequencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "0":
                    return "63";
                case "1":
                    return "125";
                case "2":
                    return "250";
                case "3":
                    return "500";
                case "4":
                    return "1k";
                case "5":
                    return "2k";
                case "6":
                    return "4k";
                case "7":
                    return "8k";
                default: return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}