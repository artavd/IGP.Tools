namespace SBL.WPF.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    internal sealed class GlowWindow : Window
    {
        private readonly Window _owner;
        private readonly GlowWindowHandler _handler;

        private bool _isClosed = false;

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
                VisibilityProperty,
                new Binding(nameof(_owner.Visibility)) { Source = _owner, Mode = BindingMode.OneWay });

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

            IsGlowing = _owner.IsActive;

            Left = _owner.Left - 4;
            Top = _owner.Top - 4;
            Width = _owner.Width + 8;
            Height = _owner.Height + 8;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _isClosed = true;
        }
    }
}
