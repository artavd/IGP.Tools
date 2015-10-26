namespace SBL.WPF.Controls.Glow
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.WPF.Controls.Win32;

    internal sealed class GlowWindow : Window
    {
        private readonly Window _owner;

        private bool _isClosed = false;
        private IntPtr _handle;
        private IntPtr _ownerHandle;

        static GlowWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(typeof(GlowWindow)));
        }

        public static readonly DependencyProperty GlowRadiusProperty =
            DependencyProperty.Register(nameof(GlowRadius), typeof(int), typeof(GlowWindow), new UIPropertyMetadata(8));

        public GlowWindow([NotNull] GlowChromeBehavior behavior)
        {
            Contract.ArgumentIsNotNull(behavior, () => behavior);

            _owner = behavior.OwnerWindow;

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            ResizeMode = ResizeMode.NoResize;
            ShowActivated = false;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.Manual;

            SetBinding(
                GlowRadiusProperty,
                new Binding(nameof(behavior.GlowRadius)) { Source = behavior, Mode = BindingMode.OneWay });

            SetBinding(
                BorderBrushProperty,
                new Binding(nameof(_owner.BorderBrush)) { Source = _owner, Mode = BindingMode.OneWay });

            SetBinding(
                BorderThicknessProperty,
                new Binding(nameof(_owner.BorderThickness)) { Source = _owner, Mode = BindingMode.OneWay });

            _owner.ContentRendered += (s, e) =>
            {
                if (_isClosed) return;
                Show();
                Update();
            };

            _owner.StateChanged += (s, e) => Update();
            _owner.LocationChanged += (s, e) => Update();
            _owner.SizeChanged += (s, e) => Update();
            _owner.Activated += (s, e) => Update();
            _owner.Deactivated += (s, e) => Update();
            _owner.Closed += (s, e) => Close();
        }

        public int GlowRadius
        {
            get { return (int)GetValue(GlowRadiusProperty); }
            set { SetValue(GlowRadiusProperty, value); }
        }

        public void Update()
        {
            if (_isClosed) return;

            if (_ownerHandle == IntPtr.Zero)
            {
                _ownerHandle = new WindowInteropHelper(_owner).Handle;
            }

            int radius = GlowRadius;
            int left = (int)Math.Round(_owner.Left - radius);
            int top = (int)Math.Round(_owner.Top - radius);
            int width = (int)Math.Round(_owner.Width + radius * 2);
            int height = (int)Math.Round(_owner.Height + radius * 2);

            WinAPI.SetWindowPos(_handle, _ownerHandle, left, top, width, height, SetWindowPosParam.SWP_NOACTIVATE);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = PresentationSource.FromVisual(this) as HwndSource;
            if (source != null)
            {
                _handle = source.Handle;
                SetWindowStyle();
                source.AddHook(WndProc);
            }

            Owner = _owner;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _isClosed = true;
        }

        private void SetWindowStyle()
        {
            // To remove from Alt + Tab selection
            WindowStylesEx style = _handle.GetWindowLongExStyle();
            style |= WindowStylesEx.WSEX_TOOLWINDOW;
            _handle.SetWindowLongExStyle(style);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            return IntPtr.Zero;
        }
    }
}
