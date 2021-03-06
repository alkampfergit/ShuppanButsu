﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Utils;

namespace ShuppanButsu.Domain.Blog.PostEvents
{
    public class PostCreated : AggregateRootCreationDomainEvent
    {
        public String Title { get; private set; }

        public String Content { get; private set; }

        public String SlugCode { get; private set; }

        public String BlogName { get; private set; }

        public String Excerpt { get; set; }

        public PostCreated(String title, String content, String slugCode, String blogName, String excerpt) : base(Guid.NewGuid().ToAggregateRootId())
        {
            Title = title;
            Content = content;
            SlugCode = slugCode;
            BlogName = blogName;
            Excerpt = excerpt;
        }
    }

}
