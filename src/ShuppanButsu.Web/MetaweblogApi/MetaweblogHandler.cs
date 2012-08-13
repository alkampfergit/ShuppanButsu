using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using ShuppanButsu.Web.MetaweblogApi.Domain;

namespace ShuppanButsu.Web.MetaweblogApi
{
    /// <summary>
    /// ORIGINALLY TAKEN FROM THE CODE OF DEXTER BLOG ENGINE http://dexterblogengine.codeplex.com/
    /// </summary>
    public partial class MetaWeblogHandler : XmlRpcService, IMetaWeblog
    {
    

        public MetaWeblogHandler()
        {
            Debug.WriteLine("Handler for metaweblog called");
         
        }

        #region IMetaWeblog Members

        /// <summary>
        /// 	Add a new post.
        /// </summary>
        /// <param name = "blogid">The blogid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "post">The post.</param>
        /// <param name = "publish">if set to <c>true</c> [publish].</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public string AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    var data = ProcessPostData(blogid, username, password, post, null);

            //    dexterCall.Complete(true);

            //    return data;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}

            return String.Empty;
        }

        /// <summary>
        /// 	Updates an existing post.
        /// </summary>
        /// <param name = "postid">The postid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "post">The post.</param>
        /// <param name = "publish">if set to <c>true</c> [publish].</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    ProcessPostData(string.Empty, username, password, post, postid.ToInt32(0));

            //    dexterCall.Complete(true);

