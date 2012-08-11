using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Infrastructure
{
    public interface ICommandDispatcher
    {
        Boolean ExecuteCommand(ICommand command);
    }

    public interface IDomainEventDispatcher 
    {

        void DispatchEvent(DomainEvent @event);
    }
}
