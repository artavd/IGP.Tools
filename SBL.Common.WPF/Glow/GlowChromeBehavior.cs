namespace SBL.Common.WPF.Glow
{
    using System.Windows;
    using System.Windows.Interactivity;

    public sealed class GlowChromeBehavior : Behavior<Window>
    {
        private GlowWindow _glow;

        public static readonly DependencyProperty GlowRadiusProperty =
            GlowWindow.GlowRadiusProperty.AddOwner(typeof (GlowChromeBehavior));

        public int GlowRadius
        {
            get { return (int)GetValue(GlowRadiusProperty); }
            set { SetValue(GlowRadiusProperty, value); }
        }

        public Window OwnerWindow => AssociatedObject;

        // TODO: AA: Add OnDetached / OnChanged after checking on memory leaks
        protected override void OnAttached()
        {
            base.OnAttached();

            _glow = new GlowWindow(this);
        }
    }
}