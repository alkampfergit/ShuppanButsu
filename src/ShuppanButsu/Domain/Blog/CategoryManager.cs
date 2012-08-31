using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog.CategoryManagerEvents;
using ShuppanButsu.Utils;

namespace ShuppanButsu.Domain.Blog
{
    public class CategoryManager : EventSourcingBasedEntity
    {
        List<Category> categories = new List<Category>() { new Category() {Name = ""}};

        private CategoryManager() { }

        public static CategoryManager Create(AggregateRootFactory factory, String id) 
        {
            return factory.Create<CategoryManager>();
        }

        public void AddCategory(String categoryName, String categoryDescription, String parentCategory) 
        {
            String slug = categoryName.Slugify();
            if (!String.IsNullOrEmpty(parentCategory)) 
            { 
                //Verify parent category exists.
                if (!categories.Any(c => c.Name.Equals(parentCategory, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException("There is not category named " + parentCategory + " to be used as parent", "parentCategory");
                }
            }
            //Verify no category with the same name in the system
            if (categories.Any(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))) 
            {
                throw new ArgumentException("There is already a category named " + categoryName, "categoryName");
            }

            Category parent = null;
            if (!String.IsNullOrEmpty(parentCategory)) 
            {
                parent = categories
                    .Where(c => c.Name.Equals(parentCategory, StringComparison.OrdinalIgnoreCase))
                    .Single();
            }
            String path = parent == null ? categoryName : parent.Path + "/" + categoryName;
            String parentName = parent == null ? String.Empty : parent.Name;
            CategoryAdded evt = new CategoryAdded(categoryName,  parentName, slug, categoryDescription, path);

            RaiseEvent(evt);
        }

        private void Apply(CategoryAdded evt) 
        {
            Category newCategory = new Category()
            {
                Name = evt.Name,
                Description = evt.Description,
                Slug = evt.Slug,
                Parent = categories.Single(c => c.Name.Equals(evt.Parent, StringComparison.OrdinalIgnoreCase)),
                Path = evt.Path,
            };

            categories.Add(newCategory);
        }

        /// <summary>
        /// private class to take care of categories.
        /// </summary>
        private class Category {
            public String Name { get; set; }
            public Category Parent { get; set; }
            public String Slug { get; set; }
            public String Description { get; set; }
            public String Path { get; set; }
        }
    }


}
