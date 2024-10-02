using System;
using System.Collections.Generic;
using System.Text;

namespace BicycleClimbsLibrary
{
	public class PathSegment
	{
		PathPoint point1;
		PathPoint point2;
		double distance = double.NaN;

		public PathSegment(PathPoint point1, PathPoint point2)
		{
			this.point1 = point1;
			this.point2 = point2;
		}

		public PathSegment(PathElevationPoint point1, PathElevationPoint point2)
		{
			this.point1 = new PathPoint();
			this.point1.Latitude = point1.Latitude;
			this.point1.Longitude = point1.Longitude;

			this.point2 = new PathPoint();
			this.point2.Latitude = point2.Latitude;
			this.point2.Longitude = point2.Longitude;
		}

		private double Arccos(double x)
		{
			if (x == 1)
			{
				return 0;
			}
			return Math.Atan(-x / Math.Sqrt(-x * x + 1)) + 2 * Math.Atan(1);
		}

		const double factor = Math.PI / 180.0;

		public double Distance
		{
			get
			{
				if (double.IsNaN(distance))
				{
					double p1Lat = point1.Latitude * factor;
					double p1Long = point1.Longitude * factor;
					double p2Lat = point2.Latitude * factor;
					double p2Long = point2.Longitude * factor;

					double radius = 3958 * 5280;

					distance = Arccos(Math.Sin(p1Lat) * Math.Sin(p2Lat) +
								     Math.Cos(p1Lat) * Math.Cos(p2Lat) * Math.Cos(p2Long - 
									 p1Long)) * radius;
				}
				return distance;
			}
			set
			{
				distance = value;
			}
		}

		public PathPoint Interpolate(double start, double end, double current)
		{
			double ratioTwo = (current - start) / (end - start);
			double ratioOne = 1.0 - ratioTwo;

			PathPoint point = new PathPoint();
			point.Latitude = point1.Latitude * ratioOne + point2.Latitude * ratioTwo;
			point.Longitude = point1.Longitude * ratioOne + point2.Longitude * ratioTwo;
			return point;
		}
	}
}
