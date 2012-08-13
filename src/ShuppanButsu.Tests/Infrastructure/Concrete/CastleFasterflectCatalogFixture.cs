using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTestsEx;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Infrastructure.Concrete;
using ShuppanButsu.Tests.Utils;
using ShuppanButsu.Tests.Utils.AutoMock;
using Xunit;

namespace ShuppanButsu.Tests.Infrastructure.Concrete
{
    [UseAutoMockingContainer(new Type[] { typeof(CastleFasterflectHandlerCatalog) })]
    public class CastleFasterflectHandlerCatalogFixture : BaseTestFixtureWithHelper
    {
        private CastleFasterflectHandlerCatalog sut;

        protected override void OnTestSetUp()
        {
            base.OnTestSetUp();
            sut = this.GetSut<CastleFasterflectHandlerCatalog>();
        }

          
        [Fact]
        public void Verify_that_assembly_are_scanned_correctly()
        {
            //Verify that I'm able to execute Testcommand
            sut.GetExecutorFor(typeof(TestCommand)).Should().Not.Be.Null();
        }

        [Fact]
        public void Verify_that_executors_really_call_the_method()
        {
            //Verify that I'm able to execute Testcommand
            var cmd = new TestCommand();
            sut.GetExecutorFor(typeof(TestCommand)).Invoke(cmd);
            cmd.CallCount.Should().Be.EqualTo(1);
        }

        [Fact]
        public void Verify_multiple_executor_classes_are_scanned_correctly()
        {
            //TestCommand1 is handled by an executor with two methods
            var cmd1 = new TestCommand1();
            sut.GetExecutorFor(typeof(TestCommand1)).Invoke(cmd1);
            cmd1.CallCount.Should().Be.EqualTo(1);

            var cmd2 = new TestCommand2();
            sut.GetExecutorFor(typeof(TestCommand2)).Invoke(cmd2);
            cmd2.CallCount.Should().Be.EqualTo(1);
        }

        [Fact]
        public void Verify_that_assembly_are_scanned_correctly_for_event_handler()
        {
            //Verify that I'm able to execute Testcommand
            var listOfHandlers = sut.GetAllHandlerFor(typeof(MyDomainEvent));
            listOfHandlers.Should().Have.Count.GreaterThan(0);
        }

        [Fact]
        public void Verify_that_handler_can_be_invoked_correctly()
        {
            //Verify that I'm able to execute Testcommand
            var listOfHandlers = sut.GetAllHandlerFor(typeof(MyDomainEvent));
            MyDomainEvent evt = new MyDomainEvent();
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(evt);
            }
            evt.CallCount.Should().Be.EqualTo(1);
        }

        [Fact]
        public void Verify_we_can_create_a_catch_event_from_base_type()
        {
            var listOfHandlers = sut.GetAllHandlerFor(typeof(MyBaseDomainEvent));
            MyBaseDomainEvent evtbase = new MyBaseDomainEvent();
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(evtbase);
            }
            evtbase.CallCount.Should().Be.EqualTo(1);

            MyDerivedDomainEvent evtderived = new MyDerivedDomainEvent();
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(evtderived);
            }
            evtderived.CallCount.Should().Be.EqualTo(1);
        }

        [Fact]
        public void Verify_handler_of_derived_class()
        {
            var listOfHandlers = sut.GetAllHandlerFor(typeof(MyDerivedDomainEvent));
            MyDerivedDomainEvent evtderived = new MyDerivedDomainEvent();
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(evtderived);
            }
            evtderived.CallCountSpecific.Should().Be.EqualTo(1);
        }

        [Fact]
        public void Verify_that_default_handler_is_singleton()
        {
            //get handler and call event
            var listOfHandlers = sut.GetAllHandlerFor(typeof(AnotherEvent));
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(new AnotherEvent());
            }
            Int32 actualCount = EventHandlerDefault.ConstructorCallCount;
            //call again
            foreach (var handler in listOfHandlers)
            {
                handler.Invoke(new AnotherEvent());
            }
            EventHandlerDefault.ConstructorCallCount.Should().Be.EqualTo(actualCount);
        }


    }

    #region Command helper classes

    public class TestCommand : ICommand
    {
        public Guid Id { get; set; }
        public Int32 CallCount { get; set; }
    }

    public class TestCommand1 : ICommand
    {
        public Guid Id { get; set; }
        public Int32 CallCount { get; set; }
    }

    public class TestCommand2 : ICommand
    {
        public Guid Id { get; set; }
        public Int32 CallCount { get; set; }
    }

    public class TestCommandHandler : ICommandExecutor
    {
        public void DoingTheTest(TestCommand testCommand)
        {
            testCommand.CallCount++;
        }
    }


    public class MultipleCommandHandler : ICommandExecutor
    {
        public void DoingTheTest(TestCommand1 testCommand)
        {
            testCommand.CallCount++;
        }

        public void ExecutingAnotherCommand(TestCommand2 testCommand)
        {
            testCommand.CallCount++;
        }
    }

    #endregion

    #region Domain Handler helper classes

    public class MyDomainEvent : DomainEvent { public Int32 CallCount { get; set; } }

    public class MyBaseDomainEvent : DomainEvent { public Int32 CallCount { get; set; } }

    public class MyDerivedDomainEvent : MyBaseDomainEvent { public Int32 CallCountSpecific { get; set; } }

    public class EventHandler1 : IDomainEventHandler
    {

        public void BariBari(MyDomainEvent evt)
        {

            evt.CallCount++;
        }

        public void Catch_derived_event(MyDerivedDomainEvent evt)
        {
            evt.CallCountSpecific++;
        }
    }

    public class EventHandlerBase : IDomainEventHandler
    {
        public void handle_can_call_me_whathever_u_wanna(MyBaseDomainEvent baseEvent)
        {
            baseEvent.CallCount++;
        }


    }

    public class AnotherEvent : DomainEvent { public Int32 CallCount { get; set; } }
    public class AnotherEvent2 : DomainEvent { public Int32 CallCount { get; set; } }

     

    public class EventHandlerDefault : IDomainEventHandler
    {
        public static Int32 ConstructorCallCount;

        public EventHandlerDefault()
        {
            ConstructorCallCount++;
        }

        public void Handle(AnotherEvent evt)
        {


        }
    }
    #endregion
}
