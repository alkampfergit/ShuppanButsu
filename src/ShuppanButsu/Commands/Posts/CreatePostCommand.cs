using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Commands.Posts
{
    public class CreatePostCommand : CommandBase
    {
        public String Title { get; set; }
        public String Content { get; set; }
        public String Excerpt { get; set; }
        public String BlogName { get; set; }
        //public DateTime PublicationDate { get; set; }

    }
}
