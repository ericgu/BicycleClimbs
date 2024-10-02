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

public partial class InfoWindow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string regionId = Context.Request.QueryString["RegionId"];
        string markerId = Context.Request.QueryString["MarkerId"];

        if (regionId == null)
        {
            regionId = "0";
        }
        if (markerId == null)
        {
            markerId = "4";
        }
        int markerIdInt = Int32.Parse(markerId);

        ClimbCollection climbs = new ClimbCollection();
        climbs.Populate("where regionid=" + regionId + " order by name");

        Climb climb = climbs[markerIdInt];
        if (climb == null)
        {
            Response.Write("<B>climb  " + markerId + " not found.</br>");
            return;
        }

        p_details.BorderWidth = 3;
        p_details.BorderStyle = BorderStyle.Ridge;
        p_details.CellPadding = 5;
        p_details.CellSpacing = 0;
        AddRow(p_details, "Length", climb.LengthInMiles.ToString() + " miles");
        AddRow(p_details, "Elevation Gain", climb.ElevationGain.ToString() + " feet");
        AddRow(p_details, "Gradient", climb.Gradient.ToString() + " %");
        AddRow(p_details, "Max Gradient", climb.MaxGradient.ToString() + " %");

        p_climbName.Text = String.Format("<h3><a target='_climbWindow' href='climbdetail.aspx?ClimbId={0}'>{1}</a></h3>", climb.Id, climb.Name);

    }

    void AddRow(Table table, string cell1, string cell2)
    {
        TableRow row = new TableRow();
        TableCell cell;

        cell = new TableCell();
        cell.Text = cell1;
        cell.BorderStyle = BorderStyle.Ridge;
        row.Cells.Add(cell);
        cell = new TableCell();
        cell.Text = cell2;
        cell.HorizontalAlign = HorizontalAlign.Right;
        cell.BorderStyle = BorderStyle.Ridge;
        row.Cells.Add(cell);

        table.Rows.Add(row);
    }
}
