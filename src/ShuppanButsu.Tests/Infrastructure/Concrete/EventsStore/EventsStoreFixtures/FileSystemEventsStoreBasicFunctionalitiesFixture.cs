using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Infrastructure.Concrete.EventsStore;
using Xunit;
using SharpTestsEx;
using ShuppanButsu.Tests.Utils;
using System.IO;

namespace ShuppanButsu.Tests.Infrastructure.Concrete.EventsStore.EventsStoreFixtures
{
    public abstract class BaseEventStoreFixture : BaseTestFixtureWithHelper, IDisposable
    {

        IEventsStore sut;

        public BaseEventStoreFixture()
        {
            sut = GenerateSut();
        }

        protected abstract IEventsStore GenerateSut();


        public void Dispose()
        {
            if (sut is IDisposable) ((IDisposable)sut).Dispose();
        }

        [Fact]
        public void verify_smoke_save_of_a_single_event()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            Event evt = new Event(aPayload);
            sut.PersistEvents(new Event[] { evt }, Guid.NewGuid());
        }

        [Fact]
        public void verify_save_of_a_single_event()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            Event evt = new Event(aPayload, "CorrelationTest");
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt }, commitId);

            var commit = sut.GetByCommitId(commitId).ToList();
            commit.Should().Have.Count.EqualTo(1);
            var loadedEvt = commit.Single();
            loadedEvt.Payload.Should().Be.OfType<SamplePayload>();
            SamplePayload loadedPayload = (SamplePayload)loadedEvt.Payload;
            loadedEvt.Ticks.Should().Be.EqualTo(evt.Ticks);
            loadedEvt.CorrelationId.Should().Be.EqualTo(evt.CorrelationId);
            loadedPayload.StringProperty.Should().Be.EqualTo(aPayload.StringProperty);
            loadedPayload.DoubleProperty.Should().Be.EqualTo(aPayload.DoubleProperty);
        }

        [Fact]
        public void verify_save_of_Payload_with_private_setter_property()
        {
            SamplePayload aPayload = new SamplePayload(42);
            Event evt = new Event(aPayload, "CorrelationTest");
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt }, commitId);

            var commit = sut.GetByCommitId(commitId).ToList();
            commit.Select(e => (SamplePayload)e.Payload).Single().PrivateSetterProperty.Should().Be.EqualTo(42);
        }

        [Fact]
        public void verify_save_of_two_events()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            SamplePayload anotherPayload = new SamplePayload() { DoubleProperty = 134.34, StringProperty = "This is a another test" };
            Event evt = new Event(aPayload);
            Event anotherEvt = new Event(anotherPayload);
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt, anotherEvt }, commitId);

            var commit = sut.GetByCommitId(commitId).ToList();
            commit.Should().Have.Count.EqualTo(2);
            var loadedEvt = commit.First();
            loadedEvt.Payload.Should().Be.OfType<SamplePayload>();
            SamplePayload loadedPayload = (SamplePayload)loadedEvt.Payload;
            loadedPayload.StringProperty.Should().Be.EqualTo(aPayload.StringProperty);
            loadedPayload.DoubleProperty.Should().Be.EqualTo(aPayload.DoubleProperty);

            loadedEvt = commit.ElementAt(1);
            loadedEvt.Payload.Should().Be.OfType<SamplePayload>();
            loadedPayload = (SamplePayload)loadedEvt.Payload;
            loadedPayload.StringProperty.Should().Be.EqualTo(anotherPayload.StringProperty);
            loadedPayload.DoubleProperty.Should().Be.EqualTo(anotherPayload.DoubleProperty);
        }

        [Fact]
        public void verify_reload_by_correlation_id()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            var correlationId = Guid.NewGuid().ToString();
            Event evt = new Event(aPayload, correlationId);
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt }, commitId);

            var commit = sut.GetByCorrelationId(correlationId).ToList();
            commit.Should().Have.Count.EqualTo(1);
            var loadedEvt = commit.Single();
            loadedEvt.Payload.Should().Be.OfType<SamplePayload>();
            SamplePayload loadedPayload = (SamplePayload)loadedEvt.Payload;
            loadedPayload.StringProperty.Should().Be.EqualTo(aPayload.StringProperty);
            loadedPayload.DoubleProperty.Should().Be.EqualTo(aPayload.DoubleProperty);
        }

        [Fact]
        public void verify_reload_of_all_events_ordered()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 1 };
            SamplePayload anotherPayload = new SamplePayload() { DoubleProperty = 2 };
            Event evt = new Event(aPayload);
            Event anotherEvt = new Event(anotherPayload);
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt, anotherEvt }, commitId);


            aPayload = new SamplePayload() { DoubleProperty = 3 };
            anotherPayload = new SamplePayload() { DoubleProperty = 4 };
            evt = new Event(aPayload);
            anotherEvt = new Event(anotherPayload);
            commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt, anotherEvt }, commitId);


            var reloaded = sut.GetRange(0L, Int64.MaxValue);
            reloaded.Should().Have.Count.EqualTo(4);
            reloaded.Select(e => ((SamplePayload)e.Payload).DoubleProperty)
                .Should().Have.SameSequenceAs(new double[] { 1, 2, 3, 4 });
        }

        [Fact]
        public void verify_cannot_save_more_than_one_commitid()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            Event evt = new Event(aPayload);
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt }, commitId);
            (new Action(() => sut.PersistEvents(new Event[] { evt }, commitId))).Should().Throw();
        }
    }

    public class SamplePayload
    {

        public String StringProperty { get; set; }

        public Double DoubleProperty { get; set; }

        public Int32 PrivateSetterProperty { get; private set; }

        public SamplePayload() { }

        public SamplePayload(Int32 privateSetterProperty)
        {
            PrivateSetterProperty = privateSetterProperty;
        }
    }

    public class FileSystemEventsStoreFixture : BaseEventStoreFixture
    {
        protected override IEventsStore GenerateSut()
        {
            Console.WriteLine("FileSystemEventsStoreFixture");
            var dirpath = "c:\\temp\\testevents";
            if (System.IO.Directory.Exists(dirpath))
            {
                System.IO.Directory.Delete(dirpath, true);
            }
            return new FileSystemEventsStore(dirpath);
        }
    }

    [UseNhProf]
    public class SqliteEventsStoreFixture : BaseEventStoreFixture
    {
        protected override IEventsStore GenerateSut()
        {
            //Delete the eventual existing db
            if (File.Exists("eventstore.db")) File.Delete("eventstore.db");
            return new SqlEventsStore(@"Infrastructure\Concrete\EventsStore\EventsStoreFixtures\SampleNhEventStoreConfiguration.xml");
        }
    }
}
