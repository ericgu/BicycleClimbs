using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;

using System.Data.SqlClient;

using BicycleClimbsLibrary;
using System.Data.OleDb;

public partial class ClimbDetail : System.Web.UI.Page
{
    Climb m_climb;
	int climbIdInt;

	public ClimbCollection GetClimbs(string query)
	{
		ClimbCollection climbs = new ClimbCollection();
		climbs.Populate(query);

		return climbs;
	}

	void GraphCallback(int currentItem, int totalItems)
	{
		if (currentItem <= 1)
		{
			Response.BufferOutput = false;
			Response.Write("Creating gradient map<br>");
		}

		Response.Write(String.Format("{0}/{1} ", currentItem, totalItems));
		Response.Flush();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			bool authenticated = Page.Request.IsAuthenticated;
			bool editClimb = Page.User.IsInRole("EditClimb");

            string climbId = Context.Request.QueryString["ClimbId"];
            if (climbId == null)
            {
                climbId = "4";
            }
            climbIdInt = Int32.Parse(climbId);
            
            if (Master.LoadCookie() && Master.CookieUser.CanCreateClimb)
            {
                p_editLink.InnerHtml = @"<a href=""enterClimb.aspx?ClimbId=" + climbId + @""">Edit</a>";
            }
             
			ClimbCollection climbs = GetClimbs("where id=" + climbId);
			if (climbs.Count == 0)
			{
				p_description.InnerHtml = "<B>climb  " + climbId + " not found.</br>";
				return;
			}

            if (Master.CookieUser != null)
            {
                climbs.LoadCompletedData(Master.CookieUser.Id);
            }
            m_climb = climbs[0];
            
            if (Page.IsPostBack)
            {
            }

            p_completed.Checked = m_climb.Completed;

			Master.Title = "BicycleClimbs.com " + m_climb.Name;
            Master.PageHeading = m_climb.Name;

            p_description.InnerHtml = m_climb.Description;
			p_wattage.InnerHtml = "<a href=\"Wattage.aspx?ClimbId=" + climbId + "\">Estimate Wattage</a>";

			//p_map.LoadClimb(climbIdInt);
			
			//p_gradient.ImageUrl = "http://www.bicycleclimbs.com/climbdata/" + climb.Id.ToString() + "_profile.jpg";
			string dir = p_gradient.TemplateSourceDirectory;
			string path = Page.MapPath(p_gradient.TemplateSourceDirectory);


			string filenamePath = Context.Request.ApplicationPath + "/climbdata/";
			try
			{
				string gradientFilename = "ProfileGraph.aspx?ClimbId=" + climbId;
				//string gradientFilename = climb.CreateGraph(path + "\\ClimbData", new PathElevationCollection.UpdateElevationCallback(GraphCallback));

				if (gradientFilename != null)
				{
					p_gradient.InnerHtml = String.Format(@"<img src=""{0}""><br>", gradientFilename);
				}
			}
			catch (Exception exc)
			{
				Response.Write("Error creating graph: " + exc.GetType().ToString());
			}
			ClimbFilenameCollection filenames = new ClimbFilenameCollection();
			filenames.Populate(climbIdInt);

			foreach (ClimbFilename climbFilename in filenames)
			{
				if (climbFilename.Map)
				{
					p_oldmap.InnerHtml += String.Format(@"<img src=""{0}""><br>", filenamePath + climbFilename.Filename);
				}
			}

			//p_log.InnerText = "Path: " + path + " filenamepath: " + filenamePath;

			//p_gradient.ImageUrl = filenamePath + gradientFilename;

			foreach (ClimbFilename climbFilename in filenames)
			{
				if (!climbFilename.Map)
				{
					p_gradient.InnerHtml += String.Format(@"<img src=""{0}""><br>", filenamePath + climbFilename.Filename);
				}
			}
			
			//p_map.ImageUrl = Context.Request.ApplicationPath + "/climbdata/" + climb.Id.ToString() + "_map.jpg";

			//Climb climb = new Climb(new DataReaderFake());
            m_climb.PopulateDetailTable(detailTable);

			Master.Map.LoadClimb(climbIdInt);

            ThreadPool.QueueUserWorkItem(new WaitCallback(m_climb.FetchElevations));
 		}
		catch (Exception exc)
		{
			Response.Write(exc.ToString());
		}
		p_zoomIn.Click += new EventHandler(p_zoomIn_Click);
		p_zoomOut.Click += new EventHandler(p_zoomOut_Click);
        p_completed.CheckedChanged += new EventHandler(p_completed_CheckedChanged);
	}

    void p_completed_CheckedChanged(object sender, EventArgs e)
    {
        m_climb.Completed = !m_climb.Completed;
        ClimbCollection climbs = new ClimbCollection();
        climbs.UpdateCompleted(Master.CookieUser.Id, climbIdInt, m_climb.Completed);
        Response.Redirect("ClimbDetail.aspx?ClimbId=" + m_climb.Id.ToString());
    }

	void p_zoomOut_Click(object sender, EventArgs e)
	{
		ModifyClimbZoom(1);
	}

	void p_zoomIn_Click(object sender, EventArgs e)
	{
		ModifyClimbZoom(-1);
	}

	private void ModifyClimbZoom(int inc)
	{
		if (!Master.LoadCookie())
		{
			return;
		}

		if (!Master.CookieUser.CanCreateClimb)
		{
			return;
		}

		m_climb.Zoom += inc;
		m_climb.Update();
		Response.Redirect("ClimbDetail.aspx?ClimbId=" + m_climb.Id.ToString());
	}
}


