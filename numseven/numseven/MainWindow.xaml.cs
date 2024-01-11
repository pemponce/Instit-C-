using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Drawing.Brushes;
using Rectangle = System.Windows.Shapes.Rectangle;
using Window = System.Windows.Window;

namespace numseven
{
    public partial class MainWindow : Window
    {
        private const int canvasWidth = 800;
        private const int canvasHeight = 600;
        private const int circleWidth = 100;
        private const int circleHeight = 100;
        private const int rectangleWidth = 150;
        private const int rectangleHeight = 100;
        private const int triangleBase = 100;
        private const int triangleHeight = 100;
        private int z = 0;
        private int x = 0;
        private int c = 0;

        private List<string> detectedShapes;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            detectedShapes = new List<string>();
        }

        private void DetectShapesButton_Click(object sender, RoutedEventArgs e)
        {
            detectedShapes.Clear();
            foreach (var shape in canvas.Children)
            {
                if (shape is Ellipse)
                {
                    detectedShapes.Add("Круг");
                }
                else if (shape is Rectangle)
                {
                    detectedShapes.Add("Прямоугольник");
                }
                else if (shape is Polygon)
                {
                    detectedShapes.Add("Треугольник");
                }
            }
            if (detectedShapes.Count > 0)
            {
                string result = "Обнаружены следующие фигуры: " + string.Join(", ", detectedShapes);
                MessageBox.Show(result);
            }
            else
            {
                MessageBox.Show("На форме не обнаружено ни одной фигуры.");
            }
        }

        private void AddShapeToCanvas(Shape shape)
        {
            double x = random.NextDouble() * (canvasWidth - shape.Width);
            double y = random.NextDouble() * (canvasHeight - shape.Height);
            Canvas.SetLeft(shape, x);
            Canvas.SetTop(shape, y);
            canvas.Children.Add(shape);
        }
        private void RemShapeToCanvas(Shape shape)
        {
            canvas.Children.Remove(shape);
        }

        private void CircleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Ellipse circle = new Ellipse()
            {
                Width = circleWidth,
                Height = circleHeight,
                Stroke = System.Windows.Media.Brushes.Red,
                StrokeThickness = 2
            };
            if (z % 2 == 0)
            {
                AddShapeToCanvas(circle); 
            }
            else
            {
                RemShapeToCanvas(circle);
            }
            
            z++;
        }

        private void RectangleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = new Rectangle()
            {
                Width = rectangleWidth,
                Height = rectangleHeight,
                Stroke = System.Windows.Media.Brushes.Blue,
                StrokeThickness = 2
            };
            if (x % 2 == 0)
            {
                AddShapeToCanvas(rectangle); 
            }
            else
            {
                RemShapeToCanvas(rectangle);
            }
            
            x++;
        }

        private void TriangleCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Polygon triangle = new Polygon()
            {
                Points = new PointCollection() { new Point(0, 0), new Point(triangleBase, 0), new Point(triangleBase / 2, triangleHeight) },
                Stroke = System.Windows.Media.Brushes.Green,
                StrokeThickness = 2
            };
            if (c % 2 == 0)
            {
                AddShapeToCanvas(triangle); 
            }
            else
            {
                RemShapeToCanvas(triangle);
            }
            
            c++;
        }

        private void ClearCanvasButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
    }
}