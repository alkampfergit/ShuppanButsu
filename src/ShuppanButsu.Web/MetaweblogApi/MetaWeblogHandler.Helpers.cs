#region Disclaimer/Info

/////////////////////////////////////////////////////////////////////////////////////////////////
//
//   File:		MetaWeblogHandler.Helpers.cs
//   Website:		http://dexterblogengine.com/
//   Authors:		http://dexterblogengine.com/About.ashx
//   Rev:		1
//   Created:		19/01/2011
//   Last edit:		19/01/2011
//   License:		GNU Library General Public License (LGPL)
// 
//   For updated news and information please visit http://dexterblogengine.com/
//   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
//   For any question contact info@dexterblogengine.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Security;
using CookComputing.XmlRpc;
using ShuppanButsu.Web.MetaweblogApi.Domain;


namespace ShuppanButsu.Web.MetaweblogApi
{
	partial class MetaWeblogHandler {
        //void ValidateUser ( string username , string password ) {
        //    bool isValid = Membership.ValidateUser ( username , password );

        //    if ( !isValid ) {
        //        logger.Warn ( "Invalid Credential." );
        //        throw new XmlRpcFaultException ( 0 , "User is not valid!" );
        //    }
        //}

        //static void ConvertAllowCommentsForPosts ( int allowComments , Post item ) {
        //    // SiteConfiguration conf;

        //    switch ( allowComments ) {
        //        case 0:
        //            // none and default seem to have the same value...so I return the default
        //            // and comments are enabled by default on the posts, the moderation shuould be another thing
        //            item.EnableComment ( );
        //            //conf = configurationService.Configuration;
        //            //if (conf.CommentConfiguration.ModerationType == ModerationType.Post)
        //            //{
        //            //    item.EnableComment();
        //            //}
        //            //else
        //            //{
        //            //    item.DisableComment();
        //            //}
        //            break;
        //        case 1:
        //            item.EnableComment ( );
        //            break;
        //        case 2:
        //            item.DisableComment ( );
        //            break;
        //        default:
        //            // comments are enabled by default on posts
        //            item.EnableComment ( );
        //            //conf = configurationService.Configuration;
        //            //if (conf.CommentConfiguration.ModerationType == ModerationType.Post)
        //            //{
        //            //    item.EnableComment();
        //            //}
        //            //else
        //            //{
        //            //    item.DisableComment();
        //            //}
        //            break;
        //    }
        //}

        //void ConvertAllowComments ( int allowComments , Page item ) {
        //    switch ( allowComments ) {
        //        case 0:
        //            item.DisableComment ( );
        //            break;
        //        case 1:
        //            item.EnableComment ( );
        //            break;
        //        case 2:
        //            item.DisableComment ( );
        //            break;
        //        default:
        //            item.DisableComment ( );
        //            break;
        //    }
        //}

        //string ProcessPostData ( string blogId , string username , string password , MetaWeblogApi.Domain.Post post , int? postId ) {
        //    var title = post.title.DecodeHtml ( );
        //    string body = post.description;

        //    var newPost = !postId.HasValue || postId.Value < 1
        //                    ? Post.CreateNewPost ( title , null , body , username )
        //                    : postService.GetByKey ( postId.Value );

        //    newPost.Title = title;
        //    newPost.Slug = post.wp_slug;
        //    newPost.FormattedBody = body;

        //    if ( !string.IsNullOrEmpty ( post.mt_excerpt ) ) {
        //        newPost.Abstract = post.mt_excerpt;
        //    }
        //    else {
        //        newPost.Abstract = ( !string.IsNullOrEmpty ( post.description ) )
        //                            ? post.description.CleanHtmlText ( ).Trim ( ).Replace ( "&nbsp;" , string.Empty ).Cut ( 250 )
        //                            : string.Empty;
        //    }
        //    if ( newPost.IsTransient ) {
        //        newPost.PublishDate = ( post.dateCreated == DateTime.MinValue || post.dateCreated == DateTime.MaxValue )
        //                                ? DateTime.Now
        //                                : new DateTime ( post.dateCreated.Ticks , DateTimeKind.Utc );
        //    }
        //    else {
        //        newPost.PublishDate = ( post.dateCreated == DateTime.MinValue || post.dateCreated == DateTime.MaxValue )
        //                                ? newPost.PublishDate
        //                                : new DateTime ( post.dateCreated.Ticks , DateTimeKind.Utc );
        //    }

