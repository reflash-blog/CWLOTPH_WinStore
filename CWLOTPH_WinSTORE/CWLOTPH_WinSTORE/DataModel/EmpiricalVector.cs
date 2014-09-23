namespace CWLOTPH.DataObject
{
    public class EmpiricalVector
    {
        double _consistencyIndexMaterial;
        double _indexOfMaterialFlow;
        double _temperatureReduction;
        double _theHeatTransferCoefficient;
        double _temperatureCoefficientOfViscosity;

        public double Mu
        {
            get { return _consistencyIndexMaterial; }
            set { _consistencyIndexMaterial = value; }
        }
        public double n
        {
            get { return _indexOfMaterialFlow; }
            set { _indexOfMaterialFlow = value; }
        }
        public double Tgamma
        {
            get { return _temperatureReduction; }
            set { _temperatureReduction = value; }
        }
        public double alphaU
        {
            get { return _theHeatTransferCoefficient; }
            set { _theHeatTransferCoefficient = value; }
        }
        public double b
        {
            get { return _temperatureCoefficientOfViscosity; }
            set { _temperatureCoefficientOfViscosity = value; }
        }

    }
}
