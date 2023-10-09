using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Labb3
{
    internal class VML_Test
    {
        [DllImport("Labb3Dll.dll")]

        public static extern void TanRequest(int size, double[] src, double[] vdTanRes, double[] tgRes, ref long vdTanTime, ref long tanTime);
        public double[] FuncArg { get; private set; }
        public double[] VdTanRes { get; private set; }
        public double[] TanRes { get; private set; }
        public double MaxDiff {
            get {
                double maxDiff = -1;
                for (int i = 0; i < FuncArg.Length; ++i)
                {
                    double curDiff = Math.Abs(VdTanRes[i] - TanRes[i]);
                    if (curDiff > maxDiff)
                    {
                        maxDiff = curDiff;
                    }
                }
                return maxDiff;
            }
            private set { }
        }
        public double MaxDiffArg {
            get {
                double maxDiffArg = 0;
                double maxDiff = -1;
                for (int i = 0; i < FuncArg.Length; ++i)
                {
                    double curDiff = Math.Abs(VdTanRes[i] - TanRes[i]);
                    if (curDiff > maxDiff)
                    {
                        maxDiff = curDiff;
                        maxDiffArg = FuncArg[i];
                    }
                }
                return maxDiffArg;
            }
            private set { } 
        }
        public long VdTanRunTime { get; private set; }
        public long TanRunTime { get; private set; }
        public double TimeFactor {
            get {
                return (double) (1.0 * TanRunTime / VdTanRunTime);
            }
            private set { } 
        }

        public VML_Test(ref double[] funcArg)
        {
            FuncArg = funcArg;
            VdTanRes = new double[FuncArg.Length];
            TanRes = new double[FuncArg.Length];
        }

        public VML_Test(long size)
        {
            FuncArg = new double[size];
            double left = -Math.PI / 2;
            double step = Math.PI / (size - 1);
            for (int i = 0; i < size; ++i)
            {
                FuncArg[i] = left + i * step;
            }
            VdTanRes = new double[size];
            TanRes = new double[size];
        }

        public override string ToString()
        {
            string info = $"Func argument quantity is {FuncArg.Length},\n" +
                $"Maximum of the difference is {MaxDiff},\n" +
                $"Maximum of the difference argument is {MaxDiffArg},\n" +
                $"Execution time of vdTan is {VdTanRunTime} microseconds,\n" +
                $"Execution time of Tan is {TanRunTime} microseconds,\n" +
                $"Time ratio coefficient is {TimeFactor}, that is ";
            if (VdTanRunTime < TanRunTime)
            {
                info += $"vdTan {(double)(1.0 * TanRunTime / VdTanRunTime)} faster than tan.\n";
            }
            else if (VdTanRunTime > TanRunTime)
            {
                info += $"tan {(double)(1.0 * VdTanRunTime / TanRunTime)} faster than vdTan.\n";
            }
            else
            {
                info += $"tan and vdTan are the same in speed.\n";
            }
            return info;
        }

        public string ToLongString()
        {
            string info = $"{ToString()}\n" +
                          $"Additional information about VML_Test:\n";
            info += "Values of function arguments: ";
            foreach (var arg in FuncArg)
            {
                info += $"{arg} ";
            }
            info += "\nValues of vdTan: ";
            foreach (var arg in VdTanRes)
            {
                info += $"{arg} ";
            }
            info += "\nValues of tan: ";
            foreach (var arg in TanRes)
            {
                info += $"{arg} ";
            }
            return info + "\n";
        }

        public void Save(string filename)
        {
            string text = ToString();
            FileStream? fs = null;
            try
            {
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, text + "------------------------------------------------\n");
                }
                else
                {
                    File.WriteAllText(filename, text + "------------------------------------------------\n");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ERROR occurred while saving: {exc}");
            }
            finally
            {
                fs?.Close();
            }
        }

        public void VML_Init()
        {
            double[] tmpVdTanRes = new double[FuncArg.Length];
            double[] tmpTanRes = new double[FuncArg.Length];
            long tmpVdTanRunTime = 0, tmpTanRunTime = 0;
            try
            {
                TanRequest(FuncArg.Length, FuncArg, tmpVdTanRes, tmpTanRes, ref tmpVdTanRunTime, ref tmpTanRunTime);
                VdTanRes = tmpVdTanRes;
                TanRes = tmpTanRes;
                VdTanRunTime = tmpVdTanRunTime;
                TanRunTime = tmpTanRunTime;
            }
            catch (Exception exc)
            {
                Console.WriteLine($"ERROR occurred while executing C++ code: {exc}");
            }
            Save("Labb3OUTPUT.txt");
        }
    } 
}
