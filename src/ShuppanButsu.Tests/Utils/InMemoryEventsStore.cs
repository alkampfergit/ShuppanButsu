using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Tests.Utils
{
    public class InMemoryEventsStore : IEventsStore
    {
        private class Slot {
           public Event Event;
           public Guid CommitId;
        }
        List<Slot> _events = new List<Slot>();

        public void PersistEvents(IEnumerable<Event> domainEvents, Guid commitId)
        {
            foreach (var @event in domainEvents)
            {
                _events.Add(new Slot() { Event = @event, CommitId = commitId });
            }
        }

        public IEnumerable<Event> GetByCorrelationId(string correlationId)
        {
            foreach (var slot in _events)
            {
                if (slot.Event.CorrelationId.Equals(correlationId))
                    yield return slot.Event;
            }
        }

        public IEnumerable<Event> GetByTimestampRange(DateTime? start, DateTime? end)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetByTimestampRange(long? start, DateTime? end)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetByCommitId(Guid commitId)
        {
            foreach (var slot in _events)
            {
                if (slot.CommitId.Equals(commitId))
                    yield return slot.Event;
            }
        }


        public IEnumerable<Event> GetRange(long tickFrom, long tickTo)
        {
            return _events.Select(e => e.Event);
        }
    }
}
