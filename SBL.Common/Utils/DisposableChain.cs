namespace SBL.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using SBL.Common.Annotations;
    using SBL.Common.Extensions;

    public sealed class DisposableChain : IDisposable
    {
        private readonly IList<IDisposable> _chain = new List<IDisposable>();

        private bool _isDisposed = false;

        public T AddToChain<T>([NotNull] T disposableObject) where T : class, IDisposable
        {
            Contract.ArgumentIsNotNull(disposableObject, () => disposableObject);

            if (_isDisposed)
            {
                throw new ObjectDisposedException("Disposable chain");
            }

            _chain.Add(disposableObject);
            return disposableObject;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DisposableChain()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _chain.Foreach(d => d.Dispose());
            }

            _isDisposed = true;
        }
    }
}