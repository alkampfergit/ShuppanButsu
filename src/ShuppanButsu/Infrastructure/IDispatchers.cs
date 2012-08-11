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

    public interface IDomainEventInterceptor
    {
        void OnGenerated(DomainEvent @event);
    }

    public class NullEventInterceptor : IDomainEventInterceptor
    {
        public static NullEventInterceptor Instance { get; private set; }

        static NullEventInterceptor()
        {
            Instance = new NullEventInterceptor();
        }

        public void OnGenerated(DomainEvent @event)
        {
        }
    }
}
