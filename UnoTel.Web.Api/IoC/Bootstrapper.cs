using Microsoft.AspNetCore.Builder;
using SimpleInjector;
using System;
using System.Threading;
using UnoTel.Web.Core.Services;

namespace UnoTel.Web.Api.IoC
{
    internal static class Bootstrapper
    {
        private static readonly Lazy<Container> _container = new Lazy<Container>(SetupContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        internal static Container Container => _container.Value;

        private static Container SetupContainer()
        {
            Container container = new Container();

            return container;
        }

        internal static void InitializeContainer(this Container container, IApplicationBuilder app)
        {
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            container.RegisterPackages(new[] { typeof(IUnoTelService).Assembly });

            container.Verify();
        }
    }
}
