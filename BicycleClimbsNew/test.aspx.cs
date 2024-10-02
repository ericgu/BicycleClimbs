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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;

using BicycleClimbsLibrary;

public partial class test : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
	}

	public OleDbConnection OpenConnection()
	{
		string connect = "Provider=SQLOLEDB;Data Source=66.186.25.159;User Id=ericgu;Password=eric1bike2;Initial Catalog=BicycleClimbs";

		OleDbConnection conn = new OleDbConnection(connect);
		conn.Open();
		return conn;

	}
}
