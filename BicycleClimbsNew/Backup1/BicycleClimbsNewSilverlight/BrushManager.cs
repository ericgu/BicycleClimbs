using System;
using System.Net;
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
    public class BrushManager
    {
        Color[] colors;
        Brush[] brushes = new Brush[21];

        void BlendColors(int low, int high)
        {
            double steps = high - low;
            double rDelta = (colors[high].R - colors[low].R) / steps;
            double gDelta = (colors[high].G - colors[low].G) / steps;
            double bDelta = (colors[high].B - colors[low].B) / steps;

            for (int i = low + 1; i < high; i++)
            {
                colors[i] = Color.FromArgb(255,
                                (byte)(colors[low].R + (rDelta * (i - low))),
                                (byte)(colors[low].G + (gDelta * (i - low))),
                                (byte)(colors[low].B + (bDelta * (i - low))));
            }
        }

        void InitColors()
        {
            //  0 => -5
            //  5 =>  0
            // 10 =>  5
            // 15 => 10
            // 20 => 15

            colors = new Color[21];
            colors[20] = Color.FromArgb(255, 100, 0, 0);
            colors[15] = Color.FromArgb(255, 255, 0, 0);
            colors[10] = Color.FromArgb(255, 255, 255, 0);
            colors[5] = Color.FromArgb(255, 0, 255, 0);
            colors[0] = Color.FromArgb(255, 0, 0, 255);

            BlendColors(0, 5);
            BlendColors(5, 10);
            BlendColors(10, 15);
            BlendColors(15, 20);
        }


        public void InitBrushes()
        {
            InitColors();

            for (int i = 0; i < brushes.Length; i++)
            {
                brushes[i] = new SolidColorBrush(colors[i]);
            }
        }

        public Brush GetBrush(double gradient)
        {
            int index = (int)(gradient * 100);
            if (index < -5)
            {
                index = -5;
            }
            else if (index > 13)
            {
                index = 13;
            }

            index += 5; // offset into array

            return brushes[index];
        }
    }
}
