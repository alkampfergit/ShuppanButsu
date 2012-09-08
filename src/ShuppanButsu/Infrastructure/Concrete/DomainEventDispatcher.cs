using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core;
using Castle.Core.Logging;
using ShuppanButsu.Utils;

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
                _logger.SetOpType("event", @event.GetType().FullName + " timestamp:" + @event.Timestamp);
                try
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatching:" + @event.GetType().FullName);
                    InnerDispatch(@event);
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatched:" + @event.GetType().FullName);
                }
                finally
                {
                    _logger.RemoveOpType();
                }
                
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
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatching event " + @event.GetType() + " to the handler " + executor.DefiningType + "." + executor.Invoker.Method.Name);
                    executor.Invoke(@event);
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatched event " + @event.GetType() + " to the handler " + executor.DefiningType + "." + executor.Invoker.Method.Name);
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
