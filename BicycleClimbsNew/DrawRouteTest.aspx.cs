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

public partial class DrawRouteTest : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		p_upload.Click += new EventHandler(p_upload_Click);
	}

	void p_upload_Click(object sender, EventArgs e)
	{
		string values = DrawnRoute.Text;
	}
}
