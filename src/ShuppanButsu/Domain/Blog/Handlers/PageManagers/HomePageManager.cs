using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using ShuppanButsu.Domain.Blog.PostEvents;
using ShuppanButsu.Infrastructure;

namespace ShuppanButsu.Domain.Blog.Handlers.PageManagers
{
    /// <summary>
    /// This is the component that needs to manage the creation of home page 
    /// of each single blog. 
    /// </summary>
    public class HomePageManager : BaseDomainEventHandler
    {
        private List<PostExtract> postsInHomePage = new List<PostExtract>();

        public void PostCreatedHandler(PostCreated evt) 
        {
            HtmlDocument doc = new HtmlDocument();
            String templateFileName = Path.Combine(Configuration.TemplateDirectory, "index.html");
            FileInfo finfo = new FileInfo(templateFileName);
            if (!finfo.Exists) 
            {
                Logger.Fatal("Template file missing: " + finfo.FullName);
                return;
            }
             
            doc.LoadHtml(File.ReadAllText(finfo.FullName));
            //now find the div with the template of the excerpt
            var templateOfExcerpt = doc.DocumentNode.SelectSingleNode("//*[@id='{posttemplate}']");
            if (templateOfExcerpt == null)
            {
                Logger.Fatal("The template in file " + finfo.FullName + " has not {posttemplate} element to use. The template is malformed");
                return;
            }

            //remove the id attribute, it is just noise in the file.
            templateOfExcerpt.Attributes["id"].Remove();
            //now we have template to modify.
            var titleNode = templateOfExcerpt.SelectSingleNode(".//*[@id='{title}']");
            if (titleNode == null)
            {
                Logger.Fatal("The template in file " + finfo.FullName + " has not {title} element to use. The template is malformed");
                return;
            }
            titleNode.Attributes["id"].Remove();
            titleNode.RemoveAllChildren();
            titleNode.AppendChild(HtmlTextNode.CreateNode(evt.Title));


            var excerptNode = templateOfExcerpt.SelectSingleNode(".//*[@id='{excerpt}']");
            if (excerptNode == null)
            {
                Logger.Fatal("The template in file " + finfo.FullName + " has not {excerpt} element to use. The template is malformed");
                return;
            }
            excerptNode.Attributes["id"].Remove();
            excerptNode.RemoveAllChildren();
            excerptNode.AppendChild(HtmlTextNode.CreateNode(evt.Excerpt));

            //now save the new file
            doc.Save("index.html");
        }

        private class PostExtract 
        {
            public String PostId { get; set; }
            public String PostExcerpt { get; set; }
            public Int64 PostDate { get; set; }
        }
    }
}
