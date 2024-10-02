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
using System.Data.OleDb;

public partial class ClimbDetailPathXML : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		int climbId = 4;
		string parameter = Context.Request.QueryString["ClimbId"];
		if (parameter != null)
		{
			climbId = Int32.Parse(parameter);
		}

		PathCollection paths = new PathCollection();
		paths.FetchPathForClimb(climbId);

		Response.ContentType = "Text/xml";
		Response.Write(paths.Xml.OuterXml);
	}
}
