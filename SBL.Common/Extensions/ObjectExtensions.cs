namespace SBL.Common.Extensions
{
    using System;
    using SBL.Common.Annotations;

    public static class ObjectExtensions
    {
        public static TResult Eval<T, TResult>(
            [CanBeNull] this T obj,
            [NotNull] Func<T, TResult> func,
            [CanBeNull] Func<TResult> defaultProvider = null) where T : class
        {
            Contract.ArgumentIsNotNull(func, () => func);

            TResult result = default(TResult);
            if (obj != null)
            {
                result = func(obj);
            }
            else if (defaultProvider != null)
            {
                result = defaultProvider();
            }

            return result;
        }

        public static TResult As<TResult>([CanBeNull] this object obj)
        {
            return (TResult)obj;
        }

        public static void DisposeIfPossible([NotNull] this object obj)
        {
            if (obj is IDisposable)
            {
                obj.As<IDisposable>().Dispose();
            }
        }
    }
}