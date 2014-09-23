namespace CWLOTPH.DataObject
{
    public class ModeVector
    {
        double _speedOfMovementOfTheTopCover;
        double _temperatureOfTheUpperLid;

        public double Vu
        {
            get { return _speedOfMovementOfTheTopCover; }
            set { _speedOfMovementOfTheTopCover = value; }
        }
        public double Tu
        {
            get { return _temperatureOfTheUpperLid; }
            set { _temperatureOfTheUpperLid = value; }
        }
    }
}
