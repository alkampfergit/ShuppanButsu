using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace ShuppanButsu.Infrastructure
{
    /// <summary>
    /// This class declares some basic dependencies that are useful for derived handlers
    /// </summary>
    public class BaseDomainEventHandler : IDomainEventHandler
    {
        /// <summary>
        /// Reference to a logger
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Reference to the configuration of ShuppanButsu engine.
        /// </summary>
        public ShuppanButsuConfiguration Configuration { get; set; }
    }
}
