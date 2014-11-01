namespace IGP.Tools.IO.Module
{
    using IGP.Tools.IO.Implementation;
    using Microsoft.Practices.Unity;

    public class IOExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterType<IPortFactory, WellKnownPortTypesPortFactory>(
                new ContainerControlledLifetimeManager());
        }
    }
}