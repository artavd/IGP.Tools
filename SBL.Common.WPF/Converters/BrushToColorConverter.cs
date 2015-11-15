namespace SBL.Common.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using SBL.Common;

    [ValueConversion(typeof(Brush), typeof(Color))]
    public sealed class BrushToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = value as SolidColorBrush;
            Contract.IsNotNull(brush);
            Contract.IsTrue(targetType == typeof(Color));

            return brush.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.OfType<Color>(value);
            Contract.IsTrue(typeof(Brush).IsAssignableFrom(targetType));

            return new SolidColorBrush((Color)value);
        }
    }
}