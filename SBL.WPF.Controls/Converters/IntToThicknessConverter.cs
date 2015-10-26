namespace SBL.WPF.Controls.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using SBL.Common;

    [ValueConversion(typeof(int), typeof(Thickness))]
    public sealed class IntToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.OfType<int>(value);
            Contract.IsTrue(targetType == typeof(Thickness));

            return new Thickness((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}