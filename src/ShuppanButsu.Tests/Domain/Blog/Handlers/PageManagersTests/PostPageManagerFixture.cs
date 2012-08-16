﻿using System;
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
    public class PostPageManagerFixture : BaseTestFixtureWithHelper
    {
        PostPageManager sut;

        protected override void OnTestSetUp()
        {
            base.OnTestSetUp();
            sut = new PostPageManager();
            sut.Configuration = new ShuppanButsuConfiguration
            (
                "Domain\\Blog\\Handlers\\PageManagersTests\\TestTemplates"
            ).WithBaseGenerationDirectory("GenerationDir");
            sut.Logger = new TestLogger(LoggerLevel.Error);
            //Clear all file eventually generated by such a test. 
            foreach (var generatedFile in Directory.EnumerateFiles(".", "the-title*.*", SearchOption.AllDirectories) )
            {
                File.Delete(generatedFile);
            }
        }

        [Fact]
        public void Verify_creation_of_post_file()
        {
            sut.CreatePostHtmlDocument(new PostCreated("The Title", "Content", "the-title", "", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            Assert.True(File.Exists("GenerationDir\\the-title.html"));
        }

        [Fact]
        public void Verify_content_of_post_file()
        {
            sut.CreatePostHtmlDocument(new PostCreated("The Title", "Content", "the-title", "", "The excerpt"));

            //verify that a file exists and gets created with the expected result.
            String outputFile = File.ReadAllText("GenerationDir\\the-title.html");
            //Verify that there is a span with the title content.
            outputFile.Should().Contain("<h1 class=\"title\" id=\"{title}\">The Title</h1>");
            //verify that there is a span with the excerpt
            outputFile.Should().Contain("<p class=\"content\" id=\"{content}\">Content</p>");
        }
    }
}