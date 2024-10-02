using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Drawing;

namespace BicycleClimbsLibrary
{
	public class PathElevationCollection: List<PathElevationPoint>
	{
		public bool pathComplete = false;
		public int ClimbId;

		public void LoadPathForClimb(int climbId)
		{
			this.ClimbId = climbId;

			string query = @"select Latitude, Longitude, Elevation, Number from climbPathElevation where elevation <> -100 and climbid = " + climbId.ToString() + " order by number";

			DataReader reader = Database.ExecuteReader(query);
			while (reader.Read())
			{
				PathElevationPoint pathElevationPoint = new PathElevationPoint(reader);
				Add(pathElevationPoint);
			}
        }

		public int GetUpdateCount()
		{
			int itemsToFetch = 0;
			foreach (PathElevationPoint point in this)
			{
				if (point.Elevation == PathElevationPoint.ElevationUndefined)
				{
					itemsToFetch++;
				}
			}
			return itemsToFetch;
		}

		public void UpdateElevation()
		{
			if (GetUpdateCount() == 0)
			{
				return;
			}

			ElevationWebService service = new ElevationWebService();

			for (int i = Count - 1; i >= 0; i--)
			{
				PathElevationPoint point = this[i];

				if (point.Elevation == PathElevationPoint.ElevationUndefined)
				{
					try
					{
						point.Elevation = service.FetchElevation(point.Latitude, point.Longitude, false);
						if (point.Elevation < -100000)
						{
							point.Elevation = PathElevationPoint.ElevationNoValue;
						}
						//elevation += 5 - random.NextDouble() * 10;
						//point.Elevation = elevation;
						point.SaveToDatabase(ClimbId);
					}
					catch (Exception)
					{
						break;
					}
				}
				else if (point.Elevation == PathElevationPoint.ElevationNoValue)
				{
					this.RemoveAt(i);
				}
			}
		}

		public void UpdateClimbFromElevation(int climbId)
		{
			ClimbCollection climbs = new ClimbCollection();
			Climb climb = climbs.LoadClimb(climbId);

            int pointsToUpdate = GetUpdateCount();
            if (Count < 2 || pointsToUpdate != 0)

			if (Count == 0 || pointsToUpdate != 0 ||
				(climb.ElevationGain != 0.0 && climb.Length != 0.0))
			{ 
				return;
			}

			double maxElevation = Single.MinValue;
			double minElevation = Single.MaxValue;

			int minIndex = -1;
			int maxIndex = -1;
			for (int index = 0; index < Count; index++)
			{
				PathElevationPoint pathElevationPoint = this[index];

				if (pathElevationPoint.Elevation >= 0.0)
				{
					if (pathElevationPoint.Elevation > maxElevation)
					{
						maxElevation = pathElevationPoint.Elevation;
						maxIndex = index;
					}

					if (pathElevationPoint.Elevation < minElevation)
					{
						minElevation = pathElevationPoint.Elevation;
						minIndex = 0;
					}
				}
			}

				// figure out the length between min and max points
			if (maxIndex > minIndex)
			{
				PathElevationPoint lastPoint = this[minIndex];

				double distance = 0.0;
				for (int i = minIndex + 1; i <= maxIndex; i++)
				{
					PathElevationPoint point = this[i];

					PathSegment segment = new PathSegment(lastPoint, point);
					distance += segment.Distance;
					lastPoint = point;
				}

				climb.Length = (int) distance;
			}

			climb.ElevationGain = (int) (maxElevation - minElevation);
			Database database = new Database();

			climb.Update();
		}

		public Bitmap CreateGraph(string title)
		{
			int pointsToUpdate = GetUpdateCount();

			if (Count < 2 || pointsToUpdate != 0)
			{
				Bitmap bitmap = new Bitmap(700, 300);
				Graphics graphics = Graphics.FromImage(bitmap);

				Font font = new Font("Arial", 25);
				graphics.DrawString("Elevation Data to Fetch = " + pointsToUpdate.ToString(), font, Brushes.Goldenrod, 50, 150);

				return bitmap;
			}

			List<DataPoint> dataPoints = new List<DataPoint>();

			PathElevationPoint lastPoint = this[0];
			dataPoints.Add(new DataPoint(0, lastPoint.Elevation));

			double distance = 0.0;
			for (int i = 1; i < Count; i++)
			{
				PathElevationPoint point = this[i];
				PathPoint p1 = new PathPoint();

				PathSegment segment = new PathSegment(lastPoint, point);
				distance += segment.Distance;
				double distanceInMiles = distance / 5280.0;
				dataPoints.Add(new DataPoint(distanceInMiles, point.Elevation));

				lastPoint = point;
			}

			ProfileGraph graph = new ProfileGraph();
			graph.Points = dataPoints;
			graph.Title = title;

			return graph.CreateGraph(700, 300);
		}
	}
}
