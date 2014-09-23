using System.Collections.ObjectModel;

namespace CWLOTPH_WinSTORE.DataModel
{
    public class VisualisationDataObject
    {
        private static ObservableCollection<VectorObject> _vObjects;

        public static ObservableCollection<VectorObject> VectorObject
        {
            get
            {
                if (_vObjects != null) return _vObjects;
                _vObjects = new ObservableCollection<VectorObject>();
                InitDataObject();
                ProcessingDataObject.Clear();
                return _vObjects;
            }
        }

        private static void InitDataObject()
        {
            VectorObject.Clear();
            var geometryVector = new VectorObject("Vector-1", "Геометрический вектор",
                "Вектор, описывающий геометрические параметры объекта",
                "Данный вектор описывает геометрические параметры объекта управления, такие как длина, ширина и высота канала");
            geometryVector.Items.Add(new VectorItem("H", "0.03", "Высота канала"));
            geometryVector.Items.Add(new VectorItem("W", "0.17", "Ширина канала"));
            geometryVector.Items.Add(new VectorItem("L", "6.6", "Длина канала"));
            var empiricalVector = new VectorObject("Vector-2", "Эмпирический вектор",
                "Вектор, описывающий эмпирические параметры объекта",
                "Данный вектор описывает эмпирические параметры объекта, такие как вязкость, индекс консистенции материала и другие");
            empiricalVector.Items.Add(new VectorItem("Mu", "1550", "Индекс консистенции материала"));
            empiricalVector.Items.Add(new VectorItem("n", "0.4", "Индекс течения материала"));
            empiricalVector.Items.Add(new VectorItem("Tgamma", "180", "Температурный спад"));
            empiricalVector.Items.Add(new VectorItem("alphaU", "2000", "Коэффициент передачи тепла"));
            empiricalVector.Items.Add(new VectorItem("b", "0.015", "Температурный коэффициент вязкости"));
            var modeVector = new VectorObject("Vector-3", "Режимный вектор",
                "Вектор, описывающий режимные параметры объекта",
                "Данный вектор описывает режимные параметры объекта управления, такие как скорость движения крышки");
            modeVector.Items.Add(new VectorItem("Vu", "1.0", "Скорость движения верхней крышки"));
            modeVector.Items.Add(new VectorItem("Tu", "200.0", "Температура верхней крышки"));
            var materialPropertiesVector = new VectorObject("Vector-4", "Свойства материала",
                "Вектор, описывающий свойства материала",
                "Данный вектор описывает свойства материала, такие как плотность, удельная теплоемкость");
            materialPropertiesVector.Items.Add(new VectorItem("Ro", "890.0", "Плотность"));
            materialPropertiesVector.Items.Add(new VectorItem("T0", "175.0", "Температура плавления"));
            materialPropertiesVector.Items.Add(new VectorItem("C", "2302.0", "Средняя удельная теплоемкость"));
            var additionalDataVector = new VectorObject("Vector-5", "Вектор дополнительных данных",
                "Вектор, хранящий дополнительные данные",
                "Данный вектор хранит дополнительные данные, такие как шаг дискретизации");
            additionalDataVector.Items.Add(new VectorItem("DiscretizationStep", "0.5", "Шаг дискретизации"));
            VectorObject.Add(geometryVector);
            VectorObject.Add(empiricalVector);
            VectorObject.Add(modeVector);
            VectorObject.Add(materialPropertiesVector);
            VectorObject.Add(additionalDataVector);
        }
    }
}
