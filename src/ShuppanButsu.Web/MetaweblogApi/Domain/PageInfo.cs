

using System;
using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	/// <summary>
	/// 	minimal details
	/// </summary>
	[XmlRpcMissingMapping(MappingAction.Ignore)]
	public class PageInfo
	{
		public DateTime dateCreated;
		public int page_id;
		public int page_parent_id;
		public string page_title;
	}
}
