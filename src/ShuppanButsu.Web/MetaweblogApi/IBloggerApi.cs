

using CookComputing.XmlRpc;
using ShuppanButsu.Web.MetaweblogApi.Domain;

namespace ShuppanButsu.Web.MetaweblogApi
{
	public interface IBloggerApi {
		[XmlRpcMethod ( "blogger.deletePost" , Description = "Deletes a post." )]
		[return: XmlRpcReturnValue ( Description = "Always returns true." )]
		bool deletePost (
			string appKey ,
			string postid ,
			string username ,
			string password ,
			[XmlRpcParameter (
				Description = "Where applicable, this specifies whether the blog "
				              + "should be republished after the post has been deleted." )] bool publish );

		[XmlRpcMethod ( "blogger.getUsersBlogs" ,
			Description = "Returns information on all the blogs a given user "
			              + "is a member." )]
		BlogInfo[] getUsersBlogs (
			string appKey ,
			string username ,
			string password );
	}
}
