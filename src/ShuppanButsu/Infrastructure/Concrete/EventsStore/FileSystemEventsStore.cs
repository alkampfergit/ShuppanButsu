using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShuppanButsu.Infrastructure.Concrete.EventsStore
{
    /// <summary>
    /// Primitive implementation of an event store that store events in file system.
    /// </summary>
    public class FileSystemEventsStore : IEventsStore, IDisposable
    {
        private String _baseDirectory;
        FileStream _lockFilestream;

        String _commitIdFolder;
        String _eventFolder;
        
        JsonSerializerSettings serializerSettings;

        public FileSystemEventsStore(String baseDirectory)
        {
            _baseDirectory = baseDirectory;
            //TODO: Try to understand if the directory is locked by someone else and do a graceful error message
            _lockFilestream = File.Open(GetLockFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            StreamWriter wr = new StreamWriter(_lockFilestream);
            wr.Write(Environment.MachineName + " pid=" + Process.GetCurrentProcess().Id);
            wr.Close();

            _commitIdFolder = Path.Combine(baseDirectory, "Commits");
            if (!Directory.Exists(_commitIdFolder)) Directory.CreateDirectory(_commitIdFolder);

            _eventFolder = Path.Combine(baseDirectory, "Events");
            if (!Directory.Exists(_eventFolder)) Directory.CreateDirectory(_eventFolder);

            serializerSettings = new JsonSerializerSettings();
            serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        }

        private string GetLockFileName()
        {
            return Path.Combine(_baseDirectory, "eventstore.lock");
        }

        public void PersistEvents(IEnumerable<Event> domainEvents, Guid commitId)
        {

            //Need to persist events with a given commitId, 
            String commitIdFile = GetCommitIdFile(commitId);

            //now write all the streams and the relative position in files
            var indexes = new List<Index>();

            foreach (var @event in domainEvents)
            {
                String correlationId = String.IsNullOrWhiteSpace(@event.CorrelationId) ? "null" : @event.CorrelationId;
                String correlationFileName = GetCorrelationFileName(correlationId);
                String serialized = JsonConvert.SerializeObject(@event, serializerSettings);
                using (FileStream fs = File.Open(correlationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                using (BinaryWriter sw = new BinaryWriter(fs))
                {
                    fs.Seek(0, SeekOrigin.End);
                    indexes.Add(new Index(fs.Position, correlationId));
                    //sw.Write(serialized.Length);
                    sw.Write(serialized);
                }
            }

            using (FileStream fs = File.Open(commitIdFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryWriter sw = new BinaryWriter(fs))
            {
                foreach (var index in indexes)
                {
                    sw.Write(index.PositionInStream);
                    //sw.Write(index.CorrelationId.Length);
                    sw.Write(index.CorrelationId);
                }
            }
        }

        #region FileNameHandling

        private string GetCorrelationFileName(String correlationId)
        {
            return Path.ChangeExtension(Path.Combine(_eventFolder, correlationId), ".events");
        }

        private string GetCommitIdFile(Guid commitId)
        {
            return Path.ChangeExtension(Path.Combine(_commitIdFolder, commitId.ToString("N")), ".commit");
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetByCorrelationId(string correlationId)
        {
            String correlationFileName = GetCorrelationFileName(correlationId);
            using (FileStream fs2 = File.Open(correlationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryReader br2 = new BinaryReader(fs2))
            {
                while (br2.BaseStream.Position < br2.BaseStream.Length)
                {
                    String serialized = br2.ReadString();
                    yield return JsonConvert.DeserializeObject<Event>(serialized, serializerSettings);
                }
            }
        }

        public IEnumerable<Event> GetByCommitId(Guid commitId)
        {
            String commitIdFile = GetCommitIdFile(commitId);
            using (FileStream fs = File.Open(commitIdFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    Int64 pos = br.ReadInt64();
                    String correlationId = br.ReadString();
                    String correlationFileName = GetCorrelationFileName(correlationId);
                    using (FileStream fs2 = File.Open(correlationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    using (BinaryReader br2 = new BinaryReader(fs2))
                    {
                        fs2.Seek(pos, SeekOrigin.Begin);
                        String serialized = br2.ReadString();
                        yield return JsonConvert.DeserializeObject<Event>(serialized, serializerSettings);
                    }
                }
            }
        }

        public void Dispose()
        {
            _lockFilestream.Close();
            File.Delete(GetLockFileName());

        }
    }

    public class Index
    {

        public Int64 PositionInStream { get; private set; }

        public String CorrelationId { get; private set; }

        public Index(Int64 positionInStream, String correlationId)
        {
            PositionInStream = positionInStream;
            CorrelationId = correlationId;
        }
    }
}
