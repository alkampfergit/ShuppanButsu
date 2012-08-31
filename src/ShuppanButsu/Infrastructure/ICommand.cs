using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Infrastructure
{
    public interface ICommand
    {
        Guid Id { get; }
    }

    /// <summary>
    /// Convenience class to avoid implementing some basic command properties in 
    /// all concrete classes
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        public Guid Id { get; protected set; }

        public CommandBase() {
            Id = Guid.NewGuid();
        }
    }
}
