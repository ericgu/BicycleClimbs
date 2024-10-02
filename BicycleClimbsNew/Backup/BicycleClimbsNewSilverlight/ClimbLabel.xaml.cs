using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BicycleClimbsSilverlight
{
	public partial class ClimbLabel : UserControl
	{
		public ClimbLabel()
		{
			// Required to initialize variables
			InitializeComponent();

            //C_TextBlock.SizeChanged += new SizeChangedEventHandler(C_TextBlock_SizeChanged);
		}

        void C_TextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double height = C_TextBlock.ActualHeight;

            if (height <= 48)
            {
                C_Path.Height = height + 15;
            }
        }

        public new string Content
        {
            get { return C_TextBlock.Text; }
            set 
            { 
                C_TextBlock.Text = value;

                double height = C_TextBlock.ActualHeight;
                double pathHeight = 31;

                if (height == 32)
                {
                    pathHeight = 50;
                }
                else if (height == 48)
                {
                    pathHeight = 75;
                }
                else if (height >= 64)
                {
                    pathHeight = 100;
                }

                C_Path.Height = pathHeight;

                C_Path.SetValue(Canvas.TopProperty, 95 - pathHeight);
                C_TextBlock.SetValue(Canvas.TopProperty, 95 - pathHeight);
            }
        }

        public Brush Fill
        {
            get { return C_Path.Fill; }
            set { C_Path.Fill = value; }
        }

        public Brush Stroke
        {
            get { return C_TextBlock.Foreground; }
            set { C_TextBlock.Foreground = value; }
        }

        public double Scale
        {
            get { return ClimbLabelScale.ScaleX; }
            set
            {
                ClimbLabelScale.ScaleX = value;
                ClimbLabelScale.ScaleY = value;
            }

        }
	}
}