using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Facilities.Logging;
using Castle.Windsor;
using ShuppanButsu.Infrastructure;
using Castle.MicroKernel.Registration;
using ShuppanButsu.Infrastructure.Concrete;

namespace ShuppanButsu
{
    /// <summary>
    /// This is the bootstrap class, the one used to initialize everything.
    /// </summary>
    public class Bootstrap
    {
        public static Bootstrap Instance { get; private set; }

        WindsorContainer _container;

        static Bootstrap() 
        {
            Instance = new Bootstrap();
        }

        private Bootstrap() 
        {
            _container = new WindsorContainer();
            _container.AddFacility(new LoggingFacility(LoggerImplementation.Log4net, "log4net.config"));
            _container.Register(
                Component.For<IDomainEventHandlerCatalog, ICommandHandlerCatalog>().ImplementedBy<CastleFasterflectHandlerCatalog>(),
                Component.For<ShuppanButsuConfiguration>().ImplementedBy<ShuppanButsuConfiguration>());
        }
    }
}
