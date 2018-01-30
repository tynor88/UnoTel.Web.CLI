using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using UnoTel.Web.Api.IoC;
using UnoTel.Web.Api.Validation;

namespace UnoTel.Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<SmsValidator>());

            IntegrateSimpleInjector(services);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            Bootstrapper.Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Bootstrapper.Container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(Bootstrapper.Container));

            services.UseSimpleInjectorAspNetRequestScoping(Bootstrapper.Container);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            Bootstrapper.Container.InitializeContainer(app);
        }
    }
}
