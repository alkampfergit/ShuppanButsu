using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Castle.Facilities.Logging;
using Castle.Facilities.Startable;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ShuppanButsu.Domain.Blog;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Infrastructure.Concrete;
using ShuppanButsu.Infrastructure.Concrete.EventsStore;

namespace ShuppanButsu.Web
{
    public class WebSiteBootstrap
    {
        public static WebSiteBootstrap Instance { get; private set; }

        WindsorContainer _container;

        public WindsorContainer Container { get { return _container; } }

        static WebSiteBootstrap()
        {
            //Init the singleton, use the standard container created by bootstrap of shuppanbutsu that does basic
            //configuration, this part adds remaining configuration
            Instance = new WebSiteBootstrap();
            Instance._container = new WindsorContainer();
            Instance.Container.Install(FromAssembly.This());
            Instance.Container.Install(FromAssembly.Containing<Post>());
        }
    }

    public class EverythingInProcessWindsorInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {

            container.AddFacility(new LoggingFacility(LoggerImplementation.Log4net, "log4net.config"));
            container.AddFacility(new StartableFacility());

            container.Register(
                Component.For<IDomainEventHandlerCatalog, ICommandHandlerCatalog>().ImplementedBy<CastleFasterflectHandlerCatalog>(),
                Component.For<ICommandDispatcher>().ImplementedBy<CommandDispatcher>(),
                Component.For<IDomainEventDispatcher>().ImplementedBy<DomainEventDispatcher>(),
                Component.For<UnitOfWork>().ImplementedBy<UnitOfWork>());

            container.Register(
                Component.For<AggregateRootFactory>().UsingFactoryMethod(() => new AggregateRootFactory()));



            container.Register(
                Component.For<IEventsStore>().ImplementedBy<SqlEventsStore>()
                        .DependsOn(Property.ForKey("configurationFileName").Eq(HostingEnvironment.MapPath("~/NhEventStoreConfiguration.xml")))); 
        }
    }
}