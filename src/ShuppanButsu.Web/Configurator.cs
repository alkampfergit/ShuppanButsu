using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Castle.MicroKernel.Registration;

namespace ShuppanButsu.Web
{
    public class Configurator : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            ShuppanButsuConfiguration configuration = CreateConfiguration();
            container.Register(
                Component.For<ShuppanButsuConfiguration>().UsingFactoryMethod(() => configuration));
        }

        public ShuppanButsuConfiguration CreateConfiguration() 
        {
            String templateDirectory = ConfigurationManager.AppSettings["templatedir"] ?? HostingEnvironment.MapPath( "~/Templates");
            var configuration = new ShuppanButsuConfiguration(templateDirectory);
            return configuration;
        }
    }
}