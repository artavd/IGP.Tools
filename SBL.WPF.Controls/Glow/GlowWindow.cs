namespace SBL.WPF.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Interop;
    using System.Windows.Media;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.WPF.Controls.Win32;

    internal sealed class GlowWindow : Window
    {
        private readonly Window _owner;
        private readonly GlowWindowHandler _handler;

        private bool _isClosed = false;
        private IntPtr _handle;
        private IntPtr _ownerHandle;

        static GlowWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GlowWindow), new FrameworkPropertyMetadata(typeof(GlowWindow)));
        }

        public static readonly DependencyProperty IsGlowingProperty =
            DependencyProperty.Register(nameof(IsGlowing), typeof(bool), typeof(GlowWindow), new UIPropertyMetadata(true));

        public static readonly DependencyProperty ActiveGlowBrushProperty =
            DependencyProperty.Register(nameof(ActiveGlowBrush), typeof(Brush), typeof(GlowWindow), new UIPropertyMetadata(null));

        public static readonly DependencyProperty InactiveGlowBrushProperty =
            DependencyProperty.Register(nameof(InactiveGlowBrush), typeof(Brush), typeof(GlowWindow), new UIPropertyMetadata(null));

        public GlowWindow([NotNull] GlowChromeBehavior behavior, [NotNull] GlowWindowHandler handler)
        {
            Contract.ArgumentIsNotNull(behavior, () => behavior);
            Contract.ArgumentIsNotNull(handler, () => handler);

            _owner = behavior.OwnerWindow;
            _handler = handler;

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Visibility = Visibility.Collapsed;
            ResizeMode = ResizeMode.NoResize;
            ShowActivated = false;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.Manual;

            SetBinding(
                ActiveGlowBrushProperty,
                new Binding(nameof(behavior.ActiveGlowBrush)) { Source = behavior, Mode = BindingMode.OneWay});

            SetBinding(
                InactiveGlowBrushProperty,
                new Binding(nameof(behavior.InactiveGlowBrush)) { Source = behavior, Mode = BindingMode.OneWay });

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

        public bool IsGlowing
        {
            get { return (bool)GetValue(IsGlowingProperty); }
            set { SetValue(IsGlowingProperty, value); }
        }

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

        public void Update()
        {
            if (_isClosed) return;

            if (_ownerHandle == IntPtr.Zero)
            {
                _ownerHandle = new WindowInteropHelper(_owner).Handle;
            }

            IsGlowing = _owner.IsActive;

            int left = (int)Math.Round(_owner.Left - 8);
            int top = (int)Math.Round(_owner.Top - 8);
            int width = (int)Math.Round(_owner.Width + 16);
            int height = (int)Math.Round(_owner.Height + 16);

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
