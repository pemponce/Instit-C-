using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ComGrapTask3
{
    public partial class _3 : Window
    {
        private const int MaxIterations = 1000000;
        private readonly Random random = new Random();
        private List<Point> points = new List<Point>();

        public _3()
        {
            InitializeComponent();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            points.Clear();
            int iterations = int.Parse(IterationsTextBox.Text);
            CalculateFern(iterations);
            DrawFern(100);
            double fractalDimension = CalculateFractalDimension();
            MessageBox.Show($"Fractal dimension: {fractalDimension:F3}");
        }

        private void CalculateFern(int iterations)
        {
            double x = 0, y = 0;

            for (int i = 0; i < iterations; i++)
            {
                double randomValue = random.NextDouble();

                if (randomValue < 0.01)
                {
                    x = 0;
                    y = 0.16 * y;
                }
                else if (randomValue < 0.86)
                {
                    double newX = 0.85 * x + 0.04 * y;
                    double newY = -0.04 * x + 0.85 * y + 1.6;
                    x = newX;
                    y = newY;
                }
                else if (randomValue < 0.93)
                {
                    double newX = 0.2 * x - 0.26 * y;
                    double newY = 0.23 * x + 0.22 * y + 1.6;
                    x = newX;
                    y = newY;
                }
                else
                {
                    double newX = -0.15 * x + 0.28 * y;
                    double newY = 0.26 * x + 0.24 * y + 0.44;
                    x = newX;
                    y = newY;
                }

                points.Add(new Point(x, y));
            }
        }

        private void DrawFern(double scaleFactor)
        {
            Canvas.Children.Clear();

            foreach (Point point in points)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 1,
                    Height = 1,
                    Fill = Brushes.White,
                    Margin = new Thickness(point.X * scaleFactor, -point.Y * scaleFactor, 0, 0)
                };

                Canvas.Children.Add(ellipse);
            }
        }

        private double CalculateFractalDimension()
        {
            double totalLength = 0;
            double maxLength = 0;

            for (int i = 1; i < points.Count; i++)
            {
                double length = CalculateDistance(points[i - 1], points[i]);
                totalLength += length;
                maxLength = Math.Max(maxLength, length);
            }

            return Math.Log(totalLength / maxLength) / Math.Log(1 / 2.0);
        }

        private double CalculateDistance(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void IterationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
