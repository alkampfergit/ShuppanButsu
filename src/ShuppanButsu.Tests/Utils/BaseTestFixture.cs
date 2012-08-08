using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShuppanButsu.Tests.Utils
{
    public abstract class BaseTestFixture : IDisposable
    {

        #region Initialization and private

        private List<IDisposable> disposableList;
        private List<Action> disposableActions;

        public BaseTestFixture()
        {
            disposableList = new List<IDisposable>();
            disposableActions = new List<Action>();
            try
            {
                OnTestConstructor();
                OnTestSetUp();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error during fixture setup {0}", ex);
                throw;
            }
        }

        protected virtual void OnTestSetUp() { 
        
        }

        protected virtual void OnTestConstructor()
        {
        }

        public void Dispose()
        {
            Boolean ErrorOnDispose = false;
            disposableList.ForEach(d =>
            {
                try
                {
                    d.Dispose();
                }
                catch (Exception)
                {
                    ErrorOnDispose = true;
                }
            });
            Boolean ErrorOnTearDownAction = false;
            disposableActions.ForEach(a =>
            {
                try
                {
                    a();
                }
                catch (Exception)
                {
                    ErrorOnTearDownAction = true;
                }
            });
            OnDisposing();
            Assert.False(ErrorOnDispose, "Some disposable object generates errors during Fixture Tear Down");
            Assert.False(ErrorOnTearDownAction, "Some tear down action generates errors during Fixture Tear Down");
        }

        protected virtual void OnDisposing()
        {

        }


        #endregion

        #region Cleanup management

        public void DisposeAtTheEndOfTest(IDisposable disposableObject)
        {
            disposableList.Add(disposableObject);
        }

        public void ExecuteAtTheEndOfTest(Action action)
        {
            disposableActions.Add(action);
        }

        #endregion

        #region Context

        private Dictionary<String, object> TestContext = new Dictionary<string, object>();
        public void SetIntoTestContext(String key, Object value)
        {
            if (!TestContext.ContainsKey(key))
                TestContext.Add(key, value);
            else
                TestContext[key] = value;
        }

        public T GetFromTestContext<T>(String key)
        {
            return (T)TestContext[key];
        }

        #endregion
    }

    public abstract class BaseTestFixtureWithHelper : BaseTestFixture
    {

        protected List<ITestHelper> _helpers = new List<ITestHelper>();

        protected IEnumerable<ITestHelper> Helpers { get { return _helpers.OrderByDescending(h => h.Priority); } }
        #region Rhino mocks helper

        private Attribute[] customAttributes;


        #endregion

        protected sealed override void OnTestConstructor()
        {
            Type type = this.GetType();
            customAttributes = Attribute.GetCustomAttributes(type);
            foreach (ITestHelperAttribute attribute in
                customAttributes.OfType<ITestHelperAttribute>())
            {
                _helpers.Add(attribute.Create());
            }
        }

        protected override void OnTestSetUp()
        {
            foreach (var helper in Helpers) helper.SetUp(this);
            base.OnTestConstructor();
        }


        protected override void OnDisposing()
        {
            foreach (var helper in Helpers) helper.TearDown(this);
            base.OnDisposing();
        }
    }

    /// <summary>
    /// A test helper is an object that can be used to interact
    /// with the test
    /// </summary>
    public interface ITestHelper
    {
        void SetUp(BaseTestFixture fixture);
        void TearDown(BaseTestFixture fixture);

        /// <summary>
        /// 
        /// </summary>
        Int32 Priority { get; }
    }

    public interface ITestHelperAttribute
    {
        ITestHelper Create();
    }
}
