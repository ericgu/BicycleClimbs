using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class ClimbFilenameCollection : List<ClimbFilename>
	{
		public void Populate(int climbID)
		{
			string query = @"select ClimbId, Map, Filename from climbFiles where climbID=" + climbID.ToString();

			DataReader reader = Database.ExecuteReader(query);

			while (reader.Read())
			{
				ClimbFilename climbFilename = new ClimbFilename(reader);
				Add(climbFilename);
			}
        }
	}
}
