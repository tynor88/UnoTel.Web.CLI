using SimpleInjector;
using SimpleInjector.Packaging;
using UnoTel.Web.Core.Services;

namespace UnoTel.Web.Core.IoC
{
    public class Bootstrapper : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register<IUnoTelService, UnoTelService>(Lifestyle.Singleton);
            container.Register<LoginService>(Lifestyle.Singleton);
            container.Register<SendSmsService>(Lifestyle.Singleton);
            container.Register<CookieProvider>(Lifestyle.Singleton);
        }
    }
}
