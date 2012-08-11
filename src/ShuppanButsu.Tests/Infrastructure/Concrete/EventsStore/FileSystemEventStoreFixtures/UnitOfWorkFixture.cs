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
using ShuppanButsu.Infrastructure.Concrete;
using Rhino.Mocks;

namespace ShuppanButsu.Tests.Infrastructure.Concrete
{
    public class UnitOfWorkFixture
    {
        UnitOfWork sut;

        public UnitOfWorkFixture()
        {
            sut = new UnitOfWork(new InMemoryEventsStore(), MockRepository.GenerateStub<IDomainEventDispatcher>());
        }

        [Fact]
        public void verify_base_save_and_load() 
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            sut.Save(entity);
            sut.Commit(Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Id.Should().Be.EqualTo(entity.Id);
        }

        [Fact]
        public void verify_commit_will_now_modify_aggregateRoots_after_previous_commit_without_explicit_load() 
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            entity.Increment(10);
            sut.Save(entity);
            sut.Commit(Guid.NewGuid());

            //Now increment the entity, but the old unit of work was committed, this entity should not tracked anymore
            entity.Increment(32);
            sut.Commit(Guid.NewGuid());

            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.IntProperty.Should().Be.EqualTo(10);
        }

        [Fact]
        public void verify_save_get_modify_save()
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
            entity.Increment(10);
            sut.Save(entity);
            sut.Commit(Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Increment(32);
            sut.Commit(Guid.NewGuid());

            var reloaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            reloaded.IntProperty.Should().Be.EqualTo(42);
        }

        [Fact]
        public void verify_get_back_whole_stream_of_events()
        {
            TestClassForAggregateRoot entity = new TestClassForAggregateRoot();
                        sut.Save(entity);
            sut.Commit(Guid.NewGuid());
            var loaded = sut.GetById<TestClassForAggregateRoot>(entity.Id);
            loaded.Id.Should().Be.EqualTo(entity.Id);
        }
    }
}