        //    if ( newPost.PublishDate <= configurationService.Configuration.GetFromUtcToUserTimeZone ( DateTime.Now.ToUniversalTime ( ) ) ) {
        //        newPost.Publish ( );
        //    }
        //    else {
        //        newPost.UnPublish ( );
        //    }

        //    newPost.BreakOnAggregate = body.Contains ( "[more]" );

        //    // remove and readd the categories (if used to update an existing post)
        //    // we might have removed a post from a category
        //    newPost.Categories.Clear ( );

        //    if ( post.categories != null ) {
        //        IList <string> categories = post.categories.ToList ( );

        //        var allCategories = categoryService.GetAllCategories ( );

        //        foreach ( string c in categories ) {
        //            var cat = allCategories.Where ( x => x.Name == c ).FirstOrDefault ( );
        //            if ( cat != null ) {
        //                newPost.AddCategory ( cat );
        //            }
        //        }
        //    }

        //    newPost.Tags.Clear ( );

        //    IEnumerable <string> tags = ExtractTags ( post );

        //    if ( tags != null && tags.Count ( ) > 0 ) {
        //        tags.Where ( x => !string.IsNullOrEmpty ( x ) ).ForEach ( x => newPost.Tags.Add ( TagItem.CreateNewTagItem ( x , newPost ) ) );
        //    }

        //    ConvertAllowCommentsForPosts ( post.mt_allow_comments , newPost );

        //    // set the author if specified, otherwise use the user that is authenticated by wlw
        //    // once a post is done, only the 'poster' can modify it
        //    if ( !string.IsNullOrEmpty ( post.wp_author_id ) ) {
        //        // get the list of members
        //        WpAuthor[] authors = WpGetAuthors ( blogId , username , password );
        //        int authorId = Convert.ToInt32 ( post.wp_author_id );
        //        string author = authors.Where ( a => a.user_id == authorId ).First ( ).user_login;
        //        newPost.Username = author;
        //    }
        //    else if ( !string.IsNullOrEmpty ( post.userid ) ) {
        //        // get the list of members
        //        WpAuthor[] authors = WpGetAuthors ( blogId , username , password );
        //        int authorId = Convert.ToInt32 ( post.userid );
        //        string author = authors.Where ( a => a.user_id == authorId ).First ( ).user_login;
        //        newPost.Username = author;
        //    }
        //    else {
        //        newPost.Username = username;
        //    }

        //    if ( newPost.IsTransient ) {
        //        postService.Save ( newPost );
        //    }
        //    else {
        //        postService.Update ( newPost );
        //    }

        //    return newPost.Id.ToString ( );
        //}

        //int ProcessPageData ( string blog_id , string username , string password , MetaWeblogApi.Domain.Page content , int? pageId ) {
        //    string body = content.description;
        //    var title = content.title.DecodeHtml ( );

        //    var nwPage = !pageId.HasValue || pageId.Value < 1
        //                    ? Page.CreateNewPage ( title , body , null )
        //                    : pageService.GetByKeyWithParent ( pageId.Value , true );

        //    // everything that pass through the metaweblog handler goes published by default
        //    nwPage.Publish ( );

        //    // questionable, if we do this we cannot reopen and exit the post with WLW again
        //    // string body = Utility.RemovePlumbingFromWlwHtmlTags(post.description);

