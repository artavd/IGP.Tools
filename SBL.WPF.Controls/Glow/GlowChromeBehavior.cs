namespace SBL.WPF.Controls
{
    using System.Windows;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    public sealed class GlowChromeBehavior : Behavior<Window>
    {
        private GlowWindow _glow;

        public static readonly DependencyProperty ActiveGlowBrushProperty =
            GlowWindow.ActiveGlowBrushProperty.AddOwner(typeof(GlowChromeBehavior));

        public static readonly DependencyProperty InactiveGlowBrushProperty =
            GlowWindow.InactiveGlowBrushProperty.AddOwner(typeof (GlowChromeBehavior));

        public Brush ActiveGlowBrush
        {
            get { return (Brush)GetValue(ActiveGlowBrushProperty); }
            set { SetValue(ActiveGlowBrushProperty, value); }
        }

        public Brush InactiveGlowBrush
        {
            get { return (Brush)GetValue(InactiveGlowBrushProperty); }
            set { SetValue(InactiveGlowBrushProperty, value); }
        }

        public Window OwnerWindow => AssociatedObject;

        // TODO: AA: Add OnDetached / OnChanged after checking on memory leaks
        protected override void OnAttached()
        {
            base.OnAttached();

            _glow = new GlowWindow(this, new GlowWindowHandler());
        }
    }
}