

using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[XmlRpcMissingMapping ( MappingAction.Ignore )]
	public class Enclosure {
		public int length;
		public string type;
		public string url;
	}
}
