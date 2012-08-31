using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using ShuppanButsu.Web.MetaweblogApi;

namespace ShuppanButsu.Web.App_Start
{
    public class HandlersInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(
                Component.For<MetaWeblogHandler>().ImplementedBy<MetaWeblogHandler>().LifeStyle.Transient);
        }
    }
}