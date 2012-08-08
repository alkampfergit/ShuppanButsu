﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Fasterflect;

namespace ShuppanButsu.Infrastructure.Concrete
{
    public class CastleFasterflectHandlerCatalog : ICommandHandlerCatalog, IDomainEventHandlerCatalog
    {
        private IKernel _kernel;

        private ILogger _logger;

        private Type[] _domainEventHandlers;

        private Type[] _commandExecutorTypes;

        /// <summary>
        /// Scans all the assemblies to find all the candidate command executors.
        /// </summary>
        public CastleFasterflectHandlerCatalog(IKernel kernel, ILogger logger)
        {
            _kernel = kernel;
            _logger = logger;
            String baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            String binDirectory = Path.Combine(baseDirectory, "bin");
            if (Directory.Exists(binDirectory))
            {
                ScanAllAssembliesInDirectory(binDirectory);
            }
            else
            {
                ScanAllAssembliesInDirectory(baseDirectory);
            }

        }

        private void ScanAllAssembliesInDirectory(String enumerationDirectory)
        {
            var files = Directory.EnumerateFiles(enumerationDirectory);
            foreach (var fileName in files)
            {
                //provare a caricare dinamicamente un assembly
                if (Path.GetExtension(fileName).EndsWith("dll") || Path.GetExtension(fileName).EndsWith("exe"))
                {
                    try
                    {
                        var asmName = AssemblyName.GetAssemblyName(fileName);

                        Assembly dynamicAsm = Assembly.Load(asmName);
                        Type[] allAssemblyTypes = null;
                        try
                        {
                            allAssemblyTypes = dynamicAsm.GetTypes();

                        }
                        catch (ReflectionTypeLoadException rtl)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var singleLoadException in rtl.LoaderExceptions)
                            {
                                sb.AppendLine(singleLoadException.Message);
                            }
                            _logger.Error("Unable to scan asssembly " + asmName.Name + " reason:\n" + sb.ToString());
                            continue;
                            //throw new ApplicationException("CastleFastReflectHandlerCatalog is unable to scan type of assembly " + asmName + "\n" + sb.ToString());
                        }

                        //convert to array to avoid risk of using list and outer modification.
                        _commandExecutorTypes = ScanForCommandExecutors(allAssemblyTypes).ToArray();
                        _domainEventHandlers = ScanForDomainEventHandler(allAssemblyTypes).ToArray();
                        foreach (var type in _commandExecutorTypes.Union(_domainEventHandlers))
                        {
                            _kernel.Register(Component.For(type).ImplementedBy(type).LifeStyle.Singleton);
                        }
                    }
                    catch (TypeLoadException ex)
                    {
                        //Create a log that tells what is wrong with that type
                        throw;
                    }
                    catch (BadImageFormatException ex)
                    {
                        //this is not a .net assembly
                        _logger.Info("Dll " + fileName + " probably is not a .NET dll and cannot be scanned for command handler or domain handler", ex);
                    }
                }
            }

        }

        private List<Type> ScanForCommandExecutors(Type[] allAssemblyTypes)
        {
            //now each of this class could contains a method that accepts a specific ICommandType, whatever
            //method accepts a single object that implements ICommand and returns void is a command executor.
            //I want also this object to be resolved by castle, because it can have dependencies.
            var executors = allAssemblyTypes
                              .Where(t => t.IsClass && !t.IsAbstract && typeof(ICommandExecutor).IsAssignableFrom(t))
                              .ToList();
            foreach (var executorType in executors)
            {

                ParameterInfo[] parameters = null;
                foreach (var minfo in executorType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(mi => mi.ReturnType == typeof(void) &&
                                    (parameters = mi.GetParameters()).Length == 1 &&
                                    typeof(ICommand).IsAssignableFrom(parameters[0].ParameterType)))
                {

                    var commandType = parameters[0].ParameterType;
                    if (cachedExecutors.ContainsKey(commandType))
                    {
                        var alreadyRegisteredInvoker = cachedExecutors[commandType];
                        String exceptionText = String.Format("Multiple handler for command {0} found: {1}.{2} and {3}. {4}",
                            commandType.Name, alreadyRegisteredInvoker.ExecutorType.FullName, alreadyRegisteredInvoker.MethodName,
                            executorType.FullName, minfo.Name);
                        throw new ApplicationException(exceptionText);
                    }
                    //I've found a method returning void accepting a command, for me is a command executor
                    MethodInvoker fastReflectInvoker = minfo.DelegateForCallMethod();
                    cachedExecutors.Add(commandType, new CommandExecutorInfo(fastReflectInvoker, executorType, _kernel, minfo.Name));
                }
            }
            return executors;
        }

        private List<Type> ScanForDomainEventHandler(Type[] allAssemblyTypes)
        {
            //now each of this class could contains a method that accepts a specific ICommandType, whatever
            //method accepts a single object that implements ICommand and returns void is a command executor.
            //I want also this object to be resolved by castle, because it can have dependencies.
            var handlers = allAssemblyTypes
                                .Where(t => t.IsClass && !t.IsAbstract && typeof(IDomainEventHandler).IsAssignableFrom(t))
                                .ToList();
            foreach (var eventHandlerType in handlers)
            {

                ParameterInfo[] parameters = null;
                foreach (var minfo in eventHandlerType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(mi => mi.ReturnType == typeof(void) &&
                                    (parameters = mi.GetParameters()).Length == 1 &&
                                    typeof(DomainEvent).IsAssignableFrom(parameters[0].ParameterType)))
                {
                    var eventType = parameters[0].ParameterType;

                    //I've found a method returning void accepting a Domain Event it is an handler
                    MethodInvoker fastReflectInvoker = minfo.DelegateForCallMethod();
                    cachedHandlers.Add(new DomainEventHandlerInfo(fastReflectInvoker, eventHandlerType, eventType, _kernel, minfo.Name));
                }
            }
            return handlers;
        }

        private class CommandExecutorInfo
        {
            private MethodInvoker _invoker;

            public Type ExecutorType { get; private set; }

            private IKernel _kernel;

            public String MethodName { get; private set; }

            public CommandExecutorInfo(MethodInvoker invoker, Type executorType, IKernel kernel, String methodName)
            {
                _invoker = invoker;
                _kernel = kernel;
                ExecutorType = executorType;
                MethodName = methodName;
            }

            public void Execute(ICommand command)
            {

                Object executor = null;
                try
                {
                    executor = _kernel.Resolve(ExecutorType);
                    _invoker.Invoke(executor, new Object[] { command });
                }
                finally
                {
                    _kernel.ReleaseComponent(executor);
                }
            }


        }

        private Dictionary<Type, CommandExecutorInfo> cachedExecutors = new Dictionary<Type, CommandExecutorInfo>();

        public CommandInvoker GetExecutorFor(Type commandType)
        {
            if (!cachedExecutors.ContainsKey(commandType))
            {
                throw new NotSupportedException("No command handler for " + commandType);
            }
            var info = cachedExecutors[commandType];
            return new CommandInvoker(info.Execute, info.ExecutorType);
        }


        /// <summary>
        /// value holder for the command handler info.
        /// </summary>
        private class DomainEventHandlerInfo
        {
            public MethodInvoker _invoker;

            public Type ExecutorType { get; private set; }

            private IKernel _kernel;

            public Type EventType { get; private set; }

            public String MethodName { get; private set; }

            public DomainEventHandlerInfo(MethodInvoker invoker, Type executorType, Type eventType, IKernel kernel, String methodName)
            {
                _invoker = invoker;
                ExecutorType = executorType;
                EventType = eventType;
                _kernel = kernel;
                MethodName = methodName;
            }

            public Boolean CanHandleEvent(Type domainEventType)
            {
                return EventType.IsAssignableFrom(domainEventType);
            }

            public void Execute(DomainEvent @event)
            {
                Object executor = null;
                try
                {
                    executor = _kernel.Resolve(ExecutorType);
                    _invoker.Invoke(executor, new Object[] { @event });
                }
                finally
                {
                    _kernel.ReleaseComponent(executor);
                }
            }


        }

        private List<DomainEventHandlerInfo> cachedHandlers = new List<DomainEventHandlerInfo>();

        public IEnumerable<DomainEventInvoker> GetAllHandlerFor(Type domainEventType)
        {
            //TODO: Cache this
            return cachedHandlers
                .Where(h => h.CanHandleEvent(domainEventType))
                .Select<DomainEventHandlerInfo, DomainEventInvoker>(h => new DomainEventInvoker(h.Execute, h.ExecutorType, h.EventType));
        }

        public IEnumerable<Type> GetAllHandlers()
        {
            //TODO: Cache this
            return cachedHandlers
                .Select(h => h.ExecutorType)
                .Distinct();
        }

        public IDictionary<Type, DomainEventInvoker> GetAllHandlerForSpecificHandlertype(Type handlerType)
        {
            Dictionary<Type, DomainEventInvoker> retValue = new Dictionary<Type, DomainEventInvoker>();
            foreach (var item in cachedHandlers
                .Where(h => h.ExecutorType == handlerType))
            {
                retValue.Add(item.EventType, new DomainEventInvoker(item.Execute, item.ExecutorType, item.EventType));
            }
            return retValue;
        }



    }

}
