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

using Microsoft.Maps.MapControl;

namespace BicycleClimbsSilverlight
{
    public partial class TrimPage : Page
    {
        public TrimPage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void FirstPointSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
//          firstPoint = (int)e.NewValue;
//          FirstPointText.Text = ((int)e.NewValue).ToString();
//          AddPolyline();
        }

        private void LastPointSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
//            lastPoint = (int)e.NewValue;
//            LastPointText.Text = ((int)e.NewValue).ToString();
//            AddPolyline();
        }

        private void RouteNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
//            name = (string) e.AddedItems[0];
//            proxy.FetchAsync(name);
        }

        private void SaveSubset_Click(object sender, RoutedEventArgs e)
        {
//            proxy.SubsetAndSaveAsync(name, SubsetName.Text, firstPoint, lastPoint);
//            proxy.SubsetAndSaveCompleted += new EventHandler<SubsetAndSaveCompletedEventArgs>(proxy_SubsetAndSaveCompleted);
        }

        private void MyMap_MouseClick(object sender, MapMouseEventArgs e)
        {
//            Garmin.Trackpoint trackpoint = FindClosestPoint(e);
//            InfoText.Text = "Gradient: " +
//                            (trackpoint.GradientAverage * 100).ToString("0") +
//                            " %";
        }

        private void Trim_Click(object sender, RoutedEventArgs e)
        {
//            FirstPointSlider.Minimum = firstPoint;
//            LastPointSlider.Minimum = firstPoint;
//            FirstPointSlider.Maximum = lastPoint;
//            LastPointSlider.Maximum = lastPoint;
        }
    }
}


