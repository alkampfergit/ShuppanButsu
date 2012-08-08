﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain;
using ShuppanButsu.Infrastructure;
using Xunit;
using SharpTestsEx;

namespace ShuppanButsu.Tests.Domain
{
    public class AggregateRootFixture
    {
        [Fact]
        public void Verify_basic_ability_of_event_reapplier() 
        {
            TestClassForAggregateRoot sut = new TestClassForAggregateRoot();
            var evt = new TestClassForAggregateRootCreated() { 
                Id = Guid.NewGuid(),
                IntProperty = 42,
                StringProperty = "42",
            };
            ((IAggregateRoot)sut).ApplyEvent(evt);
            sut.IntProperty.Should().Be.EqualTo(42);
            sut.StringProperty.Should().Be.EqualTo("42");
            sut.Id.Should().Be.EqualTo(evt.Id);
        }

        [Fact]
        public void Verify_generation_of_events() 
        {
            TestClassForAggregateRoot sut = new TestClassForAggregateRoot();
            var evts = ((IAggregateRoot)sut).GetRaisedEvents();
            evts.Should().Have.Count.EqualTo(1);
            evts.Single().Should().Be.OfType<TestClassForAggregateRootCreated>();
        }

        [Fact]
        public void verify_clear_of_events() 
        {
            TestClassForAggregateRoot sut = new TestClassForAggregateRoot();
            ((IAggregateRoot)sut).ClearRaisedEvents();
            var evts = ((IAggregateRoot)sut).GetRaisedEvents();
            evts.Should().Have.Count.EqualTo(0);
        }
    }

    class TestClassForAggregateRoot : AggregateRoot
    {

        public Int32 IntProperty;
        public String StringProperty;

        private void Apply(TestClassForAggregateRootCreated @event)
        {
            Id = @event.Id;
            IntProperty = @event.IntProperty;
            StringProperty = @event.StringProperty;
        }

        public  TestClassForAggregateRoot() 
        { 
            RaiseEvent(new TestClassForAggregateRootCreated() { 
                Id = Guid.NewGuid(),
                IntProperty = 0,
                StringProperty = "0",
            });
        }

    }

    class TestClassForAggregateRootCreated : DomainEvent
    {
        public Guid Id { get; set; }
        public Int32 IntProperty { get; set; }
        public String StringProperty { get; set; }
    }
}