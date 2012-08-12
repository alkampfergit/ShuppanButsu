

using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[XmlRpcMissingMapping ( MappingAction.Ignore )]
	public class Source {
		public string name;
		public string url;
	}
}
