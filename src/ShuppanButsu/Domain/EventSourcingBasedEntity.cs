using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Domain
{
    /// <summary>
    /// It is a base class to support event sourcing.
    /// </summary>
    public class EventSourcingBasedEntity : IEventSourcedEntity
    {
        #region Basic properties

        public String Id { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        private IDomainEventInterceptor Interceptor 
        {
            get { return _interceptor ?? (_interceptor = NullEventInterceptor.Instance); } 
            set {_interceptor = value;} 
        }
        private IDomainEventInterceptor _interceptor;
        #endregion

        #region IAggregateRoot

        void IEventSourcedEntity.ApplyEvent(DomainEvent @event)
        {
            if (@event is AggregateRootCreationDomainEvent)
            {
                Id = ((AggregateRootCreationDomainEvent)@event).Id;
            }
            FasterflectInvoker.Invoke(this, @event);
        }

        IEnumerable<DomainEvent> IEventSourcedEntity.GetRaisedEvents()
        {
            return _raisedEvents ?? emptyEvents;
        }

        void IEventSourcedEntity.ClearRaisedEvents()
        {
            _raisedEvents = null;
        }

        #endregion

        #region Event management

        private static IEnumerable<DomainEvent> emptyEvents = new DomainEvent[] { };

        private List<DomainEvent> _raisedEvents;
        private List<DomainEvent> RaisedEvents { get { return _raisedEvents ?? (_raisedEvents = new List<DomainEvent>()); } }

        protected void RaiseEvent(DomainEvent @event)
        {
            if (@event is AggregateRootCreationDomainEvent)
            {
                Id = ((AggregateRootCreationDomainEvent)@event).Id;
            }
            Interceptor.OnGenerated(@event);
            ((IEventSourcedEntity)this).ApplyEvent(@event);
            RaisedEvents.Add(@event);
        }

        #endregion

        #region Construction


        #endregion
    }

    internal static class FasterflectInvoker
    {
        /// <summary>
        /// Cache of appliers, for each domain object I have a dictionary of actions
        /// </summary>
        private static Dictionary<Type, Dictionary<Type, MethodInvoker>> invokerCache =
            new Dictionary<Type, Dictionary<Type, MethodInvoker>>();

        public static void Invoke(Object obj, DomainEvent domainEvent)
        {
            if (!invokerCache.ContainsKey(obj.GetType()))
            {
                var typeCache = new Dictionary<Type, MethodInvoker>();

                var applyMethods = obj.GetType().GetMethods(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                foreach (var item in applyMethods
                    .Where(am => am.Name.Equals("apply", StringComparison.OrdinalIgnoreCase))
                    .Select(am => new { parameters = am.GetParameters(), minfo = am })
                    .Where(p => p.parameters.Length == 1 &&
                        typeof(DomainEvent).IsAssignableFrom(p.parameters[0].ParameterType)))
                {
                    var localItem = item;
                    typeCache[localItem.parameters[0].ParameterType] = item.minfo.DelegateForCallMethod();
                }
                invokerCache[obj.GetType()] = typeCache;

            }
            var thisTypeCache = invokerCache[obj.GetType()];
            MethodInvoker invoker;
            if (thisTypeCache.TryGetValue(domainEvent.GetType(), out invoker))
            {
                invoker.Invoke(obj, domainEvent);
            }

        }

    }
}
