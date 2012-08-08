using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Castle.MicroKernel.Registration;

namespace ShuppanButsu.Tests.Utils.AutoMock
{
    public static class AutoMockingContainerExtMethods
    {

        public static AutoMockingContainer AutoMockingContainer(this BaseTestFixture fixture)
        {
            return
                fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.ContainerKey);
        }

        public static T ResolveWithAutomock<T>(this BaseTestFixture fixture)
        {
            var container =
                fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.ContainerKey);
            return container.Resolve<T>();
        }

        public static T GetSut<T>(this BaseTestFixture fixture) 
        {
            return fixture.GetFromTestContext<T>("autoMock_" + typeof(T).FullName);
        }

        public static T ResolveWithAutomock<T>(this BaseTestFixture fixture, IDictionary arguments)
        {
            var container =
                fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.ContainerKey);
            return container.Resolve<T>(arguments);
        }

        public static T GetMock<T>(this BaseTestFixture fixture)
        {
            var container =
                fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.ContainerKey);
            return container.GetFirstCreatedMock<T>();
        }
    }

    public class AutoMockingContainerHelper : ITestHelper
    {
        internal const String ContainerKey = "AutoMockingContainerHelper_container";

        public Type[] Types { get; set; }

        public string[] IgnoreDependencies { get; set; }

        public bool ResolveProperties { get; set; }

        #region ITestHelper Members

        public int Priority
        {
            get { return 1; }
        }

         public void SetUp(BaseTestFixture fixture)
        {
            AutoMockingContainer container = new AutoMockingContainer();
            fixture.DisposeAtTheEndOfTest(container);
            fixture.SetIntoTestContext(ContainerKey, container);
            foreach (var type in Types)
            {
                container.Register(Component
                    .For(type)
                    .ImplementedBy(type)
                    .LifeStyle.Transient);
            }
            container.ResolveProperties = ResolveProperties;
            if (IgnoreDependencies != null)
            {
                foreach (var ignoreDependency in IgnoreDependencies)
                {
                    container.DependencyToIgnore.Add(ignoreDependency);
                }
            }
            foreach (var type in Types)
            {
                fixture.SetIntoTestContext("autoMock_" + type.FullName, container.Resolve(type));
            }
            
        }

        public void TearDown(BaseTestFixture fixture)
        {

        }

        #endregion
    }

    public class UseAutoMockingContainerAttribute : Attribute, ITestHelperAttribute
    {

        private Type[] Types { get; set; }

        public UseAutoMockingContainerAttribute(Type[] types)
        {
            Types = types;
            ResolveProperties = true;
        }

        #region ITestHelperAttribute Members

        public ITestHelper Create()
        {
            AutoMockingContainerHelper autoMockingContainerHelper = new AutoMockingContainerHelper() { Types = this.Types };
            autoMockingContainerHelper.IgnoreDependencies = IgnoreDependencies;
            autoMockingContainerHelper.ResolveProperties = ResolveProperties;
            return autoMockingContainerHelper;
        }

        public String[] IgnoreDependencies { get; set; }

        public Boolean ResolveProperties { get; set; }
        #endregion
    }

}
