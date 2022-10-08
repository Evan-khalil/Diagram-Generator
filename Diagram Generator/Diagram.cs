using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Diagram_Generator
{
    public class Diagram
    {
        public string DiagramTitle { get; set; }
        public double DiagramX { get; set; }
        public double DiagramY { get; set; }
        public double XDivisions { get; set; }
        public double YDivisions { get; set; }
        public double XInterval { get; set; }
        public double YInterval { get; set; }
        public double X2;
        public double Y2;
        public List<Points> Points = new List<Points>();
        public Canvas canvas;

        //Add point to list, if point already exists then notify user.
        public void Add(double x, double y)
        {
            if (x > XDivisions * XInterval)
            {
                MessageBox.Show("Too large number, the X should be within the range of your X-divisition multiplies X-interval");
            }
            else if (y > YDivisions * YInterval)
            {
                MessageBox.Show("Too large number, the Y should be within the range of your Y-divisition multiplies Y-interval");
            }
            else
            {
                if (Points.Find(a => a.X == x & a.Y == y) == null)
                {
                    Points.Add(new Points(x, y));
                }
                else
                {
                    MessageBox.Show("Point already exist");
                }
            }
        }

        //Clear canvas.
        public void Clear()
        {
            canvas.Children.Clear();
        }

        //Set title.
        public void SetTitle(string title, GroupBox groupBox)
        {
            DiagramTitle = title;
            groupBox.Header = DiagramTitle;
        }

        //Draw Diagram Y line.
        public Line YLine()
        {
            Line Y = new Line
            {
                Stroke = Brushes.Red,
                X1 = 50,
                X2 = 50,
                Y1 = 50,
                Y2 = canvas.Height - 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 1
            };
            Y2 = Y.Y2;
            canvas.Children.Add(Y);
            return Y;
        }

        //Draw Diagram X line.
        public Line XLine()
        {
            Line X = new Line
            {
                Stroke = Brushes.Red,
                X1 = 50,
                X2 = canvas.Width - 50,
                Y1 = canvas.Height - 50,
                Y2 = canvas.Height - 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 1
            };
            X2 = X.X2;
            canvas.Children.Add(X);
            return X;
        }

        //Draw Diagram X div.
        public void XDiv()
        {
            Line X = XLine();
            double xDivLength = (X.X2 - X.X1) / XDivisions;
            for (int i = 0; i <= XDivisions; i++)
            {
                Line xDiv = new Line
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 3,
                    X1 = 50 + xDivLength * i,
                    X2 = 50 + xDivLength * i,
                    Y1 = X.Y2 - 2,
                    Y2 = X.Y2 + 2
                };
                canvas.Children.Add(xDiv);

                TextBlock xDivText = new TextBlock
                {
                    Text = (XInterval * i).ToString(),
                    Foreground = new SolidColorBrush(Brushes.Black.Color),
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                canvas.Children.Add(xDivText);
                Canvas.SetLeft(xDivText, xDiv.X2 - 5);
                Canvas.SetTop(xDivText, xDiv.Y1 + 20);
            }
        }

        //Draw diagram Y div.
        public void YDiv()
        {
            Line Y = YLine();
            Line X = XLine();
            double yDivLength = (Y.Y2 - Y.Y1) / YDivisions;
            for (int i = 0; i <= YDivisions; i++)
            {
                Line yDiv = new Line
                {
                    StrokeThickness = 3,
                    Stroke = Brushes.Red,
                    X1 = X.X1 - 2,
                    X2 = X.X1 + 2,
                    Y1 = Y.Y2 - yDivLength * i,
                    Y2 = Y.Y2 - yDivLength * i
                };
                canvas.Children.Add(yDiv);

                TextBlock yDivText = new TextBlock
                {
                    Text = (YInterval * i).ToString(),
                    Foreground = new SolidColorBrush(Brushes.Black.Color),
                    TextAlignment = TextAlignment.Right
                };
                Canvas.SetLeft(yDivText, yDiv.X1 - 25);
                Canvas.SetTop(yDivText, yDiv.Y2 - 10);
                canvas.Children.Add(yDivText);
            }
        }

        //Draw diagram.
        public void DrawDiagram()
        {
            XDiv();
            YDiv();
        }

        //Draw point.
        public void DrawPoint()
        {
            Clear();
            DrawDiagram();
            if (Points.Count != 0)
            {
                List<Points> points = Points.OrderBy(x => x.X).ToList();

                for (int i = 0; i < Points.Count; i++)
                {
                    double X = points[i].X / (XDivisions * XInterval);
                    double FirstPointX = X * (X2 - 50) + 50;

                    double Y = points[i].Y / (YDivisions * YInterval);
                    double FirstPointY = Y * (Y2 - 50) + 50;

                    if (Points.Count == 1)
                    {
                        Ellipse Point = new Ellipse
                        {
                            Height = 4,
                            Width = 4,
                            StrokeThickness = 2,
                            Stroke = Brushes.Red
                        };
                        Canvas.SetLeft(Point, FirstPointX - 2);
                        Canvas.SetBottom(Point, FirstPointY - 2);
                        canvas.Children.Add(Point);
                    }

                    else if (i != Points.Count - 1)
                    {
                        double SecondPointX = points[i + 1].X / (XDivisions * XInterval) * (X2 - 50) + 50;
                        double SecondPointY = points[i + 1].Y / (YDivisions * YInterval) * (Y2 - 50) + 50;
                        Line graph = new Line
                        {
                            X1 = FirstPointX,
                            X2 = SecondPointX,
                            Y1 = canvas.Height - FirstPointY,
                            Y2 = canvas.Height - SecondPointY,
                            StrokeThickness = 2,
                            Stroke = Brushes.Blue
                        };
                        canvas.Children.Add(graph);
                    }
                }

            }
        }

        //Capture mouse position and find corresponding point on diagram then print the X and Y on screen.
        public void GetPointOnMousePosition()
        {

            if (Points.Count == 0)
            {
                return;
            }
            else
            {
                Clear();
                DrawDiagram();
                DrawPoint();
                Point mousePosition = Mouse.GetPosition(canvas);
                if (mousePosition.Y > 50 && mousePosition.Y < canvas.Height - 50 &&
                    mousePosition.X > 50 && mousePosition.X < canvas.Width - 50)
                {
                    mousePosition.Y = canvas.Height - mousePosition.Y;
                    float X = (float)((mousePosition.X - 50) / (canvas.Width - 50 - 50) * XDivisions * XInterval);
                    float Y = (float)((mousePosition.Y - 50) / (canvas.Height - 50 - 50) * YDivisions * YInterval);

                    TextBlock PointTextBlock = new TextBlock
                    {
                        Text = "(" + X.ToString("N2") + "." + Y.ToString("N2") + ")",
                        Foreground = Brushes.Red
                    };
                    Canvas.SetLeft(PointTextBlock, mousePosition.X);
                    Canvas.SetBottom(PointTextBlock, mousePosition.Y);
                    canvas.Children.Add(PointTextBlock);
                }
            }
        }
    }
}
