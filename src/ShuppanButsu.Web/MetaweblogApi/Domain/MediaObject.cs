
 
using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[XmlRpcMissingMapping ( MappingAction.Ignore )]
	public class MediaObject {
		public byte[] bits;
		public string name;
		public string type;
	}
}
