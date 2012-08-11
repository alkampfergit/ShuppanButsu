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
        String _eventStreamFileName;

        JsonSerializerSettings serializerSettings;

        public FileSystemEventsStore(String baseDirectory)
        {
            _baseDirectory = baseDirectory;
            if (!Directory.Exists(_baseDirectory)) Directory.CreateDirectory(_baseDirectory);

            //TODO: Try to understand if the directory is locked by someone else and do a graceful error message
            _lockFilestream = File.Open(GetLockFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            StreamWriter wr = new StreamWriter(_lockFilestream);
            wr.Write(Environment.MachineName + " pid=" + Process.GetCurrentProcess().Id);
            wr.Close();

            _commitIdFolder = Path.Combine(baseDirectory, "Commits");
            if (!Directory.Exists(_commitIdFolder)) Directory.CreateDirectory(_commitIdFolder);

            _eventFolder = Path.Combine(baseDirectory, "Events");
            if (!Directory.Exists(_eventFolder)) Directory.CreateDirectory(_eventFolder);

            _eventStreamFileName = Path.Combine(baseDirectory, "events.stream");

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

            var indexes = new List<Index>();

            //now write all the streams and the relative position in files
            using (FileStream fs = File.Open(_eventStreamFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryWriter sw = new BinaryWriter(fs))
            {
                foreach (var @event in domainEvents)
                {
                    String correlationId = String.IsNullOrWhiteSpace(@event.CorrelationId) ? "null" : @event.CorrelationId;

                    String serialized = JsonConvert.SerializeObject(@event, serializerSettings);

                    fs.Seek(0, SeekOrigin.End);
                    //store index information
                    indexes.Add(new Index(fs.Position, correlationId));

                    sw.Write(serialized);
                }
            }

            //Write all information about the index in a file with the name of the correlation id
            using (FileStream fs = File.Open(commitIdFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryWriter sw = new BinaryWriter(fs))
            {
                foreach (var index in indexes)
                {
                    sw.Write(index.PositionInStream);

                    //write information about the index in the correlation file index
                    String correlationFileName = GetCorrelationFileName(index.CorrelationId);
                    using (FileStream fscorr = File.Open(correlationFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                    using (BinaryWriter swcorr = new BinaryWriter(fscorr))
                    {
                        fscorr.Seek(0, SeekOrigin.End);
                        swcorr.Write(index.PositionInStream);
                    }
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
            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(correlationFileName)))
            using (BinaryReader br = new BinaryReader(ms))
            using (FileStream fs2 = File.Open(_eventStreamFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryReader br2 = new BinaryReader(fs2))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    Int64 index = br.ReadInt64();
                    fs2.Seek(index, SeekOrigin.Begin);
                    String serialized = br2.ReadString();
                    yield return JsonConvert.DeserializeObject<Event>(serialized, serializerSettings);
                }
            }
        }

        public IEnumerable<Event> GetByCommitId(Guid commitId)
        {
            String commitIdFile = GetCommitIdFile(commitId);

            using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(commitIdFile)))
            using (BinaryReader br = new BinaryReader(ms))
            using (FileStream fs2 = File.Open(_eventStreamFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryReader br2 = new BinaryReader(fs2))
            {
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    Int64 index = br.ReadInt64();
                    fs2.Seek(index, SeekOrigin.Begin);
                    String serialized = br2.ReadString();
                    yield return JsonConvert.DeserializeObject<Event>(serialized, serializerSettings);
                }
            }
        }

        public void Dispose()
        {
            _lockFilestream.Close();
            File.Delete(GetLockFileName());

        }


        public IEnumerable<Event> GetRange(long tickFrom, long tickTo)
        {
            using (FileStream fs = File.Open(_eventStreamFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            using (BinaryReader br = new BinaryReader(fs))
            {
                fs.Seek(0, SeekOrigin.Begin);
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    String serialized = br.ReadString();
                    yield return JsonConvert.DeserializeObject<Event>(serialized, serializerSettings);
                }
            }
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
