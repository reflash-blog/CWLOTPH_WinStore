using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWLOTPH.DataObject;

namespace CWLOTPH_WinSTORE.DataModel
{
    public class ProcessingDataObject
    {
        public static InputDataObject InputDataObject { get; set; }
        public static OutputDataObject OutputDataObject { get; set; }

        private static readonly InputDataObject ConstantInputDataObject = new InputDataObject
        {
            DiscretizationStep = 0.5,
            EmpiricalVector = new EmpiricalVector
            {
                alphaU = 2000,
                b = 0.015,
                Mu = 1550,
                n = 0.4,
                Tgamma = 180
            },
            GeometryVector = new GeometryVector
            {
                H = 0.03,
                L = 6.6,
                W = 0.17
            },
            MaterialProperties = new MaterialProperties
            {
                C = 2302.0,
                Ro = 890.0,
                T0 = 175.0
            },
            ModeVector = new ModeVector
            {
                Tu = 200.0,
                Vu = 1.0
            }
        };

        public static void Clear()
        {
            InputDataObject = ConstantInputDataObject;
            OutputDataObject = null;
        }

        public static void Process()
        {
            OutputDataObject = new OutputDataObject();
            double Fd, gamma, Q, Qk, qa, qr, coord;
            int i;
            //int arrSize = (int)(inpDObj.L / inpDObj.step) + 1;
            //if ((arrSize * inpDObj.step) < (inpDObj.L + inpDObj.step / 2))
            //    arrSize++;
            var T = new List<double>();
            var X = new List<double>();
            var nu = new List<double>();
            var V = new List<double>();

            //// Расчеты
            Fd = 0.125 * (InputDataObject.GeometryVector.H / InputDataObject.GeometryVector.W) * (InputDataObject.GeometryVector.H / InputDataObject.GeometryVector.W)
                - 0.625 * (InputDataObject.GeometryVector.H / InputDataObject.GeometryVector.W) + 1;
            //// скорость диффер. вывода
            gamma = InputDataObject.ModeVector.Vu / InputDataObject.GeometryVector.H;
            //// производительность канала
            Q = (InputDataObject.GeometryVector.W * InputDataObject.GeometryVector.H / 2) * InputDataObject.ModeVector.Vu * Fd;
            Qk = InputDataObject.MaterialProperties.Ro * Q;
            ////Расчёт удельных тепловых потоков
            qa = InputDataObject.GeometryVector.W * (InputDataObject.EmpiricalVector.alphaU / InputDataObject.EmpiricalVector.b - InputDataObject.EmpiricalVector.alphaU * InputDataObject.ModeVector.Tu
                + InputDataObject.EmpiricalVector.alphaU * InputDataObject.EmpiricalVector.Tgamma); //heatFluxW
            qr = InputDataObject.GeometryVector.W * InputDataObject.GeometryVector.H * InputDataObject.EmpiricalVector.Mu * Math.Pow(gamma, InputDataObject.EmpiricalVector.n + 1); //heatFluxN

            coord = 0;
            i = 0;
            while (coord < (InputDataObject.GeometryVector.L + InputDataObject.DiscretizationStep / 2))
            {
                X.Add(coord);
                T.Add(Temperature(Q, qa, qr, coord, InputDataObject));
                V.Add(Viscosity(T[i], InputDataObject));
                coord += InputDataObject.DiscretizationStep;
                i++;
            }

            //// Запись данных в объект
            OutputDataObject.lengthCoordinates = X;
            OutputDataObject.temperature = T;
            OutputDataObject.viscosity = V;
            OutputDataObject.Qk = Qk;
            OutputDataObject.Tend = T[i - 1];
            OutputDataObject.Vend = V[i - 1];
        }

        private static double Temperature(double Q, double qa, double qr, double coord, InputDataObject inpObj)
        {
            return inpObj.EmpiricalVector.Tgamma + (1 / inpObj.EmpiricalVector.b) * Math.Log(((inpObj.EmpiricalVector.b * qr + inpObj.GeometryVector.W *
                inpObj.EmpiricalVector.alphaU) /
                (inpObj.EmpiricalVector.b * qa)) *
                                                      (1 - Math.Exp(-(inpObj.EmpiricalVector.b * qa * coord) /
                                                      (inpObj.MaterialProperties.Ro * inpObj.MaterialProperties.C * Q)))
                                                      +
                                                      Math.Exp(inpObj.EmpiricalVector.b *
                                                          (inpObj.MaterialProperties.T0 - inpObj.EmpiricalVector.Tgamma -
                                                           (qa * coord) / (inpObj.MaterialProperties.Ro * inpObj.MaterialProperties.C * Q))));
        }

        // расчёт вязкости материала 
        private static double Viscosity(double T, InputDataObject inpObj)
        {
            return inpObj.EmpiricalVector.Mu * Math.Exp(-inpObj.EmpiricalVector.b * (T - inpObj.EmpiricalVector.Tgamma))
                * Math.Pow((inpObj.ModeVector.Vu / inpObj.GeometryVector.H), inpObj.EmpiricalVector.n - 1);
        }
    }
}
