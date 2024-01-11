using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace kg33
{
    public partial class Task33 : Window
    {
        private const int InitialLength = 200;
        private const int MaxIterations = 9;
        private List<Point> points = new List<Point>();

        public Task33()
        {
            InitializeComponent();
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            points.Clear();
            int iterations = int.Parse(IterationsTextBox.Text);
            CalculateKochCurve(iterations);
            DrawCurve(3.0);
            double fractalDimension = CalculateFractalDimension();
            MessageBox.Show($"Fractal dimension: {fractalDimension:F3}");
        }

        private void CalculateKochCurve(int iterations)
        {
            points.Clear();
            Point startPoint = new Point(0, 0);
            Point endPoint = new Point(InitialLength, 0);
            GenerateCurve(startPoint, endPoint, iterations);
        }

        private void GenerateCurve(Point startPoint, Point endPoint, int iterations)
        {
            if (iterations == 0)
            {
                points.Add(startPoint);
                points.Add(endPoint);
            }
            else
            {
                Point p1 = startPoint;
                Point p5 = endPoint;
                Vector v = p5 - p1;
                Point p2 = p1 + v / 3;
                Point p4 = p1 + v * 2 / 3;
                Point a = RotateVector(v / 3, -60);
                Point p3 = new Point();
                p3.X = p2.X + a.X;
                p3.Y = p2.Y + a.Y;

                GenerateCurve(p1, p2, iterations - 1);
                GenerateCurve(p2, p3, iterations - 1);
                GenerateCurve(p3, p4, iterations - 1);
                GenerateCurve(p4, p5, iterations - 1);
            }
        }

        private Point RotateVector(Vector vector, double angle)
        {
            double radians = angle * Math.PI / 180;
            double cosTheta = Math.Cos(radians);
            double sinTheta = Math.Sin(radians);
            double x = vector.X * cosTheta - vector.Y * sinTheta;
            double y = vector.X * sinTheta + vector.Y * cosTheta;
            return new Point(x, y);
        }

        private void DrawCurve(double scaleFactor)
        {
            Canvas.Children.Clear();

            for (int i = 0; i < points.Count - 1; i++)
            {
                Line line = new Line
                {
                    X1 = points[i].X * scaleFactor,
                    Y1 = points[i].Y * scaleFactor,
                    X2 = points[i + 1].X * scaleFactor,
                    Y2 = points[i + 1].Y * scaleFactor,
                    Stroke = Brushes.White
                };

                Canvas.Children.Add(line);
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

            return Math.Log(totalLength / maxLength) / Math.Log(1 / 3.0);
        }

        private double CalculateDistance(Point p1, Point p2)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
