using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }

        void ApplyEvent(DomainEvent @event);

        /// <summary>
        /// Retrieve the list of events that were actually stored in the AggregateRoot
        /// </summary>
        /// <returns></returns>
        IEnumerable<DomainEvent> GetRaisedEvents();

        /// <summary>
        /// Clear the list of events that were internally stored by the aggregate root
        /// </summary>
        void ClearRaisedEvents();
    }
}
