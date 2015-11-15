namespace IGP.Tools.DeviceEmulatorManager.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
    using SBL.Common;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public interface IThemeService
    {
        void Initialize(
            [NotNull] Application application,
            [NotNull] ResourceDictionary theme,
            [NotNull] ResourceDictionary accents);

        void ChangeTheme([NotNull] ResourceDictionary theme);

        void ChangeAccents([NotNull] ResourceDictionary accents);
    }

    // TODO: rework to service?
    public static class WellKnownThemes
    {
        public static readonly ResourceDictionary DarkTheme = CreateTheme("Dark");
        public static readonly ResourceDictionary LightTheme = CreateTheme("Light");

        public static readonly ResourceDictionary BlueAccents = CreateTheme("Blue", true);
        public static readonly ResourceDictionary OrangeAccents = CreateTheme("Orange", true);
        public static readonly ResourceDictionary PurpleAccents = CreateTheme("Purple", true);

        private static readonly ConcurrentDictionary<string, ResourceDictionary> Themes = new ConcurrentDictionary<string, ResourceDictionary>();
        private static readonly ConcurrentDictionary<string, ResourceDictionary> Accents = new ConcurrentDictionary<string, ResourceDictionary>();

        static WellKnownThemes()
        {
            RegisterTheme("Dark", DarkTheme);
            RegisterTheme("Light", LightTheme);

            RegisterAccents("Blue", BlueAccents);
            RegisterAccents("Orange", OrangeAccents);
            RegisterAccents("Purple", PurpleAccents);
        }

        [NotNull]
        public static ResourceDictionary GetTheme(string theme)
        {
            ResourceDictionary result = null;
            if (!Themes.TryGetValue(theme, out result))
            {
                throw new ArgumentOutOfRangeException($"Theme '{theme}' is not registered as well known.");
            }

            return result;
        }

        [NotNull]
        public static ResourceDictionary GetAccents(string accents)
        {
            ResourceDictionary result = null;
            if (!Accents.TryGetValue(accents, out result))
            {
                throw new ArgumentOutOfRangeException($"Accents '{accents}' are not registered as well known.");
            }

            return result;
        }

        public static void RegisterTheme([NotNull] string theme, [NotNull] ResourceDictionary resource)
        {
            Contract.ArgumentIsNotNull(theme, () => theme);
            Contract.ArgumentIsNotNull(resource, () => resource);

            if (!Themes.TryAdd(theme, resource))
            {
                throw new InvalidOperationException($"Theme '{theme}' is already registered as well known.");
            }
        }

        public static void RegisterAccents([NotNull] string accents, [NotNull] ResourceDictionary resource)
        {
            Contract.ArgumentIsNotNull(accents, () => accents);
            Contract.ArgumentIsNotNull(resource, () => resource);

            if (!Accents.TryAdd(accents, resource))
            {
                throw new InvalidOperationException($"Accents '{accents}' are already registered as well known.");
            }
        }

        private static ResourceDictionary CreateTheme(string name, bool isAccent = false)
        {
            string uri = isAccent ?
                $@"pack://application:,,,/SBL.Common.WPF;component/Themes/Accents/{name}.xaml" :
                $@"pack://application:,,,/SBL.Common.WPF;component/Themes/{name}.xaml";

            return new ResourceDictionary { Source = new Uri(uri, UriKind.Absolute) };
        }
    }

    internal sealed class ThemeService : IThemeService
    {
        private Dispatcher _uiDispatcher;

        private ResourceDictionary _theme;
        private ResourceDictionary _accents;

        private bool _isInitialized = false;

        public void Initialize(Application application, ResourceDictionary theme, ResourceDictionary accents)
        {
            Contract.ArgumentIsNotNull(application, () => application);
            Contract.ArgumentIsNotNull(theme, () => theme);
            Contract.ArgumentIsNotNull(accents, () => accents);

            if (_isInitialized)
            {
                throw new InvalidOperationException("Theme service is already initialized.");
            }

            _uiDispatcher = application.Dispatcher;

            InitializeResourceDictionary(application, theme, ref _theme);
            InitializeResourceDictionary(application, accents, ref _accents);

            _isInitialized = true;
        }

        public void ChangeTheme(ResourceDictionary theme)
        {
            _uiDispatcher.Invoke(() => { OverrideResourceDictionary(_theme, theme); });
        }

        public void ChangeAccents(ResourceDictionary accents)
        {
            _uiDispatcher.Invoke(() => { OverrideResourceDictionary(_accents, accents); });
        }

        private static void InitializeResourceDictionary(
            Application application,
            ResourceDictionary dictionary,
            ref ResourceDictionary field)
        {
            Contract.IsTrue(field == null, () => "Non-empty resource field initialization prohibited.");

            var appResource = new ResourceDictionary();
            dictionary.Keys.OfType<string>().Foreach(key => appResource.Add(key, dictionary[key]));
            application.Resources.MergedDictionaries.Add(appResource);

            field = appResource;
        }

        private static void OverrideResourceDictionary(ResourceDictionary dictionary, ResourceDictionary overrider)
        {
            dictionary.Keys.OfType<string>()
                .Where(overrider.Contains)
                .Foreach(key => dictionary[key] = overrider[key]);
        }
    }
}