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
using NHibernate.Linq;

namespace ShuppanButsu.Infrastructure.Concrete.EventsStore
{
    /// <summary>
    /// Primitive implementation of an event store that store events in file system.
    /// </summary>
    public class SqlEventsStore : IEventsStore, IDisposable
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration cfg;

        JsonSerializerSettings serializerSettings;

        public SqlEventsStore(String configurationFileName) 
        { 
            XmlReader reader = XmlReader.Create(new StringReader(File.ReadAllText(configurationFileName)));
            Init(reader);
        }

        public SqlEventsStore(XmlReader reader)
        {
            Init(reader);
        }

        private void Init(XmlReader reader)
        {
            cfg = new Configuration();
            cfg.Configure(reader);

            var mapper = new ConventionModelMapper();
            mapper.AfterMapClass += (inspector, type, classCustomizer) =>
            {
                classCustomizer.Lazy(false);
            };
            mapper.Class<SqlEvent>(map => map.Id(se => se.Id, idm => idm.Generator(Generators.Native)));

            var mapping = mapper.CompileMappingFor(new[] { typeof(SqlEvent) });
            //mapper.Class<SqlEvent>(m => m.Id(evt => evt.Ticks));
            //Console.Write(mapping.AsString());
            cfg.AddDeserializedMapping(mapping, "AutoModel");
            _sessionFactory = cfg.BuildSessionFactory();

            var export = new SchemaUpdate(cfg);
            export.Execute(false, true);

            serializerSettings = new JsonSerializerSettings();
            serializerSettings.TypeNameHandling = TypeNameHandling.All;
            serializerSettings.ContractResolver = new CustomContractResolver();
        }

        /// <summary>
        /// Persist a bunch of events.
        /// </summary>
        /// <param name="domainEvents"></param>
        /// <param name="commitId"></param>
        public void PersistEvents(IEnumerable<Event> domainEvents, Guid commitId)
        {
            using (var session = _sessionFactory.OpenSession())  
            using (var transaction = session.BeginTransaction())
            {
                if (session.Query<SqlEvent>().Count(se => se.CommitId == commitId) > 0) 
                {
                    throw new ArgumentException("Another commit was present with the same commitId", "commitId");
                }
                foreach (var evt in domainEvents)
                {
                    SqlEvent sqlEvent = new SqlEvent();
                    sqlEvent.CommitId = commitId;
                    sqlEvent.CorrleationId = evt.CorrelationId;
                    sqlEvent.Payload = JsonConvert.SerializeObject(evt.Payload, serializerSettings);
                    sqlEvent.Ticks = evt.Ticks;
                    session.Save(sqlEvent);
                }
                transaction.Commit();
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetByCorrelationId(string correlationId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<SqlEvent>()
                    .Where(se => se.CorrleationId == correlationId)
                    .OrderBy(se => se.Ticks)
                    .Select(se => new Event(
                        JsonConvert.DeserializeObject(se.Payload, serializerSettings),
                        se.CorrleationId,
                        se.Ticks))
                    .ToList();
            }
        }

        public IEnumerable<Event> GetByCommitId(Guid commitId)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Query<SqlEvent>()
                    .Where(se => se.CommitId == commitId)
                    .OrderBy(se => se.Ticks)
                    .Select(se => new Event(
                        JsonConvert.DeserializeObject(se.Payload, serializerSettings),
                        se.CorrleationId,
                        se.Ticks))
                    .ToList();
            }
        }

        public void Dispose()
        {
           

        }

        public IEnumerable<Event> GetRange(long tickFrom, long tickTo)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                return session.Query<SqlEvent>()
                    .Where(se => se.Ticks >= tickFrom && se.Ticks <= tickTo)
                    .OrderBy(se => se.Ticks)
                    .Select(se => new Event(
                        JsonConvert.DeserializeObject(se.Payload, serializerSettings),
                        se.CorrleationId,
                        se.Ticks))
                    .ToList();
            }
        }
    }

    public class SqlEvent 
    {
        public Int64 Id { get; set; }
        public Guid CommitId { get; set; }
        public String CorrleationId { get; set; }
        public Int64 Ticks { get; set; }
        public String Payload { get; set; }
    }
}
