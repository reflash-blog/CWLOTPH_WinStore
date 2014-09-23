using System.Collections.Generic;

namespace CWLOTPH.DataObject
{
    public class OutputDataObject
    {
        public double Qk { get; set; }  // Производительность канала
        public double Tend { get; set; } // Температура на выходе
        public double Vend { get; set; } // Вязкость на выходе
        public List<double> lengthCoordinates { get; set; }
        public List<double> temperature { get; set; }
        public List<double> viscosity { get; set; }
        public int tickCount { get; set; }
    }
}
