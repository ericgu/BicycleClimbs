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

public partial class ClimbsNew : System.Web.UI.Page
{
	int regionIdInt = 1;

	protected void Page_Load(object sender, EventArgs e)
	{
        try
        {
            
            User user = null;
            if (Request.Cookies["UserId"] != null)
            {
                HttpCookie cookie = Request.Cookies["UserId"];
                int userId = Int32.Parse(cookie.Value);
                user = UserCollection.LoadUser(userId);
                Master.CookieUser = user;

                if (user != null)
                {
                    regionIdInt = user.RegionId;
                }
            }

            int completedFilterIndex = 0;
            if (!Page.IsPostBack)
            {
                p_completedFilter.Items.Clear();
                p_completedFilter.Items.Add("All");
                p_completedFilter.Items.Add("Completed");
                p_completedFilter.Items.Add("Not Completed");

                if (user == null)
                {
                    string regionId = Context.Request.QueryString["RegionId"];
                    if (regionId == null)
                    {
                        regionId = "1";
                    }
                    regionIdInt = Int32.Parse(regionId);
                }
                p_completedFilter.SelectedIndex = 0;
            }
            else
            {
                regionIdInt = Int32.Parse(p_regionList.SelectedValue);
                completedFilterIndex = p_completedFilter.SelectedIndex;
            }

            int userIdInt = user == null ? -1 : user.Id;
            Master.Map.LoadClimbs(regionIdInt, userIdInt, completedFilterIndex);

            Master.Title = "BicycleClimbs.com";
            Master.PageHeading = "";

            RegionCollection regions = new RegionCollection();
            regions.Load();

            p_regionList.DataSource = regions;
            p_regionList.DataTextField = "Name";
            p_regionList.DataValueField = "Id";
            p_regionList.DataBind();


            //p_regionList.SelectedIndexChanged += new EventHandler(p_regionList_SelectedIndexChanged);
            p_regionList.AutoPostBack = true;
            p_regionList.SelectedValue = regionIdInt.ToString();

            p_zoomIn.Click += new EventHandler(p_zoomIn_Click);
            p_zoomOut.Click += new EventHandler(p_zoomOut_Click);

        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }

	void p_zoomOut_Click(object sender, EventArgs e)
	{
		ModifyRegionZoom(1);
	}

	void p_zoomIn_Click(object sender, EventArgs e)
	{
		ModifyRegionZoom(-1);
	}

	private void ModifyRegionZoom(int inc)
	{
		if (!Master.LoadCookie())
		{
			return;
		}

		if (!Master.CookieUser.CanCreateClimb)
		{
			return;
		}

		Region region = RegionCollection.Load(regionIdInt);
		region.Zoom += inc;
		RegionCollection.SaveRegion(region);
		Response.Redirect("ClimbsNew.aspx?RegionId=" + regionIdInt.ToString());
	}

	void p_regionList_SelectedIndexChanged(object sender, EventArgs e)
	{
		int regionId = Int32.Parse(p_regionList.SelectedValue);
		Response.Redirect("ClimbsNew.aspx?RegionId=" + regionId.ToString());
	}
}
