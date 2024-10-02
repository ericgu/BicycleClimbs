using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace BicycleClimbsLibrary
{
    public class GraphAxis
    {
        const int Y_AXIS_LABEL_PADDING = -10;

        public float Minimum = Single.MaxValue;
        public float Maximum = Single.MinValue;
        public float Division;
        public bool WidenAtMax = false;

        public string Label;
        
        public void UpdateMinMax(float value)
        {
            if (value < Minimum)
            {
                Minimum = value;
            }

            if (value > Maximum)
            {
                Maximum = value;
            }
        }

        public void CalculateDivision()
        {
            Division = 0.1f;

            float range = Maximum - Minimum;

            while (true)
            {
                if (range / Division < 10)
                {
                    break;
                }

                Division *= 5;

                if (range / Division < 10)
                {
                    break;
                }

                Division *= 2;
            }

                // Widen out the max and min to give us nice endpoints...

            Minimum = (int)(Minimum / Division) * Division;

            if (WidenAtMax)
            {
                int endCount = (int)(Maximum / Division);

                if (endCount == Maximum / Division)
                {
                    Maximum = endCount * Division;
                }
                else
                {
                    Maximum = (endCount + 1) * Division;
                }
            }
        }

        const int TICLENGTH = 5;

        public void DrawXAxis(Graphics g, Rectangle plotRectangle)
        {
            float scale = plotRectangle.Width / (Maximum - Minimum);

            Font font = new Font("Arial", 10);
            for (float value = Minimum; value <= Maximum; value += Division)
            {
                int x = plotRectangle.Left + (int)((value - Minimum) * scale);
                int y = plotRectangle.Bottom;

                g.DrawLine(Pens.Black, x, y, x, y + TICLENGTH);

                string label = value.ToString();

                SizeF labelSize = g.MeasureString(label, font);
                g.DrawString(label, font, Brushes.Black, x - labelSize.Width / 2, y + TICLENGTH);
            }

            SizeF nameSize = g.MeasureString(Label, font);
            g.DrawString(Label, font, Brushes.Black, plotRectangle.Left + plotRectangle.Width / 2 - nameSize.Width / 2,
                                                     plotRectangle.Bottom + TICLENGTH + nameSize.Height);
        }

        public void DrawYAxis(Graphics g, Rectangle plotRectangle)
        {
            float scale = plotRectangle.Height / (Maximum - Minimum);

			float maxLabelWidth = Single.MinValue;
            Font font = new Font("Arial", 10);
            for (float value = Minimum; value <= Maximum; value += Division)
            {
                int x = plotRectangle.Left;
                int y = plotRectangle.Bottom - (int)((value - Minimum) * scale);

                g.DrawLine(Pens.Black, x, y, x - TICLENGTH, y);

                string label = value.ToString();

                SizeF labelSize = g.MeasureString(label, font);
                g.DrawString(label, font, Brushes.Black, x - labelSize.Width - TICLENGTH, y - (labelSize.Height / 2));

				if (labelSize.Width > maxLabelWidth)
				{
					maxLabelWidth = labelSize.Width;
				}
            }

            SizeF nameSize = g.MeasureString(Label, font);
            PointF spot = new PointF(plotRectangle.Left - TICLENGTH - maxLabelWidth - Y_AXIS_LABEL_PADDING - nameSize.Height,
                                     plotRectangle.Top + plotRectangle.Height / 2);
            Matrix matrix = g.Transform;
            matrix.RotateAt(270, spot);
            g.Transform = matrix;
            g.DrawString(Label, font, Brushes.Black, spot.X - nameSize.Width / 2, spot.Y - nameSize.Height);
            matrix.Reset();
            g.Transform = matrix;
        }

        internal void DrawYBand(Graphics g, Brush band1, Brush band2, Rectangle plotRectangle)
        {
            float scale = plotRectangle.Height / (Maximum - Minimum);

            Font font = new Font("Arial", 10);
            int band = 0;
            for (float value = Minimum + Division; value <= Maximum; value += Division)
            {
                int y1 = plotRectangle.Bottom - (int)((value - Minimum) * scale);
                int y2 = plotRectangle.Bottom - (int)((value - Division - Minimum) * scale);

                Brush brush = (band % 2 == 0) ? band1 : band2;
                band++;
                g.FillRectangle(brush, plotRectangle.Left, y1, plotRectangle.Width, y2 - y1);
            }
        }
    }
}
