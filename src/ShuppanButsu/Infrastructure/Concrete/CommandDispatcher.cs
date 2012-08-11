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
    /// that handle the message, it is used to 
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher
    {
        private ICommandHandlerCatalog _catalog;
        private ILogger _logger;

        public CommandDispatcher(ICommandHandlerCatalog catalog, ILogger logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        public bool ExecuteCommand(ICommand command)
        {
            var executor = _catalog.GetExecutorFor(command.GetType());
            if (executor == null)
            {
                _logger.Error("No executor for command type " + command.GetType());
                return false;
            }

            try
            {
                executor.Invoke(command);
                return true;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
