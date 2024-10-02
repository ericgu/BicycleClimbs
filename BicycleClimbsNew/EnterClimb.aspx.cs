using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class EnterClimb : System.Web.UI.Page
{
	string m_climbIdString;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Master.LoadCookie()) 
		{
			p_error.InnerHtml = "<b>To enter climbs, create a login and send email to <a href=\"mailto:Eric.Gunnerson@Microsoft.Com\">Eric</a> to get privileges</b>";
			return;
		}

		if (!Master.CookieUser.CanCreateClimb)
		{
			p_error.InnerHtml = "<b>You are not allowed to add climbs</b>";
			return;
		}

		p_password.TextMode = TextBoxMode.Password;
        m_climbIdString = Context.Request.QueryString["ClimbId"];

		if (!IsPostBack)
		{
			RegionCollection regions = new RegionCollection();
			regions.Load();

			List<string> ratings = new List<string>();
			ratings.Add("A");
			ratings.Add("B");
			ratings.Add("C");

			p_rating.DataSource = ratings;
			p_rating.DataBind();

			p_regionList.DataSource = regions;
			p_regionList.DataTextField = "Name";
			p_regionList.DataValueField = "Id";
			p_regionList.DataBind();

			if (m_climbIdString != null)
			{
				LoadClimb(m_climbIdString);
			}
			else 
			{
				p_latitudeStart.Text = "0";
				p_longitudeStart.Text = "0";
				p_length.Text = "0";
				p_elevationGain.Text = "0";
				p_maxGradient.Text = "0";
				p_rating.Text = "C";

                p_latitudeText.Visible = false;
                p_latitudeStart.Visible = false;
                p_longitudeText.Visible = false;
                p_longitudeStart.Visible = false;
                p_lengthText.Visible = false;
                p_length.Visible = false;
                p_elevationGainText.Visible = false;
                p_elevationGain.Visible = false;

				p_regionList.SelectedValue = Master.CookieUser.RegionId.ToString();
			}
		}
		else
		{
			if (Master.CookieUser.Password != p_password.Text)
			{
				p_passwordError.InnerHtml = "<color=red>Incorrect password</color>";
				return;
			}

			Climb climb = new Climb();
			climb.Name = p_name.Text;
			climb.Location = p_location.Text;
			climb.LatitudeStart = Double.Parse(p_latitudeStart.Text);
			climb.LongitudeStart = Double.Parse(p_longitudeStart.Text);
			climb.Length = Double.Parse(p_length.Text);
			climb.ElevationGain = Double.Parse(p_elevationGain.Text);
			climb.MaxGradient = Double.Parse(p_maxGradient.Text);
			climb.Description = p_description.Text;
			climb.RegionId = Int32.Parse(p_regionList.SelectedValue);
			climb.Rating = p_rating.Text;

			int climbId;
			if (m_climbIdString == null)
			{
				climbId = climb.InsertIntoTable();
			}
			else
			{
				climbId = int.Parse(m_climbIdString);
				climb.Id = climbId;
				climb.Update();
			}

			Response.Redirect("DrawRoute.aspx?ClimbId=" + climbId.ToString());
		}
	}

	private void LoadClimb(string climbIdString)
	{
		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate("where id=" + climbIdString);
		Climb climb = climbs[0];

		p_regionList.SelectedValue = climb.RegionId.ToString();

		p_name.Text = climb.Name;
		p_location.Text = climb.Location;
		p_latitudeStart.Text = climb.LatitudeStart.ToString();
		p_longitudeStart.Text = climb.LongitudeStart.ToString();
		p_length.Text = climb.Length.ToString();
		p_elevationGain.Text = climb.ElevationGain.ToString();
		p_maxGradient.Text = climb.MaxGradient.ToString();
		p_description.Text = climb.Description;

		if (climb.Rating == "")
		{
			climb.Rating = "C";
		}

		p_rating.SelectedValue = climb.Rating.ToUpper();
	}
}
