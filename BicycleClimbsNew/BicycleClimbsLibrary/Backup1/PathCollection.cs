using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Xml;


namespace BicycleClimbsLibrary
{
	public class PathCollection: List<PathPoint>
	{
		List<PathSegment> segments;

		public void FetchPathForClimb(int climbId)
		{
			string query = @"select Longitude, Latitude from climbPaths where climbid = " + climbId.ToString() + " order by number";

			DataReader reader = Database.ExecuteReader(query);

			PathPoint lastPoint = null;
			while (reader.Read())
			{
				PathPoint pathPoint = new PathPoint(reader);
				if (lastPoint != null)
				{
					if (lastPoint.Latitude != pathPoint.Latitude ||
						lastPoint.Longitude != pathPoint.Longitude)
					{
						Add(pathPoint);
					}
				}
				else
				{
					Add(pathPoint);
				}
				lastPoint = pathPoint;
			}
        }

		public void SavePathForClimb(int climbId)
		{
            string query = String.Format(@"delete from climbpaths where climbid={0}", climbId);
            Database.ExecuteNonQuery(query);
            query = String.Format(@"delete from climbpathelevation where climbid={0}", climbId);
            Database.ExecuteNonQuery(query);

            int number = 0;
			foreach (PathPoint point in this)
			{
				query = String.Format(@"Insert into climbpaths(climbid, number, latitude, longitude) values ({0}, {1}, {2}, {3})",
										climbId, number, point.Latitude, point.Longitude);
				Database.ExecuteNonQuery(query);
				number++;
			}
		}

		public Location CenterPoint
		{
			get
			{
				Location centerPoint = new Location(0, 0);
				if (this.Count != 0)
				{
					centerPoint.Latitude = (this[0].Latitude + this[this.Count - 1].Latitude) / 2;
					centerPoint.Longitude = (this[0].Longitude + this[this.Count - 1].Longitude) / 2;
				}

				return centerPoint;
			}
		}

		public void CreateSegmentList()
		{
			segments = new List<PathSegment>();

			for (int i = 1; i < Count; i++)
			{
				PathSegment segment = new PathSegment(this[i - 1], this[i]);
				segments.Add(segment);
			}
		}

		public void SetSegmentList(List<PathSegment> segments)	// used for test...
		{
			this.segments = segments;
		}

			// Expand the list of points into an interpolated list of points for the graph
		public List<PathPoint> InterpolateForGraph(int xCount)
		{
			double distance = 0;
			foreach (PathSegment seg in segments)
			{
				distance += seg.Distance;
			}

			List<PathPoint> points = new List<PathPoint>();

			double increment = distance / xCount;

			double accumulatedDistance = 0;
			int currentSegment = 0;
			PathSegment segment = segments[currentSegment];
			for (double x = 0; x <= distance; x += increment)
			{
					// see if we need to switch to the next 
				if (x > accumulatedDistance + segment.Distance)
				{
					accumulatedDistance += segment.Distance;
					currentSegment++;
					segment = segments[currentSegment];
				}

				PathPoint currentPoint = segment.Interpolate(accumulatedDistance, accumulatedDistance + segment.Distance, x);

				points.Add(currentPoint);
			}

			return points;
		}

		public void CreateClimbPathElevation(int climbId)
		{
			string query = @"select count(*) from climbpathelevation where climbid=" + climbId.ToString();
			int rows = (int) Database.ExecuteScalar(query);
			if (rows != 0)
			{
				return;
			}
			
			FetchPathForClimb(climbId);
			if (Count == 0)
			{
				return;
			}
			CreateSegmentList();

			List<PathPoint> points = InterpolateForGraph(250);

			int number = 0;
			foreach (PathPoint point in points)
			{
				query = String.Format(@"Insert into climbpathelevation(climbid, number, latitude, longitude, Elevation) values ({0}, {1}, {2}, {3}, {4})",
										climbId, number, point.Latitude, point.Longitude, PathElevationPoint.ElevationUndefined);
				Database.ExecuteNonQuery(query);
				number++;
			}
		}

		public XmlElement Xml
		{
			get
			{
				Limits limitsLatitude = new Limits();
				Limits limitsLongitude = new Limits();

				foreach (PathPoint pathPoint in this)
				{
					limitsLatitude.UpdateMinMax(pathPoint.Latitude);
					limitsLongitude.UpdateMinMax(pathPoint.Longitude);
				}

				XmlDocument document = new XmlDocument();

				XmlElement page = document.CreateElement("page");
				document.AppendChild(page);

				XmlElement center = document.CreateElement("centerpoint");
				center.SetAttribute("lat", limitsLatitude.Middle.ToString());
				center.SetAttribute("lng", limitsLongitude.Middle.ToString());
				page.AppendChild(center);

				foreach (PathPoint pathPoint in this)
				{
					XmlElement point = document.CreateElement("point");
					point.SetAttribute("lat", pathPoint.Latitude.ToString());
					point.SetAttribute("lng", pathPoint.Longitude.ToString());
					page.AppendChild(point);
				}

				return page;
			}
		}
	}
}


