using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using ShuppanButsu.Web.MetaweblogApi.Domain;

namespace ShuppanButsu.Web.MetaweblogApi
{
    ///<summary>
    ///ORIGINALLY TAKEN FROM THE CODE OF DEXTER BLOG ENGINE http://dexterblogengine.codeplex.com/
    ///	The contract for the <c>MetaWeblogApi</c> implementation.
    ///</summary>
    ///<remarks>
    ///	Is IMPORTANT, so respect it when defining the structures the follows guideline:
    ///	<seealso cref = "http://www.xmlrpc.com/metaWeblogApi" />
    ///	<seealso cref = "http://codex.wordpress.org/XML-RPC_wp" />
    ///	<seealso cref = "http://www.sixapart.com/developers/xmlrpc/" />
    ///	<seealso cref = "http://www.orchardproject.net/docs/Default.aspx?Page=xml-rpc&NS=&AspxAutoDetectCookieSupport=1" />
    ///	<seealso cref = "http://msdn.microsoft.com/en-us/library/bb463260.aspx" />
    ///</remarks>
    public interface IMetaWeblog
    {
        #region MetaWeblog API

        [XmlRpcMethod("metaWeblog.newPost")]
        string AddPost(string blogid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.editPost")]
        bool UpdatePost(string postid, string username, string password, Post post, bool publish);

        [XmlRpcMethod("metaWeblog.getPost")]
        Post GetPost(string postid, string username, string password);

        [XmlRpcMethod("metaWeblog.getCategories")]
        CategoryInfo[] GetCategories(string blogid, string username, string password);

        [XmlRpcMethod("metaWeblog.getRecentPosts", Description = "Retrieves a list of the most recent existing post using the metaWeblog API. Returns the metaWeblog struct collection.")]
        Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts);

        [XmlRpcMethod("metaWeblog.newMediaObject", Description = "Uploads an image, movie, song, or other media using the metaWeblog API. Returns the metaObject struct.")]
        MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject);

        #endregion

        #region Blogger API

        [XmlRpcMethod("blogger.deletePost")]
        [return: XmlRpcReturnValue(Description = "Returns true.")]
        bool DeletePost(string key, string postid, string username, string password, bool publish);

        [XmlRpcMethod("blogger.getUsersBlogs")]
        BlogInfo[] GetUsersBlogs(string key, string username, string password);

        [XmlRpcMethod("blogger.getUserInfo")]
        UserInfo GetUserInfo(string key, string username, string password);

        #endregion

        #region Wordpresse API

        [XmlRpcMethod("wp.newCategory",
            Description = "Adds a new category to the blog engine.")]
        int WpNewCategory(
            string blog_id,
            string username,
            string password,
            WpNewCategory category);

        [XmlRpcMethod("wp.newPage", Description = "Adds a new page/article to the blog engine.")]
        int newPage(
            string blog_id,
            string username,
            string password,
            Page content,
            bool publish);

        [XmlRpcMethod("wp.editPage", Description = "Adds a new page/article to the blog engine.")]
        int editPage(
            string blog_id,
            string page_id,
            string username,
            string password,
            Page content,
            bool publish);

        [XmlRpcMethod("wp.getPages", Description = "Get an array of all the pages on a blog.")]
        Page[] getPages(
            string blog_id,
            string username,
            string password,
            int number
            );

        [XmlRpcMethod("wp.getPageList", Description = "Get an array of all the pages on a blog. Just the minimum details.")]
        PageInfo[] getPageList(
            string blog_id,
            string username,
            string password
            );

        [XmlRpcMethod("wp.getPage", Description = "Get the page identified by the page id.")]
        Page getPage(
            string blog_id,
            string page_id,
            string username,
            string password
            );

        [XmlRpcMethod("wp.deletePage", Description = "Removes a page from the blog.")]
        bool deletePage(
            string blog_id,
            string username,
            string password,
            string page_id);

        [XmlRpcMethod("wp.getCategories")]
        WpCategoryInfo[] WpGetCategories(string blogid, string username, string password);

        [XmlRpcMethod("wp.getTags")]
        WpTagInfo[] WpGetTags(string blog_id, string username, string password);

        [XmlRpcMethod("wp.getAuthors")]
        WpAuthor[] WpGetAuthors(string blog_id, string username, string password);

        #endregion

        #region Movable Type API

        [XmlRpcMethod("mt.setPostCategories")]
        bool MtSetPostCategories(string postid, string username, string password, MtCategory[] categories);

        [XmlRpcMethod("mt.getPostCategories")]
        MtCategory[] MtGetPostCategories(string postid, string username, string password);

        #endregion
    }
}
