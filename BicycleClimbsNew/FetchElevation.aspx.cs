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

public partial class FetchElevation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string latitude = Context.Request.QueryString["Latitude"];
        string longitude = Context.Request.QueryString["Longitude"];

        string response = "";

        if (latitude != null)
        {
            double latitudeValue = Double.Parse(latitude);
            double longitudeValue = Double.Parse(longitude);

            ElevationWebService service = new ElevationWebService();

            double elevation = service.FetchElevation(latitudeValue, longitudeValue, false);

            if (elevation < -100000)
            {
                elevation = -1000000;
            }

            //response = string.Format("({0}, {1}, {2})", latitude, longitude, elevation);
            response = string.Format("{0}", (int) elevation);
        }

        Response.ContentType = "Text/plain";
        Response.Write(response);

        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
    }

}
