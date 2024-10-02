using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class ClimbFilename
	{
        public int Id;
		public bool Map;
        public string Filename;

		public ClimbFilename()
		{
		}

        public ClimbFilename(IDataReader reader)
        {
            Id = (int) reader[0];
			Map = (int) reader[1] == 1;
            Filename = (string) reader[2];
 		}

		public void InsertIntoTable()
		{
			string query = String.Format(
						@"insert into climbFiles(ClimbId, Map, Filename) " +
						@"values ({0}, {1}, '{2}')",
						Id, Map ? 1 : 0, Filename);
			Database.ExecuteNonQuery(query);
		}
	}
}
