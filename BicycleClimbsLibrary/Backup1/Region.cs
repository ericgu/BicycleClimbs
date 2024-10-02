#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Drawing;

namespace BicycleClimbsLibrary
{
    public class Region
    {
        int id;
        string name;
		double latitude;
		double longitude;
		int zoom;
 
		public Region()
		{
		}

        public Region(IDataReader reader)
        {
            id = (int) reader[0];
            name = (string) reader[1];
			Latitude = (double) reader[2];
			Longitude = (double) reader[3];
			Zoom = (int) reader[4];
		}

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public double Longitude
		{
			get { return longitude; }
			set { longitude = value; }
		}

		public double Latitude
		{
			get { return latitude; }
			set { latitude = value; }
		}

		public int Zoom
		{
			get { return zoom; }
			set { zoom = value; }
		}
	}
}
