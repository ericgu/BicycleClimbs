using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

namespace BicycleClimbsLibrary
{
	public class PathElevationPoint
	{
		public const int ElevationUndefined = -1000;
		public const int ElevationNoValue = -100;

		public int Number;
		public double Latitude;
		public double Longitude;
		public double Elevation;

		public PathElevationPoint()
		{
		}

        public PathElevationPoint(IDataReader reader)
        {
            Latitude = (double) reader[0];
            Longitude = (double) reader[1];
			Elevation = (double) reader[2];
			Number = (int) reader[3];
 		}

		internal void SaveToDatabase(int climbId)
		{
			string query = String.Format(@"update ClimbPathElevation set elevation={0} where climbid={1} and number={2}",
										 Elevation, climbId, Number);

			Database.ExecuteNonQuery(query);
		}
	}
}
