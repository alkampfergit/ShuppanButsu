using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain;

namespace ShuppanButsu.Infrastructure
{
    /// <summary>
    /// No interface IRepository, because this is the only repository type I need, there is no 
    /// need for abstraction.
    /// </summary>
    public class Repository
    {
        private IEventsStore _eventStore;

        public Repository(IEventsStore eventsStore) 
        {
            _eventStore = eventsStore;
        }

        /// <summary>
        /// Return an aggregate root with the id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(Guid id) where T : AggregateRoot, new()
        {
            T aggregateRoot = new T();
            //A call to clear raised events is needed if the class implement raising events in default constructor
            ((IAggregateRoot)aggregateRoot).ClearRaisedEvents();
            var commits = _eventStore.GetByCorrelationId(id.ToString());
            foreach (var commit in commits)
            {
                ((IAggregateRoot)aggregateRoot).ApplyEvent((DomainEvent) commit.Payload);
            }
            return aggregateRoot;
        }

        public void Save(IAggregateRoot aggregateRoot, Guid commitId) 
        { 
            var uncommittedEvents = aggregateRoot.GetRaisedEvents();
            _eventStore.PersistEvents(
                uncommittedEvents.Select(evt => new Event(evt, aggregateRoot.Id.ToString())), commitId);
            aggregateRoot.ClearRaisedEvents();
            
        }

    }
}
