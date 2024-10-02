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

public partial class EditUser : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		Master.LoadCookie();
		if (!Master.CookieUser.CanEditUsers)
		{
			p_error.InnerHtml = "<b>You are not allowed to edit users</b>";
			return;
		}

		if (!Page.IsPostBack)
		{
			UserCollection users = new UserCollection();
			users.LoadUsers();

			p_repeater.DataSource = users;
			p_repeater.DataBind();
		}
		p_repeater.ItemCommand += new RepeaterCommandEventHandler(p_repeater_ItemCommand);
	}

	protected void p_repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
	{
		int userId = Int32.Parse((string) e.CommandArgument);
		User user = UserCollection.LoadUser(userId);

		switch (e.CommandName)
		{
			case "CanEditUsers":
				user.CanEditUsers = !user.CanEditUsers;
				break;

			case "CanCreateClimb":
				user.CanCreateClimb = !user.CanCreateClimb;
				break;
		}

		UserCollection.SaveUser(user);
		Response.Redirect("EditUser.aspx");
	}
}