            //    return true;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return true;
        }

        /// <summary>
        /// 	Retrieve as instance of <see cref = "Post" /> for the specified <c>Id</c>.
        /// </summary>
        /// <param name = "postid">The postid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns>An instance of <see cref = "Post" /> or null if the post doesn't exist.</returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public Post GetPost(string postid, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var item = postService.GetByKey(postid.ToInt32());

            //    WpAuthor[] authors = WpGetAuthors(string.Empty, username, password);
            //    var p = GetPost(item, authors);

            //    dexterCall.Complete(true);

            //    return p;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        ///<summary>
        ///	Return a list of all categories
        ///</summary>
        ///<param name = "blogid">The blogid.</param>
        ///<param name = "username">The username.</param>
        ///<param name = "password">The password.</param>
        ///<returns>An array of <see cref = "CategoryInfo" /> or null if there aren't categories into the database.</returns>
        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public CategoryInfo[] GetCategories(string blogid, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    // var categories = categoryService.GetCategoriesStructure().ToFlat(c => c.Categories);
            //    var categories = categoryService.GetAllCategories();

            //    var data = categories.Select(c => new CategoryInfo
            //    {
            //        categoryid = c.Id.ToString(),
            //        parentid = c.Parent == null
            //                    ? String.Empty
            //                    : c.Parent.Id.ToString(),
            //        title = c.Name,
            //        description = string.IsNullOrEmpty(c.Description)
            //                        ? string.Empty
            //                        : c.Description,
            //        //TODO: Da implementare l'url builder
            //        htmlUrl = "",
            //        rssUrl = "",
            //    }).ToArray();

            //    dexterCall.Complete(true);

            //    return data;

            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	Retrieve a list of recents <see cref = "Post" />.
        /// </summary>
        /// <param name = "blogid">The blogid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "numberOfPosts">The number of posts.</param>
        /// <returns>A list of instances of <see cref = "Post" /> or null if the post doesn't exist.</returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var posts = postService.GetCompleteList(0, numberOfPosts);

            //    var items = new Post[posts.Result.Count()];

            //    var authors = WpGetAuthors("", username, password);

            //    int i = 0;
            //    foreach (var post in posts.Result)
            //    {
            //        items[i] = GetPost(post, authors);
            //        i++;
            //    }

            //    dexterCall.Complete(true);

            //    return items;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// News the media object.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="mediaObject">The media object.</param>
        /// <returns></returns>
        public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            //dexterCall.StartSession(true);

            //ValidateUser(username, password);

            //try
            //{
            //    string userFolderName = username.MakeValidFileName();

            //    string path = "UserFiles\\" + userFolderName + "\\" + mediaObject.name;

            //    //GetFileName(mediaObject.name,targetFolder);

            //    if (mediaObject.bits != null)
            //    {
            //        storage.SaveStream(path, new MemoryStream(mediaObject.bits));

            //        var publicPath = urlBuilderBase.ResolveUrl(string.Concat("~\\", path));

            //        MediaObjectInfo objectInfo = new MediaObjectInfo
            //        {
            //            url = storage.GetPublicUrl(publicPath)
            //        };

            //        dexterCall.Complete(true);

            //        return objectInfo;
            //    }

            //    dexterCall.Complete(true);

            //    throw new XmlRpcFaultException(0, "Invalid Media");
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	Deletes the specified post.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "postid">The postid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "publish">if set to <c>true</c> [publish].</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public bool DeletePost(string key, string postid, string username, string password, bool publish)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    postService.Delete(postid.ToInt32(0));

            //    // todo: what to do with the media object related to the post we are removing ?

            //    dexterCall.Complete(true);
            //    return true;
            //}
            //catch (PostNotFoundException)
            //{
            //    dexterCall.Complete(true);

            //    throw new XmlRpcFaultException(0, "Post not found.");
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return true;
        }

        /// <summary>
        /// 	Retrieve the list of available blogs for the  logged user.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns>An arrary of <see cref = "BlogInfo" />.</returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public BlogInfo[] GetUsersBlogs(string key, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var conf = configurationService.Configuration;

            //    var blogInfo = new BlogInfo
            //    {
            //        blogid = "nothing",
            //        blogName = conf.BlogName,
            //        url = urlBuilderBase.HomePage.ToString()
            //    };

            //    dexterCall.Complete(true);

            //    return new[] { blogInfo };
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}

            var blogInfo = new BlogInfo
            {
                blogid = "root",
                blogName = "Root",
                url = "http://localhost:42000/metaweblog.axd"
            };
            return new[] { blogInfo };
        }

        /// <summary>
        /// 	Retrive a set of information for the logged user.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public UserInfo GetUserInfo(string key, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var info = new UserInfo();
            //    MembershipUser usr = Membership.GetUser(username);

            //    info.email = usr.Email;
            //    info.nickname = username;
            //    info.url = urlBuilderBase.HomePage.ToString();
            //    info.userid = ((int)usr.ProviderUserKey).ToString();

            //    dexterCall.Complete(true);

            //    return info;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	Create a new <see cref = "Domain.Model.Category" />.
        /// </summary>
        /// <param name = "blog_id">The blog_id.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "category">The category.</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public int WpNewCategory(string blog_id, string username, string password, WpNewCategory category)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    Category cat = categoryService.GetCategoryByNameWithChilds(category.name);

            //    Category parentCategory = null;
            //    if (category.parent_id > 0)
            //        parentCategory = categoryService.GetCategoryByKeyWithChilds(category.parent_id);

            //    if (cat == null)
            //    {
            //        cat = Category.CreateNewCategory(category.name, parentCategory);
            //        categoryService.Save(cat);
            //    }
            //    else
            //    {
            //        if (cat.Parent != parentCategory)
            //        {
            //            cat.Parent = parentCategory;
            //            categoryService.Update(cat);
            //        }
            //    }

            //    dexterCall.Complete(true);

            //    return cat.Id;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return 0;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public int newPage(string blog_id, string username, string password, Page content, bool publish)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    var data = ProcessPageData(blog_id, username, password, content, null);

            //    dexterCall.Complete(true);

            //    return data;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return 0;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public int editPage(string blog_id, string page_id, string username, string password, Page content, bool publish)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    var data = ProcessPageData(blog_id, username, password, content, page_id.ToInt32(0));

            //    dexterCall.Complete(true);

            //    return data;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return 0;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public Page[] getPages(string blog_id, string username, string password, int number)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    var pages = pageService.GetPages(0, int.MaxValue).Result;
            //    WpAuthor[] authors = WpGetAuthors(blog_id, username, password);

            //    var data = pages.Select(p => GetMetaweblogPage(p, authors)).ToArray();

            //    dexterCall.Complete(true);

            //    return data;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public PageInfo[] getPageList(string blog_id, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var pages = pageService.GetPages(0, int.MaxValue).Result;

            //    var data = pages.Select(p => new PageInfo
            //    {
            //        dateCreated = p.PublishDate,
            //        page_id = p.Id,
            //        page_parent_id = p.Parent != null ? p.Parent.Id : 0,
            //        page_title = p.Title,
            //    }).ToArray();

            //    dexterCall.Complete(true);

            //    return data;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public Page getPage(string blog_id, string page_id, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var item = pageService.GetByKeyWithParent(page_id.ToInt32());
            //    WpAuthor[] authors = WpGetAuthors(blog_id, username, password);
            //    Page p = GetMetaweblogPage(item, authors);

            //    dexterCall.Complete(true);

            //    return p;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public bool deletePage(string blog_id, string username, string password, string page_id)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    pageService.Delete(page_id.ToInt32(0));

            //    routingService.UpdateRoutes();

            //    dexterCall.Complete(true);

            //    return true;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return false;
        }

        ///<summary>
        ///	Return a list of all categories
        ///</summary>
        ///<param name = "blogid">The blogid.</param>
        ///<param name = "username">The username.</param>
        ///<param name = "password">The password.</param>
        ///<returns>An array of <see cref = "WpCategoryInfo" /> or null if there aren't categories into the database.</returns>
        ///<exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public WpCategoryInfo[] WpGetCategories(string blogid, string username, string password)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    //var categories = categoryService.GetCategoriesStructure().ToFlat(c => c.Categories);
            //    var categories = categoryService.GetAllCategories();

            //    WpCategoryInfo[] list = categories.Select(c => new WpCategoryInfo
            //    {
            //        categoryId = c.Id,
            //        parentId = c.Parent == null
            //                    ? 0
            //                    : c.Parent.Id,
            //        description = string.IsNullOrEmpty(c.Description)
            //                        ? string.Empty
            //                        : c.Description,
            //        categoryName = c.Name,
            //        //TODO: Da implementare l'url builder
            //        htmlUrl = "",
            //        rssUrl = ""
            //    }).ToArray();

            //    dexterCall.Complete(true);

            //    return list;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	Retrieve the top <c>500</c> tags.
        /// </summary>
        /// <param name = "blog_id">The blog_id.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public WpTagInfo[] WpGetTags(string blog_id, string username, string password)
        {
            //dexterCall.StartSession();

            //try
            //{
            //    ValidateUser(username, password);

            //    var tags = tagService.GetTopTags(500);
            //    WpTagInfo[] tagArray = tags.Select(o => new WpTagInfo
            //    {
            //        name = o.Name,
            //        count = o.Count,
            //        slug = o.Name,
            //        html_url = string.Empty,
            //        rss_url = string.Empty
            //    }).ToArray();

            //    dexterCall.Complete(true);

            //    return tagArray;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	Retrieve all authors.
        /// </summary>
        /// <param name = "blog_id">The blog_id.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns>An array of <see cref = "WpAuthor" />.</returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public WpAuthor[] WpGetAuthors(string blog_id, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    string[] usrs = Roles.GetUsersInRole("Poster");

            //    var authors = new WpAuthor[usrs.Length];

            //    for (int i = 0; i < usrs.Length; i++)
            //    {
            //        var user = Membership.GetUser(usrs[i]);

            //        authors[i] = new WpAuthor
            //        {
            //            display_name = usrs[i],
            //            user_login = usrs[i],
            //            user_email = user.Email,
            //            user_id = (int)user.ProviderUserKey,
            //            meta_value = string.Empty
            //        };
            //    }

            //    dexterCall.Complete(true);

            //    return authors;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        /// <summary>
        /// 	MoveableType set the set post categories.
        /// </summary>
        /// <param name = "postid">The postid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <param name = "categories">The categories.</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public bool MtSetPostCategories(string postid, string username, string password, MtCategory[] categories)
        {
            // ValidateUser(username, password);
            // do nothing here, we handle the category assignemnt in the AddPost and UpdatePost functions
            return true;
        }

        /// <summary>
        /// 	MoveableType get the post categories.
        /// </summary>
        /// <param name = "postid">The postid.</param>
        /// <param name = "username">The username.</param>
        /// <param name = "password">The password.</param>
        /// <returns></returns>
        /// <exception cref = "XmlRpcFaultException"> If <paramref name = "username" /> or <paramref name = "password" /> are invalid.</exception>
        public MtCategory[] MtGetPostCategories(string postid, string username, string password)
        {
            //dexterCall.StartSession(true);

            //try
            //{
            //    ValidateUser(username, password);

            //    var item = postService.GetByKey(postid.ToInt32());

            //    var cats = new MtCategory[item.Categories.Count];
            //    for (int i = 0; i < item.Categories.Count; i++)
            //    {
            //        cats[i] = new MtCategory
            //        {
            //            categoryId = item.Categories[i].Id.ToString(),
            //            categoryName = item.Categories[i].Name,
            //            // todo: fill up the 'is primary field", maybe this must be true for the first category only
            //            isPrimary = (i == 0)
            //        };
            //    }

            //    dexterCall.Complete(true);

            //    return cats;
            //}
            //catch (Exception e)
            //{
            //    dexterCall.Complete(false);
            //    logger.Error(e.Message, e);
            //    throw;
            //}
            return null;
        }

        #endregion
    }
}
