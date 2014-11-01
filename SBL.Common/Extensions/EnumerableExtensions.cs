namespace SBL.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using SBL.Common.Annotations;

    public static class EnumerableExtensions
    {
        public static void Foreach<T>(
            [NotNull] this IEnumerable<T> sequence,
            [NotNull] Action<T> action)
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