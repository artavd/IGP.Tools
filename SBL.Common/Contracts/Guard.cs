namespace SBL.Common.Contracts
{
    using System;
    using System.Linq.Expressions;

    public static class Guard
    {
        public static void ArgumentIsNotNull<T>(T value, Expression<Action<T>> ex) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
