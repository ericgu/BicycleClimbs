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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using BicycleClimbsLibrary;

public partial class ProfileGraph : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string climbId = Context.Request.QueryString["ClimbId"];
		if (climbId == null)
		{
			climbId = "4";
		}
		int climbIdInt = Int32.Parse(climbId);

		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate("where id=" + climbId);
		if (climbs.Count == 0)
		{
			return;
		}
		Climb climb = climbs[0];

		Bitmap bitmap = climb.CreateGraph();
		Response.ContentType = "image/jpeg";

		bitmap.Save(Response.OutputStream, ImageFormat.Jpeg);
	}
}
