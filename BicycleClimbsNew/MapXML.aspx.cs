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
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using BicycleClimbsLibrary;

public partial class MapXML : System.Web.UI.Page
{
	public ClimbCollection GetClimbs(string query)
	{
		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate(query);

		return climbs;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		string regionId = Context.Request.QueryString["RegionId"];
		if (regionId == null)
		{
			regionId = "1";
		}

        string userId = Context.Request.QueryString["UserId"];
        string completedFilter = Context.Request.QueryString["CompletedFilter"];

		int regionIdInt = Int32.Parse(regionId);
		Region region = RegionCollection.Load(regionIdInt);

		ClimbCollection climbs = GetClimbs("where regionid=" + regionId + " order by name");
        if (userId != null)
        {
            int userIdInt = Int32.Parse(userId);
            int completedFilterId = Int32.Parse(completedFilter);

            if (completedFilterId != 0)
            {
                climbs.LoadCompletedData(userIdInt);
                climbs.FilterCompleted(completedFilterId == 1);
            }
        }

		Response.ContentType = "Text/xml";
		Response.Write(climbs.GetXml(region.Latitude, region.Longitude).OuterXml);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache"); 
	}
}
