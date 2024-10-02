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
using BicycleClimbsSilverlight.Web;

namespace BicycleClimbsSilverlight
{
    public partial class ClimbsPage : Page
    {
        ClimbData _data = new ClimbData();
        BrushManager _brushManager;

        ClimbPathCollection _climbPathCollection;

        public ClimbsPage()
        {
            InitializeComponent();

            _brushManager = new BrushManager();
            _climbPathCollection = new ClimbPathCollection(_brushManager, ClimbPathLayer);

            _data.Load<Web.Region>(_data.GetRegionsQuery(), RegionsDone, null);

            MyMap.Center = new Location(47.6242218017578, -122.168998718262);
            MyMap.ZoomLevel = 10;

            _brushManager.InitBrushes();

            MyMap.MouseClick += new EventHandler<MapMouseEventArgs>(MyMap_MouseClick);
        }


        public void RegionsDone(LoadOperation<Web.Region> loadOperation)
        {
            List<Web.Region> regions = loadOperation.Entities.ToList<Web.Region>();

            Regions.ItemsSource = regions;
            Regions.DisplayMemberPath = "RegionName";

            Regions.SelectedIndex = 0;
        }


        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public void ClimbsDone(LoadOperation<Web.Climb> loadOperation)
        {
            List<Web.Climb> climbs = loadOperation.Entities.ToList<Web.Climb>();

            PushpinLayer.Children.Clear();

            Brush blackBrush = new SolidColorBrush(Colors.Black);
            Brush whiteBrush = new SolidColorBrush(Colors.White);

            int i = 0;
            foreach (Web.Climb climb in climbs)
            {
                double gradient = (climb.ElevationGain) / climb.Length;
                Brush brush = _brushManager.GetBrush(gradient);

                ClimbLabel label = new ClimbLabel();
                label.Fill = brush;
                if (gradient < 0.10)
                {
                    label.Stroke = blackBrush;
                }
                else
                {
                    label.Stroke = whiteBrush;
                }
                label.Content = climb.Name;
                label.MouseEnter += new MouseEventHandler(pushpin_MouseEnter);
                label.MouseLeave += new MouseEventHandler(pushpin_MouseLeave);
                label.MouseLeftButtonDown += new MouseButtonEventHandler(pushpin_MouseLeftButtonDown);
                label.Tag = climb;
                label.Name = climb.Name + i.ToString();
                i++;
                label.Scale = 0.75;

#if fred
                Pushpin pushpin = new Pushpin();
                pushpin.Background = brush;
                pushpin.FontSize = 6;
                pushpin.Foreground = blackBrush;
                pushpin.Content = climb.Name;
                pushpin.Location = new Location(climb.LatitudeStart, climb.LongitudeStart);
                pushpin.MouseEnter += new MouseEventHandler(pushpin_MouseEnter);
                pushpin.MouseLeave += new MouseEventHandler(pushpin_MouseLeave);
                pushpin.MouseLeftButtonDown += new MouseButtonEventHandler(pushpin_MouseLeftButtonDown);
                pushpin.Tag = climb;
#endif

                try
                {
                    //PushpinLayer.AddChild(pushpin, pushpin.Location, PositionOrigin.BottomCenter);

                    Location labelLocation = new Location(climb.LatitudeStart, climb.LongitudeStart);
                    PushpinLayer.AddChild(label, labelLocation, PositionOrigin.BottomCenter);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

        }


        void pushpin_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_selectedClimb == null)
            {
                ClimbPathLayer.Children.Clear();
                _climbPathCollection.StopTimer();
            }
        }

        void pushpin_MouseEnter(object sender, MouseEventArgs e)
        {
            FrameworkElement element = (FrameworkElement) sender;
            Climb climb = (Climb) element.Tag;

            if (climb != _selectedClimb)
            {
                _selectedClimb = null;
                ClimbPathLayer.Children.Clear();

                C_ClimbName.Text = climb.Name;
                C_TextBoxLength.Text = String.Format("{0:F1} miles", climb.Length / 5280);
                C_ClimbLocation.Content = climb.Location;
                C_TextBoxElevationGain.Text = climb.ElevationGain + " feet";
                int gradient = (int)((climb.ElevationGain * 100) / climb.Length);
                C_TextBoxGradient.Text = gradient.ToString() + " %";
                C_TextBoxMaxGradient.Text = climb.MaxGradient.ToString() + " %";

                _data.Load<Web.ClimbPathElevation>(_data.GetClimbPathElevationQuery(climb.ID), ClimbPathElevationDone, climb);
            }
        }

        LocationCollection _points;

        public void ClimbPathElevationDone(LoadOperation<ClimbPathElevation> loadOperation)
        {
            Climb climb = (Climb)loadOperation.UserState;

            _climbPathCollection.ClimbPathElevations = loadOperation.Entities.ToList<ClimbPathElevation>();
            _climbPathCollection.CalculateGradients();

            _points = new LocationCollection();

            Brush lastBrush = new SolidColorBrush(Colors.Green);

            ClimbPathLayer.Children.Clear();

            _climbPathCollection.StartAnimation(climb.Length);
        }


        private void Regions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Region region = (Region)e.AddedItems[0];
            _data.Load<Web.Climb>(_data.GetClimbsQuery(region.RegionId), ClimbsDone, null);

            MyMap.Center = new Location(region.Latitude, region.Longitude);
            MyMap.ZoomLevel = region.Zoom;
        }

        bool IsRectangleIn(LocationRect mapRect, LocationRect climbRect)
        {
            if (climbRect.West < mapRect.West || climbRect.West > mapRect.East) return false;
            if (climbRect.East < mapRect.West || climbRect.East > mapRect.East) return false;

            if (climbRect.North > mapRect.North || climbRect.North < mapRect.South) return false;
            if (climbRect.South > mapRect.North || climbRect.South < mapRect.South) return false;

            return true;
        }

        Location _oldCenter;
        double _oldZoomLevel;

        void pushpin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _oldCenter = MyMap.Center;
            _oldZoomLevel = MyMap.ZoomLevel;

            FrameworkElement element = (FrameworkElement)sender;
            _selectedClimb = (Climb)element.Tag;

            MyMap.Center = _climbPathCollection.Center;

            SetCenterAndZoom(_selectedClimb);
            e.Handled = true;
        }

        Climb _selectedClimb;

        void MyMap_MouseClick(object sender, MapMouseEventArgs e)
        {
            if (_selectedClimb != null)
            {
                _selectedClimb = null;
                pushpin_MouseLeave(sender, null);

                MyMap.Center = _oldCenter;
                MyMap.ZoomLevel = _oldZoomLevel;
            }
        }

        void SetCenterAndZoom(Climb climb)
        {
            LocationRect climbRect = _climbPathCollection.BoundingRectangle;

            for (int zoomLevel = 16; zoomLevel > 8; zoomLevel--)
            {
                MyMap.ZoomLevel = zoomLevel;

                if (IsRectangleIn(MyMap.TargetBoundingRectangle, climbRect))
                {
                    break;
                }
            }
        }


    }
}
