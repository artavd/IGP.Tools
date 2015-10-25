namespace SBL.WPF.Controls
{
    using System.Windows;

    internal sealed class GlowWindow : Window
    {
        static GlowWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(typeof(GlowWindow)));
        }
    }
}
