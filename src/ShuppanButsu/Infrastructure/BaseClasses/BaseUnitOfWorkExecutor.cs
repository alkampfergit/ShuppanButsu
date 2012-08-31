using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure.Concrete;

namespace ShuppanButsu.Infrastructure.BaseClasses
{
    public abstract class BaseUnitOfWorkExecutor : ICommandExecutor
    {
        protected UnitOfWork _uow;

        public BaseUnitOfWorkExecutor(UnitOfWork uow)
        {
            _uow = uow;
        }

    }
}
