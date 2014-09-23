namespace CWLOTPH.DataObject
{
    public class GeometryVector
    {
        double _length;
        double _width;
        double _deepness;

        public double L
        {
            get { return _length; }
            set { _length = value; }
        }
        public double W
        {
            get { return _width; }
            set { _width = value; }
        }
        public double H
        {
            get { return _deepness; }
            set { _deepness = value; }
        }
    }
}
