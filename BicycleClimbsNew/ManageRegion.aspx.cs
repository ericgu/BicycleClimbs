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

public partial class ManageRegion : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Master.LoadCookie();
		if (!Master.CookieUser.CanEditUsers)
		{
			p_error.InnerHtml = "<b>You are not allowed to edit regions</b>";
			return;
		}

		if (!Page.IsPostBack)
		{
			RegionCollection regions = new RegionCollection();
			regions.Load();

			p_repeater.DataSource = regions;
			p_repeater.DataBind();
		}
		p_repeater.ItemCommand += new RepeaterCommandEventHandler(p_repeater_ItemCommand);
		p_createButton.Click += new EventHandler(p_createButton_Click);
	}

	void p_createButton_Click(object sender, EventArgs e)
	{
		Region region = new Region();
		region.Name = p_name.Text;
		region.Latitude = double.Parse(p_latitude.Text);
		region.Longitude = double.Parse(p_longitude.Text);
		region.Zoom = Int32.Parse(p_zoom.Text);

		if (RegionCollection.InsertRegion(region) == -1)
		{
			p_error.InnerHtml = "Duplicate region name - please choose another";
		}
		else
		{
			Response.Redirect("ManageRegion.aspx");
		}
	}

	protected void p_repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		int regionId = Int32.Parse((string) e.CommandArgument);
		Region region = RegionCollection.Load(regionId);

		switch (e.CommandName)
		{
			case "Update":
				TextBox longitude = (TextBox) e.Item.Controls[1];
				TextBox latitude = (TextBox) e.Item.Controls[3];
				region.Longitude = double.Parse(longitude.Text);
				region.Latitude = double.Parse(latitude.Text);
				TextBox zoom = (TextBox) e.Item.Controls[5];
				region.Zoom = Int32.Parse(zoom.Text);

				RegionCollection.SaveRegion(region);
				break;

			case "Delete":
				RegionCollection.DeleteRegion(region);
				break;
		}

		Response.Redirect("ManageRegion.aspx");
	}
}
