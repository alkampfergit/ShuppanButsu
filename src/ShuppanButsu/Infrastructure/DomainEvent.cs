using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Utils;

namespace ShuppanButsu.Infrastructure
{
    public class DomainEvent
    {
        public DateTime Timestamp { get; private set; }

        public DomainEvent() 
        {
            Timestamp = DateTime.Now;
        }
    }

    public class AggregateRootCreationDomainEvent : DomainEvent
    {
        public String Id { get; private set; }

        public AggregateRootCreationDomainEvent(String id) 
        {
            Id = id;
        }

        public AggregateRootCreationDomainEvent()
        {
            Id = Guid.NewGuid().ToAggregateRootId();
        }
    }
}
