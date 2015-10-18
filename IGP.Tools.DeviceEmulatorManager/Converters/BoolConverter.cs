namespace IGP.Tools.DeviceEmulatorManager.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public abstract class BoolConverter<T> : IValueConverter
    {
        protected BoolConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        private T True { get; }
        private T False { get; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }

    public sealed class BoolToVisibilityConverter : BoolConverter<Visibility>
    {
        public BoolToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    }

    public sealed class InvertedBoolToVisibilityConverter : BoolConverter<Visibility>
    {
        public InvertedBoolToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
    }
}