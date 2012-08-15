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
    public class PostPageManager : BaseDomainEventHandler
    {
        public void CreatePostHtmlDocument(PostCreated evt)
        {
            HtmlDocument templateDocument = new HtmlDocument();
            String BlogDirectory = evt.BlogName;

            if (!String.IsNullOrEmpty(BlogDirectory) && !Directory.Exists(BlogDirectory)) Directory.CreateDirectory(BlogDirectory);
            String templateDirectory = Path.Combine(Configuration.TemplateDirectory, BlogDirectory);
            if (!String.IsNullOrEmpty(BlogDirectory) && !Directory.Exists(templateDirectory))
            {
                //Specific template for this blog does not exists, simple use the standard template
                templateDirectory = Configuration.TemplateDirectory;
            }

            String baseDirectory = Path.Combine(BlogDirectory, Configuration.BaseGenerationDirectory);
            if (!Directory.Exists(baseDirectory))
            {

                Directory.CreateDirectory(baseDirectory);
            }
            //a post is really simpler than the home page, it gets simply recreated from a template with simple substitution
            String postFileName = Path.Combine(baseDirectory, evt.SlugCode) + ".html";
            if (File.Exists(postFileName))
            {
                //oops there is a problem, the file already exists, 
                throw new NotImplementedException("We need to implement the logic for post file that already exists");
            }


            var templateFileName = new FileInfo(Path.Combine(templateDirectory, "post.html"));
            if (!templateFileName.Exists)
            {
                Logger.Fatal("Template file missing: " + templateFileName.FullName);
                return;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(File.ReadAllText(templateFileName.FullName));
            if (!ReplaceNode(doc, "{title}", evt.Title, templateFileName.FullName)) return;
            if (!ReplaceNode(doc, "{content}", evt.Content, templateFileName.FullName)) return;

            doc.Save(postFileName);
        }

        private Boolean ReplaceNode(HtmlDocument doc, String id, String text, String templateFileName)
        {
            var titleNode = doc.DocumentNode.SelectSingleNode("//*[@id='" + id + "']");
            if (titleNode == null)
            {
                Logger.Fatal("The template in file " + templateFileName + " has not " + id + " element to use. The template is malformed");
                return false;
            }
            titleNode.RemoveAllChildren();
            titleNode.AppendChild(HtmlTextNode.CreateNode(text));
            return true;
        }

    }
}
