using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HibernatingRhinos.Profiler.Appender;

namespace ShuppanButsu.Tests.Utils
{
    public class UseNhProfHelper : ITestHelper
    {

        public void SetUp(BaseTestFixture fixture)
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        }

        public void TearDown(BaseTestFixture fixture)
        {
            ProfilerInfrastructure.FlushAllMessages();
        }

        public int Priority
        {
            get { return 1; }
        }
    }

    public class UseNhProfAttribute : Attribute, ITestHelperAttribute
    {

        #region ITestHelperAttribute Members

        public ITestHelper Create()
        {
            return new UseNhProfHelper();
        }

        #endregion
    }
}
