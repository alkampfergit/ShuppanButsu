using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Domain.Blog.PostEvents;

namespace ShuppanButsu.Domain.Blog
{
    public class Post : AggregateRoot
    {
        private String content;
        public String title { get; set; }
        public String slugCode { get; set; }

        private Post() { }


        public static Post CreatePost(AggregateRootFactory factory, String title, String textContent) 
        { 

            //Slug is created replacing any non number or letter char with a dash
            StringBuilder slug = title.Aggregate(new StringBuilder(), (sb, c) => Char.IsLetterOrDigit(c) ? sb.Append(c) : sb.Append("-"));
            var evt = new PostCreated(title, textContent, slug.ToString());
            return factory.Create<Post>(evt);
        }

        private void Apply(PostCreated @event)
        {
            content = @event.Content;
            title = @event.Title;
            slugCode = @event.SlugCode;
        }
    }

   
}
