using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Infrastructure
{
    /// <summary>
    /// Represent the catalog that is able to discover and enlist 
    /// command that are available to the system.
    /// </summary>
    public interface ICommandHandlerCatalog
    {
        /// <summary>
        /// It accepts the type of the command you want to execute and gives 
        /// you bach a function that accepts the command to execute. It completely
        /// remove any decision for the Default Command Router.
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        CommandInvoker GetExecutorFor(Type commandType);
    }

    public interface IDomainEventHandler
    {

    }

    public interface IDomainEventHandlerCatalog
    {
        /// <summary>
        /// Gets the list of invoker that are associated to all executors
        /// that are able to intercept that domain event.
        /// </summary>
        /// <param name="domainEventType"></param>
        /// <returns></returns>
        IEnumerable<DomainEventInvoker> GetAllHandlerFor(Type domainEventType);

        /// <summary>
        /// Retrieve all enumerated handlers type on the system.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetAllHandlers();

        /// <summary>
        /// We need the ability to replay all handler for a specific handler, thus the catalog
        /// should be able to give you a dictionary that stores all event handlers for a given
        /// handler type.
        /// </summary>
        /// <param name="handlerType">The type of the Handler that defines handlers</param>
        /// <returns>A dictionary where the key contains the type of domain event handled and the value
        /// is the Action that actually handles the event</returns>
        IDictionary<Type, DomainEventInvoker> GetAllHandlerForSpecificHandlertype(Type handlerType);


    }

    public interface ICommandExecutor 
    { 
    
    }

    /// <summary>
    /// a class that holds some detailed information about the functions that are used to handle 
    /// </summary>
    public class DomainEventInvoker
    {
        // the actual function that have to be invoked in order to handle the event
        public Action<DomainEvent> Invoker { get; set; }

        /// <summary>
        /// the type in which the onvoker is defined
        /// </summary>
        public Type DefiningType { get; set; }

        /// <summary>
        /// The type of event handled by this invoker.
        /// </summary>
        public Type HandledType { get; set; }

        public DomainEventInvoker(Action<DomainEvent> invoker, Type definingType, Type handledType)
        {
            Invoker = invoker;
            DefiningType = definingType;
            HandledType = handledType;
        }

        public void Invoke(DomainEvent @event)
        {
            Invoker.Invoke(@event);
        }
    }

    /// <summary>
    /// This class is analogous to the <see cref="DomainEventInvoker"/> and is used to 
    /// encapsulate all details about command invoker discovered by the catalog.
    /// </summary>
    public class CommandInvoker
    {
        // the actual function that have to be invoked in order to handle the event
        public Action<ICommand> Invoker;

        /// <summary>
        /// the type in which the onvoker is defined
        /// </summary>
        public Type DefiningType { get; set; }



        public CommandInvoker(Action<ICommand> invoker, Type definingType)
        {
            Invoker = invoker;
            DefiningType = definingType;
        }

        public void Invoke(ICommand command)
        {
            Invoker.Invoke(command);
        }
    }
}
