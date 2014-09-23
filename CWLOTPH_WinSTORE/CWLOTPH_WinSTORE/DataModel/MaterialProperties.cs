namespace CWLOTPH.DataObject
{
    public class MaterialProperties
    {
        double _density;
        double _meltingPoint;
        double _averageSpecificHeatCapacity;

        public double Ro
        {
            get { return _density; }
            set { _density = value; }
        }
        public double T0
        {
            get { return _meltingPoint; }
            set { _meltingPoint = value; }
        }
        public double C
        {
            get { return _averageSpecificHeatCapacity; }
            set { _averageSpecificHeatCapacity = value; }
        }
    }
}
