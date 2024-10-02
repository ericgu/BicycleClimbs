using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BicycleClimbsLibrary;

public partial class RssFeeds : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Master.Title = "BicycleClimbs.com RSS Feeds";
		Master.PageHeading = Master.Title;

		RegionCollection regions = new RegionCollection();
		regions.Load();

		string list = "<ul>";
		foreach (Region region in regions)
		{
			string feedUrl = "http://www.bicycleclimbs.com/ClimbRss.aspx?RegionId=" + region.Id;
			string feedHtml = "<a href=\"" + feedUrl + "\">" + region.Name + "</a>";
			list += "<li>" + feedHtml + "</li>";
		}
		list += "</ul>";
		p_regionFeedList.InnerHtml = list;
	}
}
