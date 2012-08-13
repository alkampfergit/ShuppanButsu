using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using ShuppanButsu.Utils.JsonNet;

namespace ShuppanButsu.Infrastructure.Concrete.EventsStore
{
    /// <summary>
    /// Primitive implementation of an event store that store events in file system.
    /// </summary>
    public class SqLiteEventsStore : IEventsStore, IDisposable
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration cfg;

        JsonSerializerSettings serializerSettings;

        public SqLiteEventsStore(String configurationFileName) 
        { 
            XmlReader reader = XmlReader.Create(new StringReader(File.ReadAllText(configurationFileName)));
            Init(reader);
        }

        public SqLiteEventsStore(XmlReader reader)
        {
            Init(reader);
        }

        private void Init(XmlReader reader)
        {
            cfg = new Configuration();
            cfg.Configure(reader);

            var mapper = new ConventionModelMapper();
            //mapper.IsEntity((t, declared) => t.Namespace.StartsWith("Sample.QueryModel") || );

            mapper.AfterMapClass += (inspector, type, classCustomizer) =>
            {
                classCustomizer.Lazy(false);
                //classCustomizer.Id(m => m.Generator(new GuidGeneratorDef()));
            };
            var mapping = mapper.CompileMappingFor(new[] { typeof(SqlEvent) });
            var allmapping = mapping.AsString();

            cfg.AddDeserializedMapping(mapping, "AutoModel");
            _sessionFactory = cfg.BuildSessionFactory();

            var export = new SchemaUpdate(cfg);
            export.Execute(false, true);

            serializerSettings = new JsonSerializerSettings();
            serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            serializerSettings.ContractResolver = new CustomContractResolver();
        }



        public void PersistEvents(IEnumerable<Event> domainEvents, Guid commitId)
        {
            throw new NotImplementedException();
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetByCorrelationId(string correlationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Event> GetByCommitId(Guid commitId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
           

        }

        public IEnumerable<Event> GetRange(long tickFrom, long tickTo)
        {
            throw new NotImplementedException();
        }
    }

    public class SqlEvent 
    {
        public Guid CommitId { get; set; }
        public String CorrleationId { get; set; }
        public Int64 Ticks { get; set; }
        public String Payload { get; set; }
    }
}
