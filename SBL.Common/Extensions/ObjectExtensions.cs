namespace SBL.Common.Extensions
{
    using System;

    public static class ObjectExtensions
    {
        public static TResult Eval<T, TResult>(
            this T obj,
            Func<T, TResult> func,
            Func<TResult> defaultProvider = null) where T : class
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
    }
}