using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Commands.Posts;
using ShuppanButsu.Domain.Blog;
using ShuppanButsu.Infrastructure;
using ShuppanButsu.Infrastructure.BaseClasses;
using ShuppanButsu.Infrastructure.Concrete;

namespace ShuppanButsu.CommandsExecutors.Posts
{
    public class PostBaseExecutors : BaseUnitOfWorkExecutor
    {
        public PostBaseExecutors(UnitOfWork uow) : base(uow) { }

        public void CreatePost(CreatePostCommand command) 
        {
            _uow.Save( Post.CreatePost(new AggregateRootFactory(),
                command.Title,
                command.Content,
                command.Excerpt,
                command.BlogName));
            _uow.Commit(command.Id);
        }
    }
}
