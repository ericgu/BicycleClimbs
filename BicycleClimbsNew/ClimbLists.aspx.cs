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
using System.Data.SqlClient;
using System.Data.OleDb;

public partial class ClimbLists : System.Web.UI.Page
{
	int m_regionIdInt = 1;
	int m_userId = -1;
    ClimbCollection m_climbs;

	private void AddHeaderLink(TableRow headerRow, string label, string url)
	{
		TableCell cell;
		cell = new TableCell();
		cell.Text = @"<a href=""" + url + @"""><b>" + label + "</b></a>";
		cell.HorizontalAlign = HorizontalAlign.Center;
		headerRow.Cells.Add(cell);
	}

	private void AddDataCell(TableRow headerRow, string text, HorizontalAlign alignment)
	{
		TableCell cell;
		cell = new TableCell();
		cell.Text = text;
		cell.HorizontalAlign = alignment;
		headerRow.Cells.Add(cell);
	}

	public void GetClimbs(int regionId, string sortColumn)
	{
		string sortClause = "";
		switch (sortColumn)
		{
			case "Name":
				sortClause = "order by Name";
				break;

			case "Location":
				sortClause = "order by Location";
				break;

			case "Length":
				sortClause = "order by length desc";
				break;

			case "Elevation Gain":
			default:
				sortClause = "order by ElevationGain desc";
				break;

			case "Gradient":
				sortClause = "and length<>0 order by ElevationGain/Length desc";
				break;

			case "Max Gradient":
				sortClause = "order by MaxGradient desc";
				break;
		}

		m_climbs = new ClimbCollection();
		m_climbs.Populate("where regionid=" + regionId.ToString() + " " + sortClause);
		if (m_userId != -1)
		{
			m_climbs.LoadCompletedData(m_userId);
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		string sort = Context.Request.QueryString["Sort"];
		if (sort == null)
		{
			sort = "ElevationGain";
		}

		User user = null;
		if (Request.Cookies["UserId"] != null)
		{
			HttpCookie cookie = Request.Cookies["UserId"];
			m_userId = Int32.Parse(cookie.Value);
			user = UserCollection.LoadUser(m_userId);
            Master.CookieUser = user;
		}

		if (!Page.IsPostBack)
		{
			if (user == null)
			{
				string regionId = Context.Request.QueryString["RegionId"];
				if (regionId == null)
				{
					regionId = "1";
				}
				m_regionIdInt = Int32.Parse(regionId);
			}
			else
			{
				m_regionIdInt = user.RegionId;
			}
		}
		else
		{
			m_regionIdInt = Int32.Parse(p_regionList.SelectedValue);
		}

		RegionCollection regions = new RegionCollection();
		regions.Load();


		p_regionList.DataSource = regions;
		p_regionList.DataTextField = "Name";
		p_regionList.DataValueField = "Id";
		p_regionList.DataBind();

		p_regionList.AutoPostBack = true;
		p_regionList.SelectedValue = m_regionIdInt.ToString();

		Master.Title = "BicycleClimbs.com - climbs sorted by " + sort;
		Master.PageHeading = "Climbs sorted by " + sort;

		GetClimbs(m_regionIdInt, sort);
		p_gridView.DataSource = m_climbs;
		p_gridView.AutoGenerateColumns = false;
		p_gridView.AllowSorting = true;
		p_gridView.Sorting += new GridViewSortEventHandler(p_gridView_Sorting);
        p_updateCompleted.Click += new EventHandler(p_updateCompleted_Click);

		if (!Page.IsPostBack)
		{
			HyperLinkField nameColumn = new HyperLinkField();
			nameColumn.HeaderText = "Name";
			nameColumn.DataTextField = "Name";
			nameColumn.SortExpression = "Name";
            nameColumn.Target = "_ClimbWindow";
			string[] urlFields = new string[1];
			urlFields[0] = "DetailUrl";
			nameColumn.DataNavigateUrlFields = urlFields;
			p_gridView.Columns.Add(nameColumn);

            CreateGridColumn("Location", "Location", null);
            p_gridView.Columns[2].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
			CreateGridColumn("LengthInMiles", "Length", null);
			CreateGridColumn("ElevationGain", "Elevation Gain", null);
			CreateGridColumn("Gradient", "Gradient", "{0:##.0}");
			CreateGridColumn("MaxGradient", "Max Gradient", null);

            p_gridView.DataBind();
        }
    }

    void p_updateCompleted_Click(object sender, EventArgs e)
    {
        if (m_userId == -1)
        {
            return;
        }

        foreach (GridViewRow row in p_gridView.Rows)
        {
            CheckBox completedControl = (CheckBox) row.FindControl("Completed");
            HiddenField idControl = (HiddenField) row.FindControl("Id");
            bool completed = completedControl.Checked;
            int id = Int32.Parse(idControl.Value);

            m_climbs.UpdateCompleted(m_userId, id, completed);

        }
    }

	void p_gridView_Sorting(object sender, GridViewSortEventArgs e)
	{
		Master.PageHeading = "Climbs sorted by " + e.SortExpression;

		GetClimbs(m_regionIdInt, e.SortExpression);
		p_gridView.DataSource = m_climbs;
		p_gridView.DataBind();
	}

	private void CreateGridColumn(string name, string sortExpression, string dataFormatString)
	{
		TableItemStyle itemStyle = new TableItemStyle();
		BoundField lengthColumn = new BoundField();
		lengthColumn.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
		lengthColumn.DataField = name;
		lengthColumn.HeaderText = sortExpression;
		lengthColumn.SortExpression = sortExpression;
		lengthColumn.DataFormatString = dataFormatString;
		p_gridView.Columns.Add(lengthColumn);
	}
}
