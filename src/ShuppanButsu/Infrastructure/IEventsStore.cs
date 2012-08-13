using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Infrastructure
{
    /// <summary>
    /// The basic interface that implements a minimal event store capable of storing events.
    /// </summary>
    public interface IEventsStore
    {
        /// <summary>
        /// Persists a series of domain events, each block of events is uniquely identified by 
        /// a guid that mark that specific commit.
        /// </summary>
        /// <param name="domainEvents">The list of events that belongs to this commit.</param>
        /// <returns>The ticks of datetime timestamp that were</returns>
        void PersistEvents(IEnumerable<Event> domainEvents, Guid commitId);

        /// <summary>
        /// Retrieve all the events that have that specific correlation id.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        IEnumerable<Event> GetByCorrelationId(String correlationId);

        /// <summary>
        /// Gets the events related to that commit id
        /// </summary>
        /// <param name="commitId"></param>
        /// <returns></returns>
        IEnumerable<Event> GetByCommitId(Guid commitId);

        /// <summary>
        /// Retrieve a stream of events that are comprised between two ticks.
        /// </summary>
        /// <param name="tickFrom"></param>
        /// <param name="tickTo"></param>
        /// <returns></returns>
        IEnumerable<Event> GetRange(Int64 tickFrom, Int64 tickTo);
    }

    public class Event 
    {
        /// <summary>
        /// Payload of the event, it can be the <see cref="DomainEvent"/> real object stored in the event.
        /// </summary>
        public Object Payload { get; private set; }

        /// <summary>
        /// Events are stored as a continuous sequence of events, we can retrieve them searching for this 
        /// specific field and nothing else.
        /// </summary>
        public String CorrelationId { get; set; }

        /// <summary>
        /// It Contains the tick id extracted by the datetime
        /// </summary>
        public Int64 Ticks { get; set; }

        public Event() : this (null, String.Empty)
        { 
        
        }

        /// <summary>
        /// Simple Event for a domain that does not want any corrleationId
        /// </summary>
        /// <param name="payload"></param>
        public Event(Object payload) : this (payload, string.Empty) 
        {
   
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        public Event(Object payload, String correlationId)
            : this(payload, correlationId, DateTime.Now.Ticks) 
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        public Event(Object payload, String correlationId, Int64 tickId)
        {
            Payload = payload;
            CorrelationId = correlationId;
            Ticks = tickId;
        }
    }
}
