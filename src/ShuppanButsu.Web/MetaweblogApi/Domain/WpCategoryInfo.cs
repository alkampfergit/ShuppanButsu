

using System;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[Serializable]
	public class WpCategoryInfo {
		public int categoryId;
		public string categoryName;
		public string description;
		public string htmlUrl;
		public int parentId;
		public string rssUrl;
	}
}
