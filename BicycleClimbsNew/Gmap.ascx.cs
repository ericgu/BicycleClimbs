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

public partial class Gmap : System.Web.UI.UserControl
{
	protected override void Render(HtmlTextWriter output)
	{
		string computerName = System.Environment.MachineName;
		string scriptHtml;

		if (computerName == "ERICGU01")
		{
            scriptHtml = "<script src=\"http://maps.google.com/maps?file=api&v=1&key=ABQIAAAAiZoAf08AX4ClzrJYaSSV9xT2yXp_ZAY8_ufC3CFXhHIE1NvwkxRqUx0HKyTGo-t3pphCukLqkccNiQ\" type=\"text/javascript\"></script>";
            scriptHtml = "<script src=\"http://maps.google.com/maps?file=api&v=2&key=ABQIAAAAiZoAf08AX4ClzrJYaSSV9xT2yXp_ZAY8_ufC3CFXhHIE1NvwkxRqUx0HKyTGo-t3pphCukLqkccNiQ\" type=\"text/javascript\"></script>";
        }
		else
		{
			scriptHtml = "<script src=\"http://maps.google.com/maps?file=api&v=1&key=ABQIAAAAiZoAf08AX4ClzrJYaSSV9xTBqWC_ogAKx9eKDib7mMgLWMMJihSZyN8JqLbPnobFKQ6qF5XkAEZRDg\" type=\"text/javascript\"></script>";
		}
		output.Write(scriptHtml);
	}
}