        //    nwPage.Title = title;
        //    nwPage.Slug = content.wp_slug;
        //    nwPage.FormattedBody = body;
        //    nwPage.Abstract = ( !string.IsNullOrEmpty ( content.description ) )
        //                        ? content.description.CleanHtmlText ( ).Trim ( ).Replace ( "&nbsp;" , string.Empty ).Cut ( 250 )
        //                        : string.Empty;
        //    if ( nwPage.IsTransient ) {
        //        nwPage.PublishDate = ( content.dateCreated == DateTime.MinValue || content.dateCreated == DateTime.MaxValue )
        //                                ? DateTime.Now
        //                                : new DateTime ( content.dateCreated.Ticks , DateTimeKind.Utc );
        //    }
        //    else {
        //        nwPage.PublishDate = ( content.dateCreated == DateTime.MinValue || content.dateCreated == DateTime.MaxValue )
        //                                ? nwPage.PublishDate
        //                                : new DateTime ( content.dateCreated.Ticks , DateTimeKind.Utc );
        //    }

        //    nwPage.BreakOnAggregate = body.Contains ( "[more]" );

        //    int order;
        //    int.TryParse ( content.wp_page_order , out order );
        //    nwPage.SortOrder = order;

        //    int parentId;
        //    if ( int.TryParse ( content.wp_page_parent_id , out parentId ) ) {
        //        if ( parentId > 0 ) {
        //            var parentPage = pageService.GetByKeyWithParent ( parentId );
        //            nwPage.Parent = parentPage;
        //        }
        //    }

        //    ConvertAllowComments ( content.mt_allow_comments , nwPage );

        //    // set the author if specified, otherwise use the user that is authenticated by wlw
        //    // once a post is done, only the 'poster' can modify it
        //    if ( !string.IsNullOrEmpty ( content.wp_author_id ) ) {
        //        // get the list of members
        //        WpAuthor[] authors = WpGetAuthors ( blog_id , username , password );
        //        int authorId = Convert.ToInt32 ( content.wp_author_id );
        //        string author = authors.Where ( a => a.user_id == authorId ).First ( ).user_login;
        //        nwPage.Username = author;
        //    }
        //    else if ( !string.IsNullOrEmpty ( content.user_id ) ) {
        //        // get the list of members
        //        WpAuthor[] authors = WpGetAuthors ( blog_id , username , password );
        //        int authorId = Convert.ToInt32 ( content.user_id );
        //        string author = authors.Where ( a => a.user_id == authorId ).First ( ).user_login;
        //        nwPage.Username = author;
        //    }
        //    else {
        //        nwPage.Username = username;
        //    }

        //    if ( nwPage.IsTransient ) {
        //        pageService.Save ( nwPage );
        //    }
        //    else {
        //        pageService.Update ( nwPage );
        //    }

        //    // in the end we need to refresh the routes
        //    routingService.UpdateRoutes ( );

        //    return nwPage.Id;
        //}

        //static IEnumerable <string> ExtractTags ( Post post ) {
        //    string[] tagsFromBody = PostHelpers.RetrieveTagsFromBody ( post.description );
        //    string[] tagsPosed = !string.IsNullOrEmpty ( post.mt_keywords )
        //                            ? post.mt_keywords.Split ( new[] {
        //                                ';' , ','
        //                            } )
        //                            : new string[] {};
        //    var tags = new string[tagsFromBody.Length + tagsPosed.Length];
        //    tagsFromBody.CopyTo ( tags , 0 );
        //    tagsPosed.CopyTo ( tags , tagsFromBody.Length );
        //    return tags;
        //}

        //static Post GetPost ( Post item , IEnumerable <WpAuthor> authors ) {
        //    string authorId = GetAuthorId ( item , authors );

