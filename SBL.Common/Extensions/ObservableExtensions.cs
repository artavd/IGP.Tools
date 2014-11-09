namespace SBL.Common.Extensions
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using SBL.Common.Annotations;

    public static class ObservableExtensions
    {
        [NotNull]
        public static IObservable<T> Pausable<T>(
            [NotNull] this IObservable<T> observable,
            [NotNull] IObservable<bool> switcher)
        {
            Contract.ArgumentIsNotNull(observable, () => observable);
            Contract.ArgumentIsNotNull(switcher, () => switcher);

            return switcher
                .DistinctUntilChanged()
                .Where(on => on)
                .SelectMany(observable.TakeUntil(switcher.Where(on => !on)));
        }

        [NotNull]
        public static IObservable<T> DeferRepeat<T>(
            [NotNull] this Func<IObservable<T>> observableProvider)
        {
            Contract.ArgumentIsNotNull(observableProvider, () => observableProvider);

            return Observable.Defer(observableProvider).Repeat();
        }

        [NotNull]
        public static IObservable<T> DeferRepeat<T>(
            [NotNull] this Func<Task<IObservable<T>>> observableProvider)
        {
            Contract.ArgumentIsNotNull(observableProvider, () => observableProvider);

            return Observable.Defer(observableProvider).Repeat();
        }
    }
}