

using System;
using CookComputing.XmlRpc;

namespace ShuppanButsu.Web.MetaweblogApi.Domain
{
	[XmlRpcMissingMapping ( MappingAction.Ignore )]
	public class Page {
		public string[] categories;
		[XmlRpcMissingMapping ( MappingAction.Error )] [XmlRpcMember ( Description = "Required when posting." )] public DateTime dateCreated;
		public DateTime date_created_gmt;

		[XmlRpcMissingMapping ( MappingAction.Error )] [XmlRpcMember ( Description = "Required when posting." )] public string description;

		public string excerpt;
		public string link;

		public int mt_allow_comments;

		public int mt_allow_pings;
		public int page_id;

		public string page_status;
		public string permalink;
		public string text_more;
		[XmlRpcMissingMapping ( MappingAction.Error )] [XmlRpcMember ( Description = "Required when posting." )] public string title;
		public string user_id;

		public string wp_author;
		public string wp_author_display_name;

		/// <summary>
		/// 	id of the post author (used by wlw)
		/// </summary>
		public string wp_author_id;

		public string wp_page_order;

		// it should have been 'int', but WLW complaining
		public string wp_page_parent_id;

		public string wp_page_parent_title;

		// it should have been 'int', but WLW complaining


//array custom_fields
//struct
//string id
//string key
//string value

		public string wp_page_template;
		public string wp_password;
		public string wp_slug;
	}
}
