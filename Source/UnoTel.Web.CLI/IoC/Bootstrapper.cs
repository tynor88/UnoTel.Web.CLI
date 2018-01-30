using SimpleInjector;
using System;
using System.Threading;

namespace UnoTel.Web.Cli.IoC
{
    internal static class Bootstrapper
    {
        private static readonly Lazy<Container> _container = new Lazy<Container>(InitializeContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        internal static Container Container => _container.Value;

        private static Container InitializeContainer()
        {
            Container container = new Container();

            container.RegisterPackages(AppDomain.CurrentDomain.GetAssemblies());

            container.Verify();

            return container;
        }
    }
}
