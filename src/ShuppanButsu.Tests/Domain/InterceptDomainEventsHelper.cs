using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Tests.Utils;

namespace ShuppanButsu.Tests
{
    public class InterceptDomainEventsHelper : ITestHelper
    {
        public const string FactoryKey = "InterceptDomainEventsHelper_factory";
        public const string InterceptorListKey = "InterceptDomainEventsHelper_eventlist";
        public void SetUp(BaseTestFixture fixture)
        {
            List<DomainEvent> events = new List<DomainEvent>();
            fixture.SetIntoTestContext(InterceptorListKey, events);
            AggregateRootFactory factory = new AggregateRootFactory();
            factory.DomainEventInterceptor = new _Doe(events);
            fixture.SetIntoTestContext(FactoryKey, factory);
        }

        public void TearDown(BaseTestFixture fixture)
        {
            
        }

        public int Priority
        {
            get { return 1; }
        }

        private class _Doe : IDomainEventInterceptor
        {
            List<DomainEvent> Events = new List<DomainEvent>();

            public _Doe(List<DomainEvent> events) 
            {
                Events = events;
            }
            public void OnGenerated(DomainEvent @event)
            {
                Events.Add(@event);
            }
        }
    }

    public class InterceptDomainEventsAttribute : Attribute, ITestHelperAttribute
    {
        public ITestHelper Create()
        {
            return new InterceptDomainEventsHelper();
        }
    }

    public static class InterceptDomainEventsHelperExtensionMethods 
    {
        public static T CreateAggregate<T>(this BaseTestFixtureWithHelper fixture) where T : AggregateRoot
        {
            return fixture.GetFromTestContext<AggregateRootFactory>(InterceptDomainEventsHelper.FactoryKey).Create<T>();
        }

        public static IEnumerable<DomainEvent> GetEventsRaisedDuringCurrentTest(this BaseTestFixtureWithHelper fixture) 
        { 
            return fixture.GetFromTestContext<List<DomainEvent>>(InterceptDomainEventsHelper.InterceptorListKey);
        }

        public static AggregateRootFactory AggregateRootFactory(this BaseTestFixtureWithHelper fixture)
        {
            return fixture.GetFromTestContext<AggregateRootFactory>(InterceptDomainEventsHelper.FactoryKey);
        }
    }
}
