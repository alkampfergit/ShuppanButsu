using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Core.Logging;

namespace ShuppanButsu.Infrastructure.Concrete
{
    /// <summary>
    /// This is the class that executes a command dispatching to the real class
    /// </summary>
    public class DomainEventDispatcher : IDomainEventDispatcher, IStartable
    {
        private IDomainEventHandlerCatalog _catalog;
        private ILogger _logger;

        private static Thread _executingThread;

        public DomainEventDispatcher(IDomainEventHandlerCatalog catalog, ILogger logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        #region syncronization 

        private static BlockingCollection<DomainEvent> _dispatchQueue = new BlockingCollection<DomainEvent>();

        void IStartable.Start()
        {
            if (_executingThread == null)
            {
                _executingThread = new Thread(DispatchEventCore);
                _executingThread.Start();
            }
        }

        public void Stop()
        {
            //TODO: Create a good stop routine
        }

        private void DispatchEventCore(object obj)
        {
            foreach (DomainEvent @event in _dispatchQueue.GetConsumingEnumerable())
            {
                InnerDispatch(@event);
            }
        }

        #endregion

        public void DispatchEvent(DomainEvent @event)
        {
            _dispatchQueue.Add(@event);
        }

        private void InnerDispatch(DomainEvent @event)
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
