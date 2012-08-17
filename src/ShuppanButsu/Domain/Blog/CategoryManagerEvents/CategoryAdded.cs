using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Domain.Blog.CategoryManagerEvents
{
    public class CategoryAdded : DomainEvent
    {
        public String Name { get; private set; }
        public String Parent { get; private set; }
        public String Slug { get; private set; }
        public String Description { get; private set; }
        public String Path { get; private set; }
        public CategoryAdded(String name, String parent, String slug, string description, String path)
        {
            Name = name;
            Parent = parent;
            Slug = slug;
            Description = description;
            Path = path;
        }
    }
}
