using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog;
using ShuppanButsu.Domain.Blog.PostEvents;
using ShuppanButsu.Tests.Utils;
using Xunit;
using SharpTestsEx;

namespace ShuppanButsu.Tests.Domain.Blog.PostTests
{
    /// <summary>
    /// Basic fixture for the <see cref="Post" /> Class
    /// </summary>
    [InterceptDomainEvents]
    public class BasicPostFixture : BaseTestFixtureWithHelper
    {

        [Fact]
        public void Verify_calculation_of_slugId_during_post_creation() 
        {
            Post post =  Post.CreatePost(this.AggregateRootFactory(), "this is a title", "Anycontent");
            this.GetEventsRaisedDuringCurrentTest()
                .OfType<PostCreated>()
                .Single()
                .SlugCode.Should().Be.EqualTo("this-is-a-title");
        }
    }
}
