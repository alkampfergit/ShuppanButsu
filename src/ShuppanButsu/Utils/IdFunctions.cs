using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShuppanButsu.Utils
{
    public static class IdFunctions
    {
        public static String ToAggregateRootId(this Guid id) 
        {
            return id.ToString("N");
        }
    }
}
