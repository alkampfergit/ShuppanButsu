using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace ShuppanButsu.Tests.Utils
{
    [Serializable]
    public class TestLogger : MarshalByRefObject, ILogger
    {
        // Fields
        private string name;
        private LoggerLevel level = LoggerLevel.Warn;
        // Methods
        public TestLogger()
        {
            this.name = "unnamed";
        }

        public TestLogger(LoggerLevel loggerLevel)
        {
            this.name = "unnamed";
            level = loggerLevel;
        }

        public TestLogger(string name)
        {
            this.name = "unnamed";
            this.ChangeName(name);
        }

        public TestLogger(string loggerName, LoggerLevel loggerLevel)
            : this(loggerLevel)
        {
            this.ChangeName(loggerName);
        }

        protected void ChangeName(string newName)
        {
            if (newName == null)
            {
                throw new ArgumentNullException("newName");
            }
            this.name = newName;
        }

        public ILogger CreateChildLogger(string loggerName)
        {
            if (loggerName == null)
            {
                throw new ArgumentNullException("loggerName", "To create a child logger you must supply a non null name");
            }
            return new ConsoleLogger(string.Format(CultureInfo.CurrentCulture, "{0}.{1}", new object[] { Name, loggerName }));
        }



        public void Debug(string message)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, message, null);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, message, exception);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void DebugFormat(Exception exception, string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(CultureInfo.CurrentCulture, format, args), exception);
            }
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(formatProvider, format, args), null);
            }
        }

        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, string.Format(formatProvider, format, args), exception);
            }
        }

        public void Error(string message)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, message, null);
            }
        }

        public void Error(string message, Exception exception)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, message, exception);
            }
        }

        public void Error(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(CultureInfo.CurrentCulture, format, args), exception);
            }
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(formatProvider, format, args), null);
            }
        }

        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, string.Format(formatProvider, format, args), exception);
            }
        }

        public void Fatal(string message)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message, null);
            }
        }

        public void Fatal(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void Fatal(string message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message, exception);
            }
        }

        [Obsolete("Use Fatal instead")]
        public void FatalError(string message)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message, null);
            }
        }

        [Obsolete("Use Fatal instead")]
        public void FatalError(string message, Exception exception)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, message, exception);
            }
        }

        [Obsolete("Use Fatal or FatalFormat instead")]
        public void FatalError(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(CultureInfo.CurrentCulture, format, args), exception);
            }
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(formatProvider, format, args), null);
            }
        }

        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, string.Format(formatProvider, format, args), exception);
            }
        }

        public void Info(string message)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, message, null);
            }
        }

        public void Info(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void Info(string message, Exception exception)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, message, exception);
            }
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void InfoFormat(Exception exception, string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(CultureInfo.CurrentCulture, format, args), exception);
            }
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(formatProvider, format, args), null);
            }
        }

        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, string.Format(formatProvider, format, args), exception);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void Log(LoggerLevel loggerLevel, string message, Exception exception)
        {
            this.Log(loggerLevel, this.Name, message, exception);
        }

        protected void Log(LoggerLevel loggerLevel, string loggerName, string message, Exception exception)
        {

            TextWriter output = Console.Out;
            if (loggerLevel == LoggerLevel.Error || loggerLevel == LoggerLevel.Fatal)
                output = Console.Error;

            output.WriteLine("[{0}] '{1}' {2}", loggerLevel, loggerName, message);
            if (exception != null)
            {
                output.WriteLine("[{0}] '{1}' {2}: {3} {4}", new object[] { loggerLevel.ToString(), loggerName, exception.GetType().FullName, exception.Message, exception.StackTrace });
            }
        }
        public void Warn(string message)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, message, null);
            }
        }

        public void Warn(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void Warn(string message, Exception exception)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, message, exception);
            }
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(CultureInfo.CurrentCulture, format, args), null);
            }
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(CultureInfo.CurrentCulture, format, args), exception);
            }
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(formatProvider, format, args), null);
            }
        }

        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, string.Format(formatProvider, format, args), exception);
            }
        }

        // Properties
        public bool IsDebugEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Debug);
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Error);
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Fatal);
            }
        }

        [Obsolete("Use IsFatalEnabled instead")]
        public bool IsFatalErrorEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Fatal);
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Info);
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
                return (this.Level >= LoggerLevel.Warn);
            }
        }

        public LoggerLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }



        public string Name
        {
            get
            {
                return this.name;
            }
        }


        public void Debug(Func<string> messageFactory)
        {
            if (this.IsDebugEnabled)
            {
                this.Log(LoggerLevel.Debug, messageFactory(), null);
            }
        }

        public void Error(Func<string> messageFactory)
        {
            if (this.IsErrorEnabled)
            {
                this.Log(LoggerLevel.Error, messageFactory(), null);
            }
        }

        public void Fatal(Func<string> messageFactory)
        {
            if (this.IsFatalEnabled)
            {
                this.Log(LoggerLevel.Fatal, messageFactory(), null);
            }
        }

        public void Info(Func<string> messageFactory)
        {
            if (this.IsInfoEnabled)
            {
                this.Log(LoggerLevel.Info, messageFactory(), null);
            }
        }

        public void Warn(Func<string> messageFactory)
        {
            if (this.IsWarnEnabled)
            {
                this.Log(LoggerLevel.Warn, messageFactory(), null);
            }
        }
    }
}
