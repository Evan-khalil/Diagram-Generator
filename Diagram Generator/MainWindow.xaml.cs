using System.Linq;
using System.Windows;

namespace Diagram_Generator
{
    public partial class MainWindow : Window
    {
        private readonly Diagram Diagram = new Diagram();

        public MainWindow()
        {
            InitializeComponent();
            Diagram.canvas = DiagramCanvas;
        }

        //Parse values to double and draw diagram and disable settings.
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (XNODTxt.Text == "" || YNODTxt.Text == "" || XITxtx.Text == "" || YITxt.Text == "")
            {
                System.Windows.MessageBox.Show("Divistions and intervals can't be empty.");
            }

            else if (!int.TryParse(XNODTxt.Text, out int i) || !int.TryParse(YNODTxt.Text, out int i1) || !int.TryParse(XITxtx.Text, out int i2) || !int.TryParse(YITxt.Text, out int i3))
            {
                System.Windows.MessageBox.Show("Divistions and intervals can't be letters or special characters.");
            }

            else
            {
                Diagram.XDivisions = double.Parse(XNODTxt.Text);
                Diagram.YDivisions = double.Parse(YNODTxt.Text);
                Diagram.XInterval = double.Parse(XITxtx.Text);
                Diagram.YInterval = double.Parse(YITxt.Text);
                Diagram.SetTitle(DTTxt.Text, DiagramGroupBox);
                Diagram.DrawDiagram();
                SettingsGroupBox.IsEnabled = false;
                PointsGroupBox.IsEnabled = true;
            }

        }

        //Add and draw point and draw the diagram and update points listbox.
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (XCTxt.Text == "" || YCTxt.Text == "")
            {
                System.Windows.MessageBox.Show("X and Y can't be empty.");
            }

            else if (!int.TryParse(XCTxt.Text, out int i) || !int.TryParse(YCTxt.Text, out int i1))
            {
                System.Windows.MessageBox.Show("X and Y can't be letters or special characters.");
            }
            else
            {
                Diagram.Add(double.Parse(XCTxt.Text), double.Parse(YCTxt.Text));
                Diagram.Clear();
                Diagram.DrawPoint();
                Diagram.DrawDiagram();
                ListBoxSource();
            }
        }

        //Clear all texboxes.
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            XITxtx.Clear();
            YITxt.Clear();
            XNODTxt.Clear();
            YNODTxt.Clear();
            DTTxt.Clear();
        }

        //Capture mouse movement on canvas and print corresponding point on screen.
        private void CanvasMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Diagram.GetPointOnMousePosition();
        }

        //On XDirCheckBox is checked.
        private void XDirChecked(object sender, RoutedEventArgs e)
        {
            YCheckBox.IsChecked = false;
            ListBoxSource();
        }

        //On YDirCheckBox is checked.
        private void YCheckBoxCheched(object sender, RoutedEventArgs e)
        {
            XCheckBox.IsChecked = false;
            ListBoxSource();
        }

        //Update points listbox item source.
        public void ListBoxSource()
        {

            System.Collections.Generic.List<Points> points = Diagram.Points.OrderBy(x => x.X).ToList();
            if (XCheckBox.IsChecked == true)
            {
                points = Diagram.Points.OrderBy(x => x.X).ToList();//Sort points in Xdir.
            }
            else if (YCheckBox.IsChecked == true)
            {
                points = Diagram.Points.OrderBy(x => x.Y).ToList();//Sort points in Ydir.
            }
            PointsListBox.ItemsSource = points;//ListBox itemsource.
        }

        //Clear the diagram and the points list.
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            DiagramCanvas.Children.Clear();
            DiagramGroupBox.Header = "Diagram";
            Diagram.Points.Clear();
            ListBoxSource();
            PointsGroupBox.IsEnabled = false;
            SettingsGroupBox.IsEnabled = true;
        }
    }
}
