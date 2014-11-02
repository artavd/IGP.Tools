namespace SBL.Common.Extensions
{
    using System;
    using System.Reactive.Linq;
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
    }
}