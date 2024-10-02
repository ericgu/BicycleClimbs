using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
	public class PathPoint
	{
		public double Latitude;
		public double Longitude;

		public PathPoint()
		{
		}

		public PathPoint(IDataReader reader)
		{
			Longitude = (double) reader[0];
			Latitude = (double) reader[1];
		}
	}
}