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

public partial class MyHeaderControl : System.Web.UI.UserControl
{
    User m_user;

    public User User
    {
        get { return m_user; }
        set { m_user = value; }
    }

	protected void Page_Load(object sender, EventArgs e)
	{
		p_top.Width = 958;
		p_top.CellSpacing = 1;
		p_top.BackColor = System.Drawing.Color.Black;
		p_top.CellPadding = 5;

		TableRow row = new TableRow();

		TableCell name = new TableCell();
		name.Text = @"<font size=""5"" color=""#FFFFFF"">BicycleClimbs.com</font><p align=""left"">
			<font color=""#FF0000"" face=""Arial Black"" size=""2""><i>Because pain is 
			the sign of <br>
			weakness leaving your body</i></font>";
		name.Attributes.Add("align", "center");
		name.Width = 210;
		row.Cells.Add(name);

		TableCell adSense = new TableCell();
		adSense.Width = 728;
		adSense.Height = 90;
		adSense.Text = @"
			<script type=""text/javascript""><!--
				google_ad_client = ""pub-5589402870024226"";
				google_ad_width = 728;
				google_ad_height = 90;
				google_ad_format = ""728x90_as"";
				google_ad_type = ""text_image"";
				google_ad_channel ="""";
				google_color_border = ""003366"";
				google_color_bg = ""003366"";
				google_color_link = ""FF6600"";
				google_color_url = ""99CC99"";
				google_color_text = ""FFFFFF"";
		    //-->
			</script>
			<script type=""text/javascript""
				src=""http://pagead2.googlesyndication.com/pagead/show_ads.js"">
			</script>
		";

		row.Cells.Add(adSense);
		p_top.Rows.Add(row);

		p_button.Width = 958;
		p_button.CellSpacing = 1;
		p_button.BackColor = System.Drawing.Color.Black;
		p_button.CellPadding = 5;

		row = new TableRow();

		int buttonWidth = 120;
		row.Cells.Add(CreateButton(buttonWidth, "Home", "ClimbsNew.aspx"));
		row.Cells.Add(CreateButton(buttonWidth, "Lists of Climbs", "ClimbLists.aspx"));
		row.Cells.Add(CreateButton(buttonWidth, "About", "About.htm"));
		row.Cells.Add(CreateButton(buttonWidth, "RSS Feed", "RssFeeds.aspx"));
		row.Cells.Add(CreateButton(buttonWidth, "Login", "loginpage.aspx"));
		row.Cells.Add(CreateButton(958 - 5 * buttonWidth, "", ""));

		p_button.Rows.Add(row);

        p_status.Width = 958;
        p_status.CellSpacing = 1;
        p_status.BackColor = System.Drawing.Color.Black;
        p_status.CellPadding = 5;

        row = new TableRow();
        TableCell statusCell = new TableCell();
        statusCell.Width = 958;
        if (m_user != null)
        {
            string text = String.Format("User: {0}   Climbs / Completed = {1} / {2}",
                                        m_user.Username, m_user.ClimbsCompleted, m_user.ClimbsTotal);

            statusCell.Text = @"<font color=""#FFCC00"">" +
                                text +
                                @"</Font>";
        }
        row.Cells.Add(statusCell);
        p_status.Rows.Add(row);
	}

	public TableCell CreateButton(int width, string name, string url)
	{
		TableCell cell = new TableCell();
		cell.Text = @"
			<td align=""center"" width=""" + width.ToString() + @""">
			<font face=""Arial Black"" size=""2"" color=""#FFCC00"">
			<a href=""" + url + @"""><font color=""#FFCC00"">
			<span style=""text-decoration: none"">" + name + @"</span></font></a></font></td>
			";

		return cell;
	}
}
