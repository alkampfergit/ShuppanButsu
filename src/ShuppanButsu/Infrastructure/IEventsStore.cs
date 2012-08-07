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
        /// Retrieve all events between two distinct date.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<Event> GetByTimestampRange(DateTime? start, DateTime? end);

        /// <summary>
        /// Same of <see cref="GetByTimestampRange(DateTime?, DateTime?)"/> but this one accepts ticks
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        IEnumerable<Event> GetByTimestampRange(Int64? start, DateTime? end);

        /// <summary>
        /// Gets the events related to that commit id
        /// </summary>
        /// <param name="commitId"></param>
        /// <returns></returns>
        IEnumerable<Event> GetByCommitId(Guid commitId);
    }

    public class Event 
    {
        /// <summary>
        /// DateTime express as tick.
        /// </summary>
        public Int64 Timestamp { get; set; }

        /// <summary>
        /// Payload of the event, it can be the <see cref="DomainEvent"/> real object stored in the event.
        /// </summary>
        public Object Payload { get; set; }

        /// <summary>
        /// Events are stored as a continuous sequence of events, we can retrieve them searching for this 
        /// specific field and nothing else.
        /// </summary>
        public String CorrelationId { get; set; }

        public Event() 
        { 
        
        }

        /// <summary>
        /// Simple Event for a domain that does not want any payload.
        /// </summary>
        /// <param name="payload"></param>
        public Event(Object payload) : this (payload, string.Empty) 
        {
   
        }

        /// <summary>
        /// Simple Event for a domain that does not want any payload.
        /// </summary>
        /// <param name="payload"></param>
        public Event(Object payload, String correlationId)
        {
            Timestamp = DateTime.Now.Ticks;
            Payload = payload;
            CorrelationId = correlationId;
        }
    }
}