        //    // the fields reported here are all the ones you should use
        //    var p = new MetaWeblogApi.Domain.Post {
        //        dateCreated = item.PublishDate ,
        //        userid = authorId ,
        //        postid = item.Id.ToString ( ) ,
        //        description = item.FormattedBody ,
        //        title = item.Title ,
        //        //TODO: UrlBuilder
        //        //link = item.Url,
        //        //permalink = item.Url,
        //        categories = item.FlatCategories.Select ( c => c.Name ).ToArray ( ) ,
        //        mt_excerpt = item.Abstract ,
        //        // mt_text_more = 
        //        mt_allow_comments = item.CommentEnabled
        //                                ? 1
        //                                : 2 , // open or close
        //        // mt_allow_pings = 
        //        mt_keywords = item.GetCommaSeparatedTags ( ) ,
        //        wp_slug = item.Slug ,
        //        // wp_password =
        //        wp_author_id = authorId ,
        //        wp_author_display_name = item.Username
        //        // date_created_gmt =
        //        // post_status =
        //        // custom_fields =
        //        // sticky = 
        //    };
        //    return p;
        //}

        ///// <summary>
        ///// 	converts from an internal DTO to a structure a blog tool can use
        ///// </summary>
        ///// <param name = "p"></param>
        ///// <param name = "authors"></param>
        ///// <returns></returns>
        //static MetaWeblogApi.Domain.Page GetMetaweblogPage ( Page p , IEnumerable <WpAuthor> authors ) {
        //    string authorId = GetAuthorId ( p , authors );

        //    return new MetaWeblogApi.Domain.Page {
        //        dateCreated = p.PublishDate ,
        //        user_id = authorId ,
        //        page_id = p.Id ,
        //        //page_status = 
        //        description = p.FormattedBody ,
        //        title = p.Title ,
        //        //TODO: UrlBuilder
        //        //link = p.u.Url,
        //        //permalink = p.Url,
        //        //categories = 
        //        //excerpt = 
        //        //text_more = 
        //        mt_allow_comments = p.CommentEnabled
        //                                ? 1
        //                                : 2 ,
        //        //mt_allow_pings = 
        //        wp_slug = p.Slug ,
        //        //wp_password = 
        //        wp_author = p.Username ,
        //        wp_page_parent_id = p.Parent != null
        //                                ? p.Parent.Id.ToString ( )
        //                                : string.Empty ,
        //        wp_page_parent_title = p.Parent != null
        //                                ? p.Parent.Title
        //                                : string.Empty ,
        //        wp_page_order = p.SortOrder.ToString ( ) ,
        //        wp_author_id = authorId ,
        //        wp_author_display_name = p.Username ,
        //        date_created_gmt = p.PublishDate , // double check this walue

        //        //wp_page_template = 
        //    };
        //}

        ////static string GetAuthorId ( Item item , IEnumerable <WpAuthor> authors ) {
        ////    string authorId = null;

        ////    // take into account a deleted author
        ////    var auth = authors.Where ( a => a.user_login == item.Username ).FirstOrDefault ( );
        ////    if ( auth.user_id > 0 ) {
        ////        authorId = auth.user_id.ToString ( );
        ////    }

        ////    return authorId;
        ////}

        ///// <summary>
        ///// 	looks in the target folder to see if a file with the same name exists
        ///// 	if it exists it change the name adding a suffix (like : '_1')
        ///// </summary>
        ///// <param name = "proposedName"></param>
        ///// <param name = "targetFolder"></param>
        ///// <returns></returns>
        //static string GetFileName ( string proposedName , string targetFolder ) {
        //    string extension = Path.GetExtension ( proposedName );
        //    string baseName = Path.GetFileNameWithoutExtension ( proposedName );
        //    string name = baseName + extension;
        //    string filepath = Path.Combine ( targetFolder , name );
        //    int i = 1;
        //    while ( File.Exists ( filepath ) ) {
        //        name = string.Format ( "{0}_{1}{2}" , baseName , i , extension );
        //        filepath = Path.Combine ( targetFolder , name );
        //        i += 1;
        //    }
        //    return name;
        //}
	}
}