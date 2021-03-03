using System;
using System.Globalization;
using System.Windows.Data;
using static SoundLevelCalculator.Controls.NumericUpDown;
using static SoundLevelCalculator.SharedFunctions;

namespace SoundLevelCalculator.Converters
{
    public class ToMilimetersConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertUnits(value[0], (Units)Enum.Parse(typeof(Units), value[1].ToString()), Units.Milimeters).ToString(); 
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
