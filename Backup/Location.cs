using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
	public class Location
	{
		public double Latitude;
		public double Longitude;

		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
	}
}
