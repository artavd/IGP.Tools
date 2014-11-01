namespace SBL.Common.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static void Foreach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            Contract.ArgumentIsNotNull(sequence, () => sequence);
            Contract.ArgumentIsNotNull(action, () => action);

            foreach (var element in sequence)
            {
                action(element);
            }
        }
    }
}