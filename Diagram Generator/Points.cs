namespace Diagram_Generator
{
    public class Points
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Points(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
