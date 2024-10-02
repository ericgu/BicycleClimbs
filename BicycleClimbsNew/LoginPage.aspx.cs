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

public partial class LoginPage : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		p_password.TextMode = TextBoxMode.Password;

		if (!Page.IsPostBack)
		{
            if (Request.UrlReferrer != null)
            {
                p_referrer.Value = Request.UrlReferrer.ToString();
            }

			if (Master.CookieUser != null)
			{
				p_username.Text = Master.CookieUser.Username;
			}
		}
		else
		{
			User user = UserCollection.LoadUser(p_username.Text);

			if (user != null && user.Password == p_password.Text)
			{
				HttpCookie cookie = new HttpCookie("UserId");
				DateTime expiration = DateTime.Now + new TimeSpan(10000, 0, 0, 0, 0);

				cookie.Expires = expiration;
				cookie.Value = user.Id.ToString();

				Response.Cookies.Add(cookie);
				Response.Redirect("http://www.bicycleclimbs.com");
			}
			else
			{
				p_error.InnerHtml = "<B>Error with username or password</b>";
			}
		}
	}
}
