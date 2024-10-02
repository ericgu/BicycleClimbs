using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

using System.ServiceModel.DomainServices.Client;
using Microsoft.Maps.MapControl;
using BicycleClimbsSilverlight;

namespace BicycleClimbsSilverlight
{
    public class ClimbPathCollection
    {
        BrushManager _brushManager;
        MapLayer _climbPathLayer;

        public ClimbPathCollection(BrushManager brushManager, MapLayer climbPathLayer)
        {
            _brushManager = brushManager;
            _climbPathLayer = climbPathLayer;
        }

        List<ClimbPathElevation> _climbPathElevations;

        public List<ClimbPathElevation> ClimbPathElevations
        {
            get { return _climbPathElevations; }
            set 
            {
                _boundingRectangle = null;
                _climbPathElevations = value; 
            }
        }

        LocationRect _boundingRectangle = null;

        public LocationRect BoundingRectangle
        {
            get
            {
                if (_boundingRectangle == null)
                {
                    SetBoundingRectangle();
                }

                return _boundingRectangle;
            }
        }

        public Location Center
        {
            get
            {
                LocationRect rect = BoundingRectangle;

                return new Location((rect.North + rect.South) / 2,
                                    (rect.East + rect.West) / 2);
            }
        }

        public void SetBoundingRectangle()
        {
            if (_climbPathElevations == null)
            {
                return;
            }

            double minLongitude = Double.MaxValue;
            double maxLongitude = Double.MinValue;
            double minLatitude = Double.MaxValue;
            double maxLatitude = Double.MinValue;

            foreach (ClimbPathElevation climbPathElevation in _climbPathElevations)
            {
                if (climbPathElevation.Latitude > maxLatitude)
                {
                    maxLatitude = climbPathElevation.Latitude;
                }

                if (climbPathElevation.Latitude < minLatitude)
                {
                    minLatitude = climbPathElevation.Latitude;
                }

                if (climbPathElevation.Longitude < minLongitude)
                {
                    minLongitude = climbPathElevation.Longitude;
                }

                if (climbPathElevation.Longitude > maxLongitude)
                {
                    maxLongitude = climbPathElevation.Longitude;
                }
            }

            _boundingRectangle = new LocationRect(maxLatitude, minLongitude, minLatitude, maxLongitude);
        }

        const int smoothBandSize = 5;
        public List<double> GetSmoothedElevations()
        {
            List<double> smoothed = new List<double>(_climbPathElevations.Count);

            for (int i = 0; i < _climbPathElevations.Count; i++)
            {
                int start = i - smoothBandSize;
                if (start < 0)
                {
                    start = 0;
                }
                int end = i + smoothBandSize;
                if (end >= _climbPathElevations.Count)
                {
                    end = _climbPathElevations.Count - 1;
                }

                double elevation = 0;
                for (int j = start; j <= end; j++)
                {
                    elevation += _climbPathElevations[j].Elevation;
                }

                elevation /= end - start + 1;
                smoothed.Add(elevation);
            }

            return smoothed;
        }


        public void CalculateGradients()
        {
            _gradients = new List<double>(_climbPathElevations.Count);
            List<double> smoothedElevations = GetSmoothedElevations();

            _gradients.Add(0);
            for (int i = 1; i < _climbPathElevations.Count; i++)
            {
                double gradient = (smoothedElevations[i] - smoothedElevations[i - 1]) /
                                DistanceBetweenPoints(_climbPathElevations[i], _climbPathElevations[i - 1]);
                _gradients.Add(gradient);
            }
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

        public double DistanceBetweenPoints(ClimbPathElevation point1, ClimbPathElevation point2)
        {
            double p1Lat = point1.Latitude * factor;
            double p1Long = point1.Longitude * factor;
            double p2Lat = point2.Latitude * factor;
            double p2Long = point2.Longitude * factor;

            double radius = 3958 * 5280;

            double distance = Arccos(Math.Sin(p1Lat) * Math.Sin(p2Lat) +
                                 Math.Cos(p1Lat) * Math.Cos(p2Lat) * Math.Cos(p2Long -
                                 p1Long)) * radius;
            return distance;
        }

        Location _locationLast;
        System.Windows.Threading.DispatcherTimer _myDispatcherTimer;
        int _currentPoint;
        const int PointIncrement = 10;
        List<double> _gradients;

        public void StartAnimation(double climbLengthInFeet)
        {
            int millisecondsPerChunk = 3 * (int)
                ((climbLengthInFeet / 5280.0 ) *       // number of seconds
                (1000.0 / _climbPathElevations.Count));

            _currentPoint = 0;
            if (_climbPathElevations.Count != 0)
            {
                ClimbPathElevation climbPathElevationStart = _climbPathElevations[_currentPoint];
                _locationLast = new Location(climbPathElevationStart.Latitude, climbPathElevationStart.Longitude);

                StartTimer(millisecondsPerChunk);
                //UpdateClimbPath();
            }
        }

        public void UpdateClimbPath(object o, EventArgs sender)
        {
            if (_currentPoint < _climbPathElevations.Count)
            {
                for (int pointIndex = _currentPoint; pointIndex < _currentPoint + PointIncrement; pointIndex++)
                {
                    if (pointIndex < _climbPathElevations.Count)
                    {
                        ClimbPathElevation climbPathElevation = _climbPathElevations[pointIndex];
                        Location location = new Location(climbPathElevation.Latitude, climbPathElevation.Longitude);

                        MapPolyline shape = new MapPolyline();
                        //shape.Stroke = lastBrush;
                        shape.Stroke = _brushManager.GetBrush(_gradients[pointIndex]);
                        shape.Locations = new LocationCollection();
                        shape.StrokeThickness = 3;

                        _climbPathLayer.Children.Add(shape);

                        shape.Locations.Add(_locationLast);
                        shape.Locations.Add(location);
                        _locationLast = location;
                    }
                }
            }

            _currentPoint += PointIncrement;
            if (_currentPoint > _climbPathElevations.Count)
            {
                StopTimer();
            }
        }

        private void StartTimer(int intervalInMilliseconds)
        {
            StopTimer();

            _myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            _myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, intervalInMilliseconds);
            _myDispatcherTimer.Tick += new EventHandler(UpdateClimbPath);
            _myDispatcherTimer.Start();
        }

        public void StopTimer()
        {
            if (_myDispatcherTimer != null)
            {
                _myDispatcherTimer.Stop();
                _myDispatcherTimer = null;
            }
        }

    }
}
