namespace SBL.Common.Extensions
{
    using System;
    using System.Threading.Tasks;
    using SBL.Common.Annotations;

    public static class FuncExtensions
    {
        [NotNull]
        public static Task<TResult> ToTask<TResult>([NotNull] this Func<TResult> func)
        {
            Contract.ArgumentIsNotNull(func, () => func);

            return Task<TResult>.Factory.FromAsync(func.BeginInvoke, func.EndInvoke, null);
        }

        [NotNull]
        public static Task<TResult> ToTask<T, TResult>([NotNull] this Func<T, TResult> func, T param)
        {
            Contract.ArgumentIsNotNull(func, () => func);

            return Task<TResult>.Factory.FromAsync(func.BeginInvoke, func.EndInvoke, param, null);
        }
    }
}