using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace ShuppanButsu.Utils
{
    public static class LoggingUtility
    {
        public static void SetOpType(this ILogger logger, string optype, string opTypeIdentification)
        {
            log4net.ThreadContext.Properties["op_type"] = optype;
            log4net.ThreadContext.Properties["op_type_id"] = opTypeIdentification;
        }

        public static void RemoveOpType(this ILogger logger)
        {
            log4net.ThreadContext.Properties.Remove("op_type");
            log4net.ThreadContext.Properties.Remove("op_type_id");
        }
    }
}
