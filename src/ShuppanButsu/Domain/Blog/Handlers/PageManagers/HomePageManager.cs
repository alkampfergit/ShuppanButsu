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
        private List<PostCreated> postsInHomePage = new List<PostCreated>();

        public void PostCreatedHandler(PostCreated evt) 
        {
            //Verify if we need to add this post to the list.
            if (postsInHomePage.Count >= Configuration.NumberOfPostsInHomePage &&
                evt.Timestamp < postsInHomePage.Min(e => e.Timestamp)) 
            {
                //already reached maximum post number in home page, this post is not older than the other
                //nothing to add in the home page.
                return;
            }
    
            HtmlDocument templateDocument = new HtmlDocument();
            String BlogDirectory = Path.Combine(Configuration.BaseGenerationDirectory, evt.BlogName);
           
            if ( !String.IsNullOrEmpty(BlogDirectory) && !Directory.Exists(BlogDirectory)) Directory.CreateDirectory(BlogDirectory);
            String templateDirectory = Path.Combine(Configuration.TemplateDirectory, BlogDirectory);
            if (!String.IsNullOrEmpty(BlogDirectory) && !Directory.Exists(templateDirectory)) { 
                //Specific template for this blog does not exists, simple use the standard template
                templateDirectory = Configuration.TemplateDirectory;
            }

            String templateFileName = Path.Combine(templateDirectory, "index.html");
            FileInfo finfo = new FileInfo(templateFileName);
            if (!finfo.Exists) 
            {
                Logger.Fatal("Template file missing: " + finfo.FullName);
                return;
            }

            var destinationFileName = String.IsNullOrEmpty(BlogDirectory) ? "index.html" : Path.Combine(BlogDirectory, "index.html");

            templateDocument.LoadHtml(File.ReadAllText(finfo.FullName));
            //now find the div with the template of post excerpt
            var templateOfExcerpt = templateDocument.DocumentNode.SelectSingleNode("//*[@id='{posttemplate}']");
            if (templateOfExcerpt == null)
            {
                Logger.Fatal("The template in file " + finfo.FullName + " has not {posttemplate} element to use. The template is malformed");
                return;
            }

            //Template seems to be valid
            //If destination file does not exists, copy the raw template
            HtmlDocument destinationDocument = new HtmlDocument();
            if (!File.Exists(destinationFileName))
            {
                PrepareEmptyDocumentFromTemplate(finfo, destinationDocument);
            }
            else
            {
                destinationDocument.LoadHtml(File.ReadAllText(destinationFileName));
            }

            //remove from original document, remove the attribute id
            templateOfExcerpt.Remove();
            templateOfExcerpt.Attributes.Add("id", "postid-" + evt.Id);

            //now we have template, if it is a newPage we can modify this one, if it is not first time we create home page we need to copy.
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

            postsInHomePage.Add(evt);
            
            while (postsInHomePage.Count > Configuration.NumberOfPostsInHomePage)
            {
                var postToRemove = postsInHomePage.OrderBy(e => e.Timestamp).First();
                postsInHomePage.Remove(postToRemove);
                var docFragmentToRemove = destinationDocument.DocumentNode.SelectSingleNode("//*[@id='postid-" + postToRemove.Id + "']");
                if (docFragmentToRemove != null)
                {
                    docFragmentToRemove.Remove();
                }
                else 
                {
                    Logger.Warn("Post id " + postToRemove.Id + " element was not found in the home page");
                }
                
            }

            //now append this to the original document and save.
            destinationDocument.DocumentNode.SelectSingleNode("//*[@id='post-container']").AppendChild(templateOfExcerpt);
            destinationDocument.Save(destinationFileName);
        }

        private static void PrepareEmptyDocumentFromTemplate(FileInfo finfo, HtmlDocument destinationDocument)
        {
            destinationDocument.LoadHtml(File.ReadAllText(finfo.FullName));
            //clear all template related content from the home page (needed if it is the first time we create the page)
            var templateNode = destinationDocument.DocumentNode.SelectSingleNode("//*[@id='{posttemplate}']");
            templateNode.ParentNode.Attributes.Add("id", "post-container");
            templateNode.Remove();
        }
    }
}
