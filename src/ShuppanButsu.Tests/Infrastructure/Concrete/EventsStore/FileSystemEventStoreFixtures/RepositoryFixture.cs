using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Tests.Domain.TestClasses;
using ShuppanButsu.Tests.Utils;
using Xunit;
using SharpTestsEx;

namespace ShuppanButsu.Tests.Infrastructure.Concrete
{
    public class RepositoryFixture
    {
        Repository sut;

        public RepositoryFixture()
        {
            sut = new Repository(new InMemoryEventsStore());
        }

        [Fact]
        public void verify_base_save_and_load() 
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            sut.Save(entity, Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Id.Should().Be.EqualTo(entity.Id);
        }

        [Fact]
        public void verify_double_save_does_not_duplicate_events() 
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            entity.Increment(10);
            sut.Save(entity, Guid.NewGuid());
            entity.Increment(32);
            sut.Save(entity, Guid.NewGuid());

            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.IntProperty.Should().Be.EqualTo(42);
        }

        [Fact]
        public void verify_save_get_modify_save()
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            entity.Increment(10);
            sut.Save(entity, Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Increment(32);
            sut.Save(loaded, Guid.NewGuid());

            var reloaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            reloaded.IntProperty.Should().Be.EqualTo(42);
        }

        [Fact]
        public void verify_get_back_whole_stream_of_events()
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            sut.Save(entity, Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Id.Should().Be.EqualTo(entity.Id);
        }
    }
}
