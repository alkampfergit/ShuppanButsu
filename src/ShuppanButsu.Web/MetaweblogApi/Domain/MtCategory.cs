

using System;
using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[Serializable]
	public class MtCategory {
		public string categoryId;
		[XmlRpcMissingMapping ( MappingAction.Ignore )] public string categoryName;
		[XmlRpcMissingMapping ( MappingAction.Ignore )] public bool isPrimary;
	}
}
