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
using System.Data.OleDb;

using BicycleClimbsLibrary;

public partial class DrawRoute : System.Web.UI.Page
{
	string climbId;

	protected void Page_Load(object sender, EventArgs e)
	{
		climbId = Context.Request.QueryString["ClimbId"];
		if (climbId == null)
		{
			climbId = "6";
		}
		int climbIdInt = Int32.Parse(climbId);

		ClimbCollection climbs = new ClimbCollection();
		Climb climb = climbs.LoadClimb(climbIdInt);

		Region region = RegionCollection.Load(climb.RegionId);
		if (climb.LatitudeStart == 0.0)
		{
			climb.LatitudeStart = region.Latitude;
			climb.LongitudeStart = region.Longitude;
		}

		PathCollection path = new PathCollection();
		path.FetchPathForClimb(climbIdInt);

		p_titleControl.InnerText = "Draw route for " + climb.Name;
		p_title.InnerText = "Draw route for " + climb.Name + " (" + path.Count.ToString() + " points)";

		string script = @"
				<script type=""text/javascript"">
				//<![CDATA[
					function SetClimbData()
					{	
						lastLng = $$Longitude$$;
						lastLat = $$Latitude$$; 
    
				    }
				//]]>
				</script>
			";
		script = script.Replace("$$Longitude$$", climb.LongitudeStart.ToString());
		script = script.Replace("$$Latitude$$", climb.LatitudeStart.ToString());

		p_setClimbData.InnerHtml = script;

		this.p_upload.Click += new EventHandler(p_upload_Click);
	}

	void p_upload_Click(object sender, EventArgs e)
	{
#if fred
		if (!Master.LoadCookie())
		{
			p_passwordError.InnerHtml = "<color=red>You must login before entering a climb</color>";
			return;
		}

		if (Master.CookieUser.Password != p_password.Text)
		{
			p_passwordError.InnerHtml = "<color=red>Incorrect password</color>";
			return;
		}
#endif
		string s = p_uploadText.Text;

		s = s.Replace("(", "").Replace(")", "");

		string[] points = s.Split(',');

		int climbIdInt = Int32.Parse(climbId);

		PathCollection pathCollection = new PathCollection();

		for (int i = 0; i < points.Length; i += 2)
		{
			PathPoint point = new PathPoint();
			point.Longitude = Single.Parse(points[i]);
			point.Latitude = Single.Parse(points[i + 1]);
			pathCollection.Add(point);
		}

		pathCollection.SavePathForClimb(climbIdInt);

		ClimbCollection climbs = new ClimbCollection();
		Climb climb = climbs.LoadClimb(climbIdInt);

		climb.LatitudeStart = pathCollection[0].Latitude;
		climb.LongitudeStart = pathCollection[0].Longitude;

		climb.Update();

		Response.Redirect("ClimbDetail.aspx?ClimbId=" + climbIdInt.ToString());

	}
}
