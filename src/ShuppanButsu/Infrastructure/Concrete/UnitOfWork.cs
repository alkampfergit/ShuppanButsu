using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain;

namespace ShuppanButsu.Infrastructure.Concrete
{
    public class UnitOfWork
    {
        private IEventsStore _eventStore;
        private IDomainEventDispatcher _domainEventDispatcher;
        private AggregateRootFactory _factory;

        public UnitOfWork(IEventsStore eventsStore, IDomainEventDispatcher domainEventDispatcher, AggregateRootFactory factory) 
        {
            _eventStore = eventsStore;
            _domainEventDispatcher = domainEventDispatcher;
            _factory = factory;
        }

        private Dictionary<Guid, AggregateRoot> _idMap = new Dictionary<Guid, AggregateRoot>();

        /// <summary>
        /// Return an aggregate root with the id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(Guid id) where T : AggregateRoot, new()
        {
            if (_idMap.ContainsKey(id)) return (T) _idMap[id];

            T aggregateRoot = _factory.Create<T>();

            var commits = _eventStore.GetByCorrelationId(id.ToString());
            foreach (var commit in commits)
            {
                ((IAggregateRoot)aggregateRoot).ApplyEvent((DomainEvent)commit.Payload);
            }

            _idMap.Add(id, aggregateRoot);
            return aggregateRoot;
        }

        public void Save(AggregateRoot ar) 
        {
            if (_idMap.ContainsKey(ar.Id)) throw new ArgumentException("Aggregate root with the same id was already associated to this UnitOfWork");
            _idMap.Add(ar.Id, ar);
        }

        /// <summary>
        /// Save an entity in an event store, it actually saves all the uncommitted events and returns
        /// the list of saved events.
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="commitId"></param>
        /// <returns>The list of domain events that were saved for this aggregate root.</returns>
        public IEnumerable<DomainEvent> Commit(Guid commitId)
        {
            List<Event> eventsToStore = new List<Event>();
            foreach (var mapEntry in _idMap)
            {
                eventsToStore.AddRange(
                    ((IAggregateRoot)mapEntry.Value).GetRaisedEvents()
                    .Select(evt => new Event(evt, mapEntry.Key.ToString(), evt.Timestamp.Ticks)));
            }
            
            //now save everything with a single commit id.
            _eventStore.PersistEvents(eventsToStore, commitId);

            foreach (var ar in _idMap.Values)
            {
                ((IAggregateRoot)ar).ClearRaisedEvents();
            }

            var orderedEvents = eventsToStore.OrderBy(e => e.Ticks).Select(e => (DomainEvent) e.Payload);
            foreach (var @event in orderedEvents)
            {
                _domainEventDispatcher.DispatchEvent(@event);
            }

            //Now clear the identity map
            _idMap.Clear();

            return orderedEvents;

        }
    }
}
