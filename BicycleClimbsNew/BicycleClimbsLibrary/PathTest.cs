using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace BicycleClimbsLibrary
{
	[TestFixture]
	public class PathTest
	{
		[Test]
		public void TestPathRead()
		{
			DataReaderFake dataReaderFake = new DataReaderFake();

			dataReaderFake.Length = 3;
			dataReaderFake[0] = 1.0;
			dataReaderFake[1] = 2.0;
			dataReaderFake[2] = 3.0;

			PathElevationPoint point = new PathElevationPoint(dataReaderFake);

			Assert.AreEqual(1.0, point.Latitude);
			Assert.AreEqual(2.0, point.Longitude);
			Assert.AreEqual(3.0, point.Elevation);
		}

		[Test]
		public void TestPathSegment()
		{
			PathPoint point1 = new PathPoint();
			point1.Longitude = 2.0;
			point1.Latitude = 11.0;

			PathPoint point2 = new PathPoint();
			point2.Longitude = 4.0;
			point2.Latitude = 21.0;

			PathSegment segment = new PathSegment(point1, point2);

			PathPoint interp1 = segment.Interpolate(0, 10, 0);
			PathPoint interp2 = segment.Interpolate(0, 10, 5);
			PathPoint interp3 = segment.Interpolate(0, 10, 10);

			Assert.AreEqual(2.0, interp1.Longitude);
			Assert.AreEqual(11.0, interp1.Latitude);

			Assert.AreEqual(2.0, interp2.Longitude);
			Assert.AreEqual(16.0, interp2.Latitude);

			Assert.AreEqual(4.0, interp3.Longitude);
			Assert.AreEqual(21.0, interp3.Latitude);
		}

		[Test]
		public void TestPathLookup()
		{
			PathPoint point1 = new PathPoint();
			point1.Longitude = -122.0886;
			point1.Latitude = 47.6311;

			PathPoint point2 = new PathPoint();
			point2.Longitude = -122.1769;
			point2.Latitude = 47.6319;

			PathSegment segment = new PathSegment(point1, point2);

			ElevationWebService service = new ElevationWebService();
			double max = 50;
			PathPoint lastPoint = segment.Interpolate(0, max, 0.0);
			double lastElevation = service.FetchElevation(lastPoint.Latitude, lastPoint.Longitude, false);

			List<DataPoint> points = new List<DataPoint>();
			points.Add(new DataPoint(0, lastElevation));
			double x = 0;
			for (double current = 1.0; current < max; current += 1.0)
			{
				PathPoint point = segment.Interpolate(0, max, current);
				double elevation = service.FetchElevation(point.Latitude, point.Longitude, false);

				PathSegment tempSegment = new PathSegment(lastPoint, point);
				double distance = tempSegment.Distance;
				x += distance;

				DataPoint dataPoint = new DataPoint(x, elevation);
				points.Add(dataPoint);

				lastPoint = point;
				Console.WriteLine(current.ToString() + " " + elevation.ToString());
			}

			ProfileGraph graph = new ProfileGraph();
			graph.Title = "24th St";
			graph.Points = points;
			graph.CreateGraph(600, 300);
		}

		[Test]
		public void TestCreateChart()
		{
#if fred
			//--------------------------------------
			// Create The Chart
			ChartEngine engine = new ChartEngine();
			engine.Size = new System.Drawing.Size(600, 400);
			ChartCollection charts = new ChartCollection(engine);
			engine.Charts = charts;
			engine.ShowXValues = true;
			engine.ShowYValues = true;
			ChartText title = new ChartText();
			title.Text = "Gradient for 24th St";
			engine.Title = title;
			engine.ChartPadding = 25;
			engine.HasChartLegend = true;

			int pointCount = 0;

			ChartPointCollection data = new ChartPointCollection();
			Chart line = new LineChart(data, Color.Red);
			line.ShowLineMarkers = false;
			line.Fill.Color = Color.Red;
			engine.PlotBackground.Color = Color.LightBlue;
			engine.XTicksInterval = 10;
			engine.XValuesInterval = 10;
				int maxPoints = 200;
			for (pointCount = 0; pointCount < maxPoints; pointCount++)
			{
				data.Add(new ChartPoint(pointCount.ToString(), pointCount));
			}
			charts.Add(line);

			engine.GridLines = GridLines.Horizontal;

			engine.SaveToFile(@"c:\chart.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
#endif
		}

		[Test]
		public void TestSegmentInterpolate()
		{
			PathCollection paths = new PathCollection();

			PathPoint point1 = new PathPoint();
			point1.Longitude = 2.0;
			point1.Latitude = 11.0;
			paths.Add(point1);

			PathPoint point2 = new PathPoint();
			point2.Longitude = 4.0;
			point2.Latitude = 21.0;
			paths.Add(point2);

			PathPoint point3 = new PathPoint();
			point3.Longitude = 6.0;
			point3.Latitude = 41.0;
			paths.Add(point3);

			List<PathSegment> segments = new List<PathSegment>();

			PathSegment segment;

			segment = new PathSegment(point1, point2);
			segment.Distance = 5;
			segments.Add(segment);

			segment = new PathSegment(point2, point3);
			segment.Distance = 10;
			segments.Add(segment);

			paths.SetSegmentList(segments);

			TextWriter writer = File.CreateText(@"c:\path.txt");
			List<PathPoint> interpolated = paths.InterpolateForGraph(15);

			foreach (PathPoint point in interpolated)
			{
				writer.WriteLine(point.Longitude + " " + point.Latitude);
			}
			writer.Close();
		}

		[Test]
		public void TestDrawGraph()
		{
			ProfileGraph graph = new ProfileGraph();

			//Bitmap bitmap = graph.CreateGraph();

			//bitmap.Save(@"C:\profile.jpg", ImageFormat.Jpeg);
		}
	}
}
