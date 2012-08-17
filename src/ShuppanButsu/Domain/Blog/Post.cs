using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog.PostEvents;
using ShuppanButsu.Utils;

namespace ShuppanButsu.Domain.Blog
{
    /// <summary>
    /// This class represent a post in a blog.
    /// </summary>
    public class Post : AggregateRoot
    {
        private String content;
        private String title;
        private String slugCode;
        private String blogName;

        private Post() { }

        /// <summary>
        /// Factory method to create a post
        /// </summary>
        /// <param name="factory">The factory to be used for AggregateRootCreation</param>
        /// <param name="title"></param>
        /// <param name="textContent"></param>
        /// <returns></returns>
        public static Post CreatePost(AggregateRootFactory factory, String title, String textContent, String blogName) 
        { 
            //Slug is created replacing any non number or letter char with a dash
            //accents are removed
            String slug = title.Slugify();
            var evt = new PostCreated(title, textContent, slug, blogName, textContent);
            return factory.Create<Post>(evt);
        }

        private void Apply(PostCreated @event)
        {
            content = @event.Content;
            title = @event.Title;
            slugCode = @event.SlugCode;
            blogName = @event.BlogName;
        }
    }

   
}
