using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog;
using ShuppanButsu.Domain.Blog.CategoryManagerEvents;
using ShuppanButsu.Tests.Utils;
using Xunit;
using SharpTestsEx;

namespace ShuppanButsu.Tests.Domain.Blog
{
    [InterceptDomainEvents]
    public class CategoryManagerFixture : BaseTestFixtureWithHelper
    {
        CategoryManager sut;

        protected override void OnTestSetUp()
        {
            base.OnTestSetUp();
            sut = CategoryManager.Create(this.AggregateRootFactory(), "standard");
        }

        [Fact]
        public void Verify_generation_of_base_category() 
        {
            sut.AddCategory("category name", "desc", String.Empty);
            var categoryAdded = this.GetFirstDomainEvent<CategoryAdded>();

            categoryAdded.Name.Should().Be.EqualTo("category name");
            categoryAdded.Path.Should().Be.EqualTo("category name");
            categoryAdded.Slug.Should().Be.EqualTo("category-name");

        }
    }
}
