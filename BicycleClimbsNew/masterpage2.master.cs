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

public partial class MasterPage : System.Web.UI.MasterPage
{
	bool m_onRealServer;
	User m_cookieUser;

	public MasterPage()
	{
		string computerName = System.Environment.MachineName;

		m_onRealServer = (computerName == "ERICGU01");
	}
	
	public bool LoadCookie()
	{
		HttpCookie cookie = Request.Cookies["UserId"];
		if (cookie != null)
		{
			int userId = Int32.Parse(cookie.Value);
			m_cookieUser = UserCollection.LoadUser(userId);
            p_header.User = m_cookieUser;
			return true;
		}
		else
		{
			return false;
		}
	}

	public User CookieUser
	{
		get { return m_cookieUser; }
        set { 
            m_cookieUser = value;
            p_header.User = m_cookieUser;
        }
	}

	public bool OnRealServer
	{
		get
		{
			return m_onRealServer;
		}
	}

	public string Title
	{
		get
		{
			return this.TitleControl.Text;
		}
		set
		{
			this.TitleControl.Text = value;
		}
	}

	public string PageHeading
	{
		get
		{
			return this.PageHeadingControl.InnerText;
		}
		set
		{
			this.PageHeadingControl.InnerText = value;
		}
	}

	public ShowMap Map
	{
		get
		{
			return this.p_map;
		}
	}
}
