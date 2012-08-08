using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Tests.Domain.TestClasses
{
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

        public TestClassForAggregateRoot()
        {
            RaiseEvent(new TestClassForAggregateRootCreated()
            {
                Id = Guid.NewGuid(),
                IntProperty = 0,
                StringProperty = "0",
            });
        }

        public void Increment(Int32 quantity) 
        { 
            RaiseEvent(new TestClassIncremented() {Quantity = quantity});
        }

        private void Apply(TestClassIncremented evt) {

            IntProperty += evt.Quantity;
            StringProperty += evt.Quantity.ToString();
        }
    }

    class TestClassForAggregateRootCreated : DomainEvent
    {
        public Guid Id { get; set; }
        public Int32 IntProperty { get; set; }
        public String StringProperty { get; set; }
    }

    class TestClassIncremented : DomainEvent
    {
        public Int32 Quantity { get; set; }
    }
}
