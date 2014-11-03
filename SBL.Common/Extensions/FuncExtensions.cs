namespace SBL.Common.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SBL.Common.Annotations;

    public static class FuncExtensions
    {
        public static async Task<TResult> StartInTask<TResult>([NotNull] this Func<TResult> func)
        {
            Contract.ArgumentIsNotNull(func, () => func);

            return await Task.Factory.StartNew(func);
        }

        public static async Task<TResult> StartInTask<TResult>([NotNull] this Func<TResult> func, CancellationToken token)
        {
            Contract.ArgumentIsNotNull(func, () => func);

            return await Task.Factory.StartNew(func, token);
        }

        public static async Task<TResult> StartInTask<TResult>([NotNull] this Func<object, TResult> func, object parameter, CancellationToken token)
        {
            Contract.ArgumentIsNotNull(func, () => func);

            return await Task.Factory.StartNew(func, parameter, token);
        }
    }
}