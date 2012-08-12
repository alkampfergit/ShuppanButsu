
using CookComputing.XmlRpc;
using ShuppanButsu.Web.MetaweblogApi.Domain;

namespace ShuppanButsu.Web.MetaweblogApi
{
	public interface IWordPressApi {
		[XmlRpcMethod ( "wp.newCategory" ,
			Description = "Adds a new category to the blog engine." )]
		int newCategory (
			string blogid ,
			string username ,
			string password ,
			WpNewCategory category );

		[XmlRpcMethod ( "wp.newPage" , Description = "Adds a new page/article to the blog engine." )]
		int newPage (
			string blog_id ,
			string username ,
			string password ,
			Page content ,
			bool publish );

		[XmlRpcMethod ( "wp.editPage" , Description = "Adds a new page/article to the blog engine." )]
		int editPage (
			string blog_id ,
			string page_id ,
			string username ,
			string password ,
			Page content ,
			bool publish );

		[XmlRpcMethod ( "wp.getPages" , Description = "Get an array of all the pages on a blog." )]
		Page[] getPages (
			string blog_id ,
			string username ,
			string password
			);

		[XmlRpcMethod ( "wp.getPageList" , Description = "Get an array of all the pages on a blog. Just the minimum details." )]
		PageInfo[] getPageList (
			string blog_id ,
			string username ,
			string password
			);

		[XmlRpcMethod ( "wp.getPage" , Description = "Get the page identified by the page id." )]
		Post getPage (
			string blog_id ,
			string page_id ,
			string username ,
			string password
			);

		[XmlRpcMethod ( "wp.deletePage" , Description = "Removes a page from the blog." )]
		bool deletePage (
			string blog_id ,
			string username ,
			string password ,
			string page_id );
	}
}
