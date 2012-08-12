

using System;
using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[Serializable]
	public class CategoryInfo {
		public string categoryid;
		public string description;
		public string htmlUrl;
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public string parentid;
		public string rssUrl;
		public string title;
	}
}
