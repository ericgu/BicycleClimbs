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

public partial class CreateLogin : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		string userId = Context.Request.QueryString["UserId"];
		if (userId == null)
		{
			userId = "-1";
		}
		int userIdInt = Int32.Parse(userId);

		RegionCollection regions = new RegionCollection();
		regions.Load();

		p_defaultRegion.DataSource = regions;
		p_defaultRegion.DataTextField = "Name";
		p_defaultRegion.DataValueField = "Id";

		p_password.TextMode = TextBoxMode.Password;
		p_password2.TextMode = TextBoxMode.Password;

		if (!Page.IsPostBack)
		{
			p_defaultRegion.DataBind();

			User user = UserCollection.LoadUser(userIdInt);

			if (user != null)
			{
				p_username.Text = user.Username;
				p_password.Text = user.Password;
				p_defaultRegion.SelectedValue = user.RegionId.ToString();
				p_email.Text = user.Email;
				p_createButton.Text = "Update";
			}
		}

		p_createButton.Click += new EventHandler(p_createButton_Click);
	}

	void p_createButton_Click(object sender, EventArgs e)
	{
		User user = new User();
		user.Username = p_username.Text;
		user.Password = p_password.Text;
		user.RegionId = Int32.Parse(p_defaultRegion.SelectedValue);
		user.Email = p_email.Text;

		if (p_password.Text != p_password2.Text)
		{
			p_error.InnerHtml = "Passwords do not match";
		}
		else
		{
			if (UserCollection.InsertUser(user) == -1)
			{
				p_error.InnerHtml = "Duplicate username - please choose another";
			}
		}
	}
}
