using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Drawing.Imaging;

namespace BicycleClimbsLibrary
{
    public class ProfileGraph
    {
        readonly string copyright = "Copyright (c) BicycleClimbs.com, all right reserved.";

        const int LEFT_MARGIN = 60;
        const int RIGHT_MARGIN = 20;
        const int TOP_MARGIN = 30;
        const int BOTTOM_MARGIN = 50;

		const int HIGHLIGHT_RADIUS = 3;

        public Color BackgroundColor = Color.White;
        public List<DataPoint> Points;
        public string Title;

        Rectangle dataRect;
        Graphics g;

        GraphAxis xAxis = new GraphAxis();
        GraphAxis yAxis = new GraphAxis();

        int width;
        int height;
        float xScale;
        float yScale;

        public Bitmap CreateGraph(int width, int height)
        {
            this.width = width;
            this.height = height;
            xAxis.Label = "Distance (mi)";
            yAxis.Label = "Altitude (ft)";
            yAxis.WidenAtMax = true;
            CalculateLimits();

            dataRect = new Rectangle(LEFT_MARGIN, TOP_MARGIN, width - LEFT_MARGIN - RIGHT_MARGIN, height - TOP_MARGIN - BOTTOM_MARGIN);
            xAxis.CalculateDivision();
            yAxis.CalculateDivision();
            
            xScale = dataRect.Width / (xAxis.Maximum - xAxis.Minimum);
            yScale = dataRect.Height / (yAxis.Maximum - yAxis.Minimum);
            
            Bitmap bitmap = new Bitmap(width, height);

            g = Graphics.FromImage(bitmap);
            //g.SmoothingMode = SmoothingMode.AntiAlias;


 
            g.FillRectangle(new SolidBrush(BackgroundColor), 0, 0, width, height);

            //yAxis.DrawYBand(g, Brushes.Blue, Brushes.Red, dataRect);
            yAxis.DrawYBand(g, Brushes.LightBlue, Brushes.LightGray, dataRect);

            PlotData();
            HighlightMaxGradient();
            g.DrawRectangle(Pens.Black, dataRect);

            xAxis.DrawXAxis(g, dataRect);
            yAxis.DrawYAxis(g, dataRect);

            DrawTitle();
            AddCopyright();

			return bitmap;
        }

        void AddCopyright()
        {
            Font font = new Font("Arial", 8);

            SizeF size = g.MeasureString(copyright, font);

            g.DrawString(copyright, font, Brushes.Blue, width - size.Width - 2, height - size.Height - 2);
        }

        void DrawTitle()
        {
            Font font = new Font("Arial", 14);

            SizeF size = g.MeasureString(Title, font);

            g.DrawString(Title, font, Brushes.Black, dataRect.Left + dataRect.Width / 2 - size.Width / 2, dataRect.Top - size.Height);
        }

        void DrawDataLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            SmoothingMode oldMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawLine(pen, dataRect.Left + x1, dataRect.Top + y1,
                            dataRect.Left + x2, dataRect.Top + y2);
            g.SmoothingMode = oldMode;
        }

        void ClearAboveDataLine(float x1, float y1, float x2, float y2)
        {
            PointF[] points = new PointF[5];

            points[0].X = x1 + dataRect.Left;
            points[0].Y = y1 + dataRect.Top;

            points[1].X = x2 + dataRect.Left;
            points[1].Y = y2 + dataRect.Top;

            points[2].X = x2 + dataRect.Left;
            points[2].Y = dataRect.Top;

            points[3].X = x1 + dataRect.Left;
            points[3].Y = dataRect.Top;

            points[4].X = x1 + dataRect.Left;
            points[4].Y = y1 + dataRect.Top;

            SmoothingMode oldMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.Default;
            g.FillPolygon(Brushes.White, points);
            g.SmoothingMode = oldMode;
        }

        void PlotData()
        {
            DataPoint lastPoint = Points[0];

            Pen pen = new Pen(Color.Green, 2.0f);
            for (int i = 1; i < Points.Count; i++)
            {
                DataPoint point = Points[i];

                float x1 = (float)((lastPoint.x - xAxis.Minimum) * xScale);
                float y1 = (float) (dataRect.Height - (lastPoint.y - yAxis.Minimum) * yScale);

                float x2 = (float)((point.x - xAxis.Minimum) * xScale);
                float y2 = (float) (dataRect.Height - (point.y - yAxis.Minimum) * yScale);

                ClearAboveDataLine(x1, y1, x2, y2);
                DrawDataLine(pen, x1, y1, x2, y2);

                lastPoint = point;
            }

                // clear to the left and right...

            float xFirst = (float)((Points[0].x - xAxis.Minimum) * xScale);
            g.FillRectangle(Brushes.White, dataRect.Left, dataRect.Top, xFirst, dataRect.Bottom - dataRect.Top);
            float xLast = (float)((Points[Points.Count - 1].x - xAxis.Minimum) * xScale) + dataRect.Left;
            g.FillRectangle(Brushes.White, xLast, dataRect.Top, dataRect.Right - xLast, dataRect.Bottom - dataRect.Top);
        }

        void HighlightMaxGradient()
        {
            int maxGradientIndex = -1;
            float maxGradient = Single.MinValue;
            for (int i = 0; i < Points.Count - 1; i++)
            {
                float gradient = (float) ((Points[i + 1].y - Points[i].y) / (Points[i + 1].x - Points[i].x));

                if (gradient > maxGradient)
                {
                    maxGradient = gradient;
                    maxGradientIndex = i;
                }
            }

            if (maxGradientIndex != -1)
            {
                float xData = (float)((Points[maxGradientIndex].x + Points[maxGradientIndex + 1].x) / 2);
                float yData = (float)((Points[maxGradientIndex].y + Points[maxGradientIndex + 1].y) / 2);
                float x = (float)((xData - xAxis.Minimum) * xScale);
                float y = (float)(dataRect.Height - (yData - yAxis.Minimum) * yScale);

                g.FillEllipse(Brushes.Red, x + dataRect.Left - HIGHLIGHT_RADIUS, y + dataRect.Top - HIGHLIGHT_RADIUS, 
								HIGHLIGHT_RADIUS * 2, HIGHLIGHT_RADIUS * 2);
            }
        }

        void CalculateLimits()
        {
            foreach (DataPoint point in Points)
            {
                xAxis.UpdateMinMax((float) point.x);
                yAxis.UpdateMinMax((float) point.y);
            }
        }
    }
}
