namespace SBL.WPF.Controls.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using SBL.Common;

    [ValueConversion(typeof(Thickness), typeof(Thickness), ParameterType = typeof(double))]
    public sealed class BorderThicknessMultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.OfType<Thickness>(value);
            Contract.IsTrue(targetType == typeof(Thickness));

            return Multiply((Thickness)value, System.Convert.ToDouble(parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Contract.OfType<Thickness>(value);
            Contract.IsTrue(targetType == typeof(Thickness));

            return Multiply((Thickness)value, 1 / System.Convert.ToDouble(parameter));
        }

        private Thickness Multiply(Thickness thickness, double multiplier)
        {
            return new Thickness(
                thickness.Left * multiplier,
                thickness.Top * multiplier,
                thickness.Right * multiplier,
                thickness.Bottom * multiplier);
        }
    }
}