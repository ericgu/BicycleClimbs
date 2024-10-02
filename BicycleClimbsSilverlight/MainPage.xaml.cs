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

using System.ServiceModel.DomainServices.Client;

namespace BicycleClimbsSilverlight
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();

            BicycleClimbsSilverlight.Web.ClimbData data = new Web.ClimbData();

#if fred
            data.GetHelloValue("Kitty",
                    (InvokeOperation<string> invokeOperation) =>
                      {
                            MessageBox.Show(invokeOperation.Value);
                      }, 
                      null);
#endif

            data.GetHelloValue("My Baby", HelloDone, null);

            data.Load<Web.Climb>(data.GetClimbsQuery(1), ClimbsDone, null);
        }

        public void ClimbsDone(LoadOperation<Web.Climb> loadOperation)
        {
            List<Web.Climb> climbs = loadOperation.Entities.ToList<Web.Climb>();
        }


        public void HelloDone(InvokeOperation<string> invokeOperation)
        {
            MessageBox.Show(invokeOperation.Value);
        }


    }
}
