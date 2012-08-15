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
            ).WithMaxNumberOfPostsInHomePage(3); 
            sut.Logger = new TestLogger(LoggerLevel.Error);
            if (File.Exists("index.html")) File.Delete("index.html");
            if (File.Exists("subblog\\index.html")) File.Delete("subblog\\index.html");
            if (File.Exists("nonexistenttemplate\\index.html")) File.Delete("nonexistenttemplate\\index.html");
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

        [Fact]
        public void Verify_basic_home_page_when_we_have_two_posts()
        {
            sut.PostCreatedHandler(new PostCreated("The Title1", "Content1", "the-title1", "", "The excerpt1"));
            sut.PostCreatedHandler(new PostCreated("The Title2", "Content2", "the-title2", "", "The excerpt2"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("index.html"));
            String outputFile = File.ReadAllText("index.html");
            //Verify that there is a span with the title content.
            outputFile.Should().Contain("<span class=\"title\">The Title1</span>");
            //verify that there is a span with the excerpt
            outputFile.Should().Contain("<p class=\"excerpt\">The excerpt1</p>");

            //Verify that there is a span with the title content for second post
            outputFile.Should().Contain("<span class=\"title\">The Title2</span>");
            //verify that there is a span with the excerpt for second post
            outputFile.Should().Contain("<p class=\"excerpt\">The excerpt2</p>");
        }

        [Fact]
        public void Verify_basic_home_page_with_more_than_allowed_post_in_home_page()
        {
            sut.PostCreatedHandler(new PostCreated("The Title1", "Content1", "the-title1", "", "The excerpt1"));
            sut.PostCreatedHandler(new PostCreated("The Title2", "Content2", "the-title2", "", "The excerpt2"));
            sut.PostCreatedHandler(new PostCreated("The Title3", "Content3", "the-title3", "", "The excerpt3"));
            sut.PostCreatedHandler(new PostCreated("The Title4", "Content4", "the-title4", "", "The excerpt4"));

            Assert.True(File.Exists("index.html"));
            String outputFile = File.ReadAllText("index.html");
            //Verify that the first post is not present anymore because we have more than three posts
            outputFile.Should().Not.Contain("<span class=\"title\">The Title1</span>");
            //verify that there is a span with the excerpt
            outputFile.Should().Not.Contain("<p class=\"excerpt\">The excerpt1</p>");

            //Verify that there is a span with the title content for second post
            outputFile.Should().Contain("<span class=\"title\">The Title4</span>");
            //verify that there is a span with the excerpt for second post
            outputFile.Should().Contain("<p class=\"excerpt\">The excerpt4</p>");
        }

        [Fact]
        public void Verify_basic_home_page_creation_for_a_subblog() 
        {
            sut.PostCreatedHandler(new PostCreated("The Title", "Content", "the-title", "subblog", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("subblog\\index.html"));
            String outputFile = File.ReadAllText("subblog\\index.html");
            //Verify that there is a span with the title content.
            outputFile.Should().Contain("<span class=\"subblogtitle\">The Title</span>");
            //verify that there is a span with the excerpt
            outputFile.Should().Contain("<p class=\"subblogexcerpt\">The excerpt</p>");
        }

        [Fact]
        public void Verify_subblog_with_no_template_use_default_root_template()
        {
            sut.PostCreatedHandler(new PostCreated("The Title", "Content", "the-title", "nonexistenttemplate", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("nonexistenttemplate\\index.html"));
            String outputFile = File.ReadAllText("nonexistenttemplate\\index.html");
            //Verify that there is a span with the title content.
            outputFile.Should().Contain("<span class=\"title\">The Title</span>");
            //verify that there is a span with the excerpt
            outputFile.Should().Contain("<p class=\"excerpt\">The excerpt</p>");
        }

        [Fact]
        public void Verify_file_name_in_child_directory()
        {
            sut.Configuration.WithBaseGenerationDirectory("testdir");
            sut.PostCreatedHandler(new PostCreated("The Title", "Content", "the-title", "", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("testdir\\index.html"));
           
        }

        [Fact]
        public void Verify_file_name_in_child_directory_for_child_blog()
        {
            sut.Configuration.WithBaseGenerationDirectory("testdir");
            sut.PostCreatedHandler(new PostCreated("The Title", "Content", "the-title", "subblog", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("testdir\\subblog\\index.html"));

        }
    }
}
