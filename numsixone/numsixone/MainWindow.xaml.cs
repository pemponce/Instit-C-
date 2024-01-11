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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace numsixone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumLines = 100;
        private const int CanvasSize = 500;

        public MainWindow()
        {
            InitializeComponent();
            DrawRandomLines();
        }

        private void DrawRandomLines()
        {
            Random random = new Random();
            for (int i = 0; i < NumLines; i++)
            {
                int x1 = random.Next(CanvasSize);
                int y1 = random.Next(CanvasSize);
                int x2 = random.Next(CanvasSize);
                int y2 = random.Next(CanvasSize);

                DrawLineBresenham(x1, y1, x2, y2);
            }
            for (int i = 0; i < NumLines; i++)
            {
                int x1 = random.Next(CanvasSize);
                int y1 = random.Next(CanvasSize);
                int x2 = random.Next(CanvasSize);
                int y2 = random.Next(CanvasSize);

                DrawLineDDA(x1, y1, x2, y2);
            }
        }

        private void DrawLineBresenham(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawPixel(x1, y1, Brushes.Blue); // Здесь можно изменить цвет для Брезенхэма
                if (x1 == x2 && y1 == y2) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        private void DrawLineDDA(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            double xIncrement = (double)dx / steps;
            double yIncrement = (double)dy / steps;

            double x = x1;
            double y = y1;

            for (int i = 0; i <= steps; i++)
            {
                DrawPixel((int)x, (int)y, Brushes.Red); // Здесь можно изменить цвет для ЦДА
                x += xIncrement;
                y += yIncrement;
            }
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

            сanvas.Children.Add(ellipse);
        }
    }
}