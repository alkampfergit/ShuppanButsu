using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog.Handlers.PageManagers;
using ShuppanButsu.Domain.Blog.PostEvents;
using ShuppanButsu.Tests.Utils;
using Xunit;
using SharpTestsEx;
using Castle.Core.Logging;

namespace ShuppanButsu.Tests.Domain.Blog.Handlers.PageManagersTests
{
    /// <summary>
    /// Fixture for the <see cref="HomePageManager"/> class.
    /// </summary>
    public class HomePageManagerFixture : BaseTestFixtureWithHelper
    {
        HomePageManager sut;
        protected override void OnTestSetUp()
        {
            base.OnTestSetUp();
            sut = new HomePageManager();
            sut.Configuration = new ShuppanButsuConfiguration
            ( 
                "Domain\\Blog\\Handlers\\PageManagersTests\\TestTemplates"
            ); 
            sut.Logger = new TestLogger(LoggerLevel.Error);
            if (File.Exists("index.html")) File.Delete("index.html");
        }

        [Fact]
        public void Verify_basic_home_page_creation_for_blog_in_the_root() 
        { 
            sut.PostCreatedHandler(new PostCreated("The Title", "Content", "the-title", "", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("index.html"));
            String outputFile = File.ReadAllText("index.html");
            //Verify that there is a span with the title content.
            outputFile.Should().Contain("<span class=\"title\">The Title</span>");
            //verify that there is a span with the excerpt
            outputFile.Should().Contain("<p class=\"excerpt\">The excerpt</p>");
        }
    }
}
