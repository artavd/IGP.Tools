namespace SBL.WPF.Controls.Glow
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
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

            _handle.SetWindowPos(_ownerHandle, left, top, width, height, SetWindowPosParam.SWP_NOACTIVATE);
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
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.RemoveHook(WndProc);
            _isClosed = true;
        }

        private void SetWindowStyle()
        {
            // To remove from Alt + Tab selection
            var wstyle = _handle.GetWindowLongExStyle();
            wstyle |= WindowStylesEx.WSEX_TOOLWINDOW;
            _handle.SetWindowLongExStyle(wstyle);

            // To proceed double-clicks in WndProc
            var cstyle = _handle.GetClassLong(SetClassLongParam.GCL_STYLE);
            cstyle |= ClassStyles.CS_DBLCLKS;
            _handle.SetClassLong(SetClassLongParam.GCL_STYLE, cstyle);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == (int)WindowsMessages.WM_MOUSEACTIVATE)
            {
                // MA_NOACTIVATE - to not activate glow window and pass message to owner window
                handled = true;
                return new IntPtr(3);
            }

            if (msg == (int)WindowsMessages.WM_LBUTTONDOWN)
            {
                _owner.Activate();

                // pass to non-client area handler
                _ownerHandle.PostMessage(WindowsMessages.WM_NCLBUTTONDOWN, (IntPtr)GetHitTest(lparam.ToPoint()), IntPtr.Zero);
                handled = true;
            }

            if (msg == (int)WindowsMessages.WM_LBUTTONDBLCLK)
            {
                HitTest hitTest = GetHitTest(lparam.ToPoint());
                if (hitTest == HitTest.HTTOP || hitTest == HitTest.HTBOTTOM)
                {
                    _ownerHandle.SendMessage(WindowsMessages.WM_NCLBUTTONDBLCLK, (IntPtr)hitTest, IntPtr.Zero);
                }

                handled = true;
            }

            if (msg == (int)WindowsMessages.WM_NCHITTEST)
            {
                if (_owner.ResizeMode != ResizeMode.CanResize)
                {
                    return IntPtr.Zero;
                }

                var screenPoint = lparam.ToPoint();
                var clientPoint = PointFromScreen(screenPoint);

                SetCursor(clientPoint);
            }

            return IntPtr.Zero;
        }

        private HitTest GetHitTest(Point point)
        {
            int radius = GlowRadius;
            var hitTestResults = new[]
            {
                new { Result = HitTest.HTLEFT, Area = new Rect(0, radius, radius, ActualHeight - 2 * radius) },
                new { Result = HitTest.HTTOPLEFT, Area = new Rect(0, 0, radius, radius) },
                new { Result = HitTest.HTTOP, Area = new Rect(radius, 0, ActualWidth - 2 * radius, radius) },
                new { Result = HitTest.HTTOPRIGHT, Area = new Rect(ActualWidth - radius, 0, radius, radius) },
                new { Result = HitTest.HTRIGHT, Area = new Rect(ActualWidth - radius, radius, radius, ActualHeight - 2 * radius) },
                new { Result = HitTest.HTBOTTOMRIGHT, Area = new Rect(ActualWidth - radius, ActualHeight - radius, radius, radius) },
                new { Result = HitTest.HTBOTTOM, Area = new Rect(radius, ActualHeight - radius, ActualWidth - 2 * radius, radius) },
                new { Result = HitTest.HTBOTTOMLEFT, Area = new Rect(0, ActualHeight - radius, radius, radius) }
            };

            return hitTestResults.FirstOrDefault(x => x.Area.Contains(point))?.Result ?? HitTest.HTTRANSPARENT;
        }

        private void SetCursor(Point point)
        {
            switch (GetHitTest(point))
            {
                case HitTest.HTTOPLEFT:
                case HitTest.HTBOTTOMRIGHT:
                    Cursor = Cursors.SizeNWSE;
                    break;

                case HitTest.HTTOPRIGHT:
                case HitTest.HTBOTTOMLEFT:
                    Cursor = Cursors.SizeNESW;
                    break;

                case HitTest.HTTOP:
                case HitTest.HTBOTTOM:
                    Cursor = Cursors.SizeNS;
                    break;

                case HitTest.HTLEFT:
                case HitTest.HTRIGHT:
                    Cursor = Cursors.SizeWE;
                    break;
            }
        }
    }
}
