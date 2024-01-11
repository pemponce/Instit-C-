using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace numsixtwo
{
    public partial class MainWindow : Window
    {
        private const int NumCircles = 100;
        private const int CanvasSize = 800;

        public MainWindow()
        {
            InitializeComponent();
            DrawRandomCircles();
        }

        private void DrawRandomCircles()
        {
            Random random = new Random();
            for (int i = 0; i < NumCircles; i++)
            {
                int centerX = random.Next(CanvasSize);
                int centerY = random.Next(CanvasSize);
                int radius = random.Next(10, 50); // Радиус в пределах от 10 до 50
                DrawCircleBresenham(centerX, centerY, radius);
                
                centerX = random.Next(CanvasSize);
                centerY = random.Next(CanvasSize);
                radius = random.Next(10, 50); // Радиус в пределах от 10 до 50
                DrawCirclePolygon(centerX, centerY, radius);
                
                centerX = random.Next(CanvasSize);
                centerY = random.Next(CanvasSize);
                radius = random.Next(10, 50); // Радиус в пределах от 10 до 50
                DrawCircleMidPoint(centerX, centerY, radius);
            }
        }

        private void DrawCircleBresenham(int centerX, int centerY, int radius)
        {
            int x = 0;
            int y = radius;
            int delta = 1 - 2 * radius;
            int error = 0;

            while (y >= 0)
            {
                DrawPixel(centerX + x, centerY - y, Brushes.Blue); // Здесь можно изменить цвет для Брезенхэма
                DrawPixel(centerX - x, centerY - y, Brushes.Blue);
                DrawPixel(centerX - x, centerY + y, Brushes.Blue);
                DrawPixel(centerX + x, centerY + y, Brushes.Blue);

                error = 2 * (delta + y) - 1;
                if ((delta < 0) && (error <= 0))
                {
                    x++;
                    delta += 2 * x + 1;
                    continue;
                }
                error = 2 * (delta - x) - 1;
                if ((delta > 0) && (error > 0))
                {
                    y--;
                    delta += 1 - 2 * y;
                    continue;
                }
                x++;
                delta += 2 * (x - y);
                y--;
            }
        }

        private void DrawCirclePolygon(int centerX, int centerY, int radius)
        {
            int sides = 30; // Количество сторон многоугольника для аппроксимации

            double angleIncrement = 2 * Math.PI / sides;

            List<Point> points = new List<Point>();

            for (int i = 0; i < sides; i++)
            {
                double x = centerX + radius * Math.Cos(i * angleIncrement);
                double y = centerY + radius * Math.Sin(i * angleIncrement);
                points.Add(new Point(x, y));
            }

            DrawPolygon(points, Brushes.Red); // Здесь можно изменить цвет для аппроксимации многоугольником
        }

        private void DrawCircleMidPoint(int centerX, int centerY, int radius)
        {
            int x = radius;
            int y = 0;
            int radiusError = 1 - x;

            while (x >= y)
            {
                DrawPixel(centerX + x, centerY - y, Brushes.Green); // Здесь можно изменить цвет для средней точки
                DrawPixel(centerX - x, centerY - y, Brushes.Green);
                DrawPixel(centerX - x, centerY + y, Brushes.Green);
                DrawPixel(centerX + x, centerY + y, Brushes.Green);

                DrawPixel(centerX + y, centerY - x, Brushes.Green);
                DrawPixel(centerX - y, centerY - x, Brushes.Green);
                DrawPixel(centerX - y, centerY + x, Brushes.Green);
                DrawPixel(centerX + y, centerY + x, Brushes.Green);

                y++;

                if (radiusError < 0)
                {
                    radiusError += 2 * y + 1;
                }
                else
                {
                    x--;
                    radiusError += 2 * (y - x + 1);
                }
            }
        }

        private void DrawPolygon(List<Point> points, SolidColorBrush color)
        {
            Polygon polygon = new Polygon
            {
                Stroke = color,
                StrokeThickness = 1,
                Fill = color
            };

            foreach (Point point in points)
            {
                polygon.Points.Add(point);
            }

            canvas.Children.Add(polygon);
        }

        private void DrawPixel(int x, int y, SolidColorBrush color)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 2,
                Height = 2,
                Fill = color
            };

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);

            canvas.Children.Add(ellipse);
        }
    }
}