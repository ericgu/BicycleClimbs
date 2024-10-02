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

public partial class MasterTest : System.Web.UI.Page
{
	public ClimbCollection GetClimbs(string query)
	{
		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate(query);

		return climbs;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			string climbId = Context.Request.QueryString["ClimbId"];
			if (climbId == null)
			{
				climbId = "4";
			}

			ClimbCollection climbs = GetClimbs("where id=" + climbId);
			if (climbs.Count == 0)
			{
				p_description.InnerHtml = "<B>climb  " + climbId + " not found.</br>";
				return;
			}
			Climb climb = climbs[0];

			Master.Title = "BicycleClimbs.com " + climb.Name;
			//p_title.Text = "BicycleClimbs.com " + climb.Name;
			Master.PageHeading = climb.Name;

			p_description.InnerHtml = climb.Description;
#if fred
			//p_gradient.ImageUrl = "http://www.bicycleclimbs.com/climbdata/" + climb.Id.ToString() + "_profile.jpg";
			string dir = p_gradient.TemplateSourceDirectory;
			string path = Page.MapPath(p_gradient.TemplateSourceDirectory);
			string gradientFilename = climb.CreateGraph(null);
			p_gradient.ImageUrl = Context.Request.ApplicationPath + "/climbdata/" + gradientFilename;
			p_map.ImageUrl = Context.Request.ApplicationPath + "/climbdata/" + climb.Id.ToString() + "_map.jpg";
#endif
			//Climb climb = new Climb(new DataReaderFake());
			climb.PopulateDetailTable(detailTable);

		}
		catch (Exception exc)
		{
			Response.Write(exc.ToString());
		}
	}
}