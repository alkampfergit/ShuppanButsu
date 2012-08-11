using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace ShuppanButsu.Infrastructure.Concrete
{
    /// <summary>
    /// This is the class that executes a command dispatching to the real class
    /// </summary>
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private IDomainEventHandlerCatalog _catalog;
        private ILogger _logger;

        public DomainEventDispatcher(IDomainEventHandlerCatalog catalog, ILogger logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        public void DispatchEvent(DomainEvent @event)
        {
            var executors = _catalog.GetAllHandlerFor(@event.GetType());
      
            foreach (var executor in executors)
            {
                try
                {
                    executor.Invoke(@event);
                }
                catch (OutOfMemoryException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error during dispatching event " + @event.GetType() + " to the handler " + executor.DefiningType + "." + executor.Invoker.Method.Name, ex);
                }
            }
           
        }
    }
}
