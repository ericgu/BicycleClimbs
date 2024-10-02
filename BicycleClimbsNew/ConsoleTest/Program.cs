using System;
using System.Collections.Generic;
using System.Text;
using BicycleClimbsLibrary;
using System.Xml;
using System.Data.OleDb;

namespace ConsoleTest
{
	class Program
	{
		static public void Main()
		{
#if fred
			Database database = new Database();
			OleDbConnection connection = database.OpenConnection();

			for (int climbID = 4; climbID < 67; climbID++)
			{
				ClimbFilename climbFilename = new ClimbFilename();
				climbFilename.Id = climbID;

				climbFilename.Map = true;
				climbFilename.Filename = climbID.ToString() + "_map.jpg";
				climbFilename.InsertIntoTable(connection);

				climbFilename.Map = false;
				climbFilename.Filename = climbID.ToString() + "_profile.jpg";
				climbFilename.InsertIntoTable(connection);
			}
#endif
		}
	}
}
