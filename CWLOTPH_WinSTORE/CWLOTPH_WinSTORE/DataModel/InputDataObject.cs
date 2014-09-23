namespace CWLOTPH.DataObject
{
    public class InputDataObject
    {
        EmpiricalVector _empirVector;
        GeometryVector _geomVector;
        ModeVector _modeVector;
        MaterialProperties _materProperties;
        double _discretizationStep = 0.5;


        public EmpiricalVector EmpiricalVector
        {
            get { return _empirVector; }
            set { _empirVector = value; }
        }
        public GeometryVector GeometryVector
        {
            get { return _geomVector; }
            set { _geomVector = value; }
        }
        public ModeVector ModeVector
        {
            get { return _modeVector; }
            set { _modeVector = value; }
        }

        public MaterialProperties MaterialProperties
        {
            get { return _materProperties; }
            set { _materProperties = value; }
        }

        public double DiscretizationStep
        {
            get { return _discretizationStep; }
            set { _discretizationStep = value; }
        }
    }
}
