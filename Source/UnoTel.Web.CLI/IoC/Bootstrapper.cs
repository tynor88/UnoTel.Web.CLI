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

            container.Register<ExecutionService>(Lifestyle.Singleton);
            container.Register<LoginService>(Lifestyle.Singleton);
            container.Register<SendSmsService>(Lifestyle.Singleton);
            container.Register<CookieProvider>(Lifestyle.Singleton);

            container.Verify();

            return container;
        }
    }
}
