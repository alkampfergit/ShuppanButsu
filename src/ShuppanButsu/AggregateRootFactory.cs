using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using ShuppanButsu.Domain;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu
{
    public class AggregateRootFactory
    {
        private static MemberSetter interceptorSetter;
        private static MethodInvoker raiseEventInvoker;

        public IDomainEventInterceptor DomainEventInterceptor { get; set; }

        public AggregateRootFactory() 
        {
            DomainEventInterceptor = NullEventInterceptor.Instance;
        }

        static AggregateRootFactory() 
        {
            interceptorSetter = typeof(AggregateRoot)
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic)
                .Single(p => typeof(IDomainEventInterceptor).IsAssignableFrom(p.PropertyType))
                .DelegateForSetPropertyValue();

            raiseEventInvoker = typeof(AggregateRoot)
                .GetMethod("RaiseEvent", BindingFlags.NonPublic | BindingFlags.Instance)
                .DelegateForCallMethod();
        }


        public T Create<T>() where T : AggregateRoot 
        {
            T instance = (T) Activator.CreateInstance(typeof(T), true); 
            interceptorSetter.Invoke(instance, DomainEventInterceptor);
            //AR should not generate events on constructors, but if someone designed a class that raise events on the construcotr
            //those should be removed because they are not useful.
            ((IAggregateRoot)instance).ClearRaisedEvents();
            return instance;
        }

        public T Create<T>(AggregateRootCreationDomainEvent creationEvent) where T : AggregateRoot
        {
            T instance = Create<T>();
            raiseEventInvoker.Invoke(instance, creationEvent);
            return instance;
        }
    }
}
