﻿using System;
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
    /// that handle the message, it is used to 
    /// </summary>
    public class CommandDispatcher : ICommandDispatcher, IStartable
    {
        private ICommandHandlerCatalog _catalog;
        private ILogger _logger;

        private static Thread _executingThread;

        public CommandDispatcher(ICommandHandlerCatalog catalog, ILogger logger)
        {
            _catalog = catalog;
            _logger = logger;
        }

        #region syncronization

        private static BlockingCollection<ICommand> _commandQueue = new BlockingCollection<ICommand>();

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
            foreach (ICommand command in _commandQueue.GetConsumingEnumerable())
            {
                _logger.SetOpType("command", command.Id.ToString("N"));
                try
                {
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatching Command id: " + command.Id.ToString("N"), null, command);
                    InnerExecuteCommand(command);
                    if (_logger.IsDebugEnabled) _logger.Debug("Dispatched Command id: " + command.Id.ToString("N"), null, command);
                }
                finally 
                {
                    _logger.RemoveOpType();
                }

            }
        }

        #endregion

        public bool ExecuteCommand(ICommand command)
        {
            //TODO: Validate the command here, only valid commands should be inserted in the queue.
            _commandQueue.Add(command);
            return true;
        }

        private bool InnerExecuteCommand(ICommand command)
        {
            var executor = _catalog.GetExecutorFor(command.GetType());
            if (executor == null)
            {
                _logger.Error("No executor for command type " + command.GetType());
                return false;
            }

            try
            {
                if (_logger.IsDebugEnabled) _logger.Debug("Found command executor " + executor.DefiningType.FullName + "." + executor.Invoker.Method.Name);
                executor.Invoke(command);
                return true;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("exception during command execution", ex);
                return false;
            }
        }
    }
}
