using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Infrastructure.Concrete.EventsStore;
using Xunit;
using SharpTestsEx;

namespace ShuppanButsu.Tests.Infrastructure.Concrete.EventsStore.FileSystemEventStoreFixtures
{
    public class FileSystemEventsStoreBasicFunctionalitiesFixture : IDisposable
    {

        FileSystemEventsStore sut;

        public FileSystemEventsStoreBasicFunctionalitiesFixture() 
        {
            sut = new FileSystemEventsStore("c:\\temp");
        }

        public void Dispose() 
        {
            sut.Dispose();
        }

        [Fact]
        public void verify_smoke_save_of_a_single_event() 
        { 
            SamplePayload aPayload = new SamplePayload() {DoubleProperty = 10.34, StringProperty = "This is a test"};
            Event evt = new Event(aPayload);
            sut.PersistEvents(new Event[] { evt }, Guid.NewGuid());
        }

        [Fact]
        public void verify_save_of_a_single_event()
        {
            SamplePayload aPayload = new SamplePayload() { DoubleProperty = 10.34, StringProperty = "This is a test" };
            Event evt = new Event(aPayload);
            Guid commitId = Guid.NewGuid();
            sut.PersistEvents(new Event[] { evt },commitId);

            var commit = sut.GetByCommitId(commitId).ToList();
            commit.Should().Have.Count.EqualTo(1);
            var loadedEvt = commit.Single();
            loadedEvt.Payload.Should().Be.OfType<SamplePayload>();
            SamplePayload loadedPayload = (SamplePayload) loadedEvt.Payload;
            loadedPayload.StringProperty.Should().Be.EqualTo(aPayload.StringProperty);
            loadedPayload.DoubleProperty.Should().Be.EqualTo(aPayload.DoubleProperty);
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
    }

    public class SamplePayload 
    {

        public String StringProperty { get; set; }

        public Double DoubleProperty { get; set; }
    }
}
