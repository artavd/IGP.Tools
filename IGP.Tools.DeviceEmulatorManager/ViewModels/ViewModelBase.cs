namespace IGP.Tools.DeviceEmulatorManager.ViewModels
{
    using System;
    using System.Reactive.Disposables;
    using Prism.Mvvm;

    internal abstract class ViewModelBase : BindableBase, IDisposable
    {
        private CompositeDisposable _anchors = new CompositeDisposable();

        protected void RegisterForDisposing(IDisposable disposable)
        {
            _anchors.Add(disposable);
        }

        protected void RegisterEventSubscription(Action subscribe, Action unsubscribe)
        {
            _anchors.Add(SubscribeToEvent(subscribe, unsubscribe));
        }

        protected IDisposable SubscribeToEvent(Action subscribe, Action unsubscribe)
        {
            subscribe();
            return Disposable.Create(unsubscribe);
        }

        public virtual void Dispose()
        {
            _anchors.Dispose();
        }
    }
}