using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab3Splines
{
    internal class V3DataUGridSpline
    {
        [DllImport("SplinesDLL.dll")]
        public static extern void MKLSplines(int nodesQnty, int nodesQntyNU, double[] bounds,
            double[] firstValue, double[] secondValue, double[] deriv, double[] nuGrid, double[] mes1,
            double[] mes2, double[] der1, double[] der2, double[] integValue, ref int respCode);
        public V3DataUGrid UGrid { get; private set; }
        public double[] Deriv { get; private set; }
        public double[] NUGrid { get; private set; }
        public double[] Bounds { get; private set; }
        public double[] NUGridMES1 { get; private set; }
        public double[] NUGridMES2 { get; private set; }
        public double[] NUGridDER1 { get; private set; }
        public double[] NUGridDER2 { get; private set; }
        public double[] NUGridINTG { get; private set; }

        public V3DataUGridSpline(ref V3DataUGrid ugrid, double[] deriv, double[] nugrid, double[] bounds)
        {
            UGrid = ugrid;
            Deriv = deriv;
            NUGrid = nugrid;
            Bounds = bounds;
            NUGridMES1 = new double[NUGrid.Length];
            NUGridMES2 = new double[NUGrid.Length];
            NUGridDER1 = new double[NUGrid.Length];
            NUGridDER2 = new double[NUGrid.Length];
            NUGridINTG = new double[2];
        }

        public void BuildSplines()
        {
            int nodesQnty = UGrid.Grid.Amount;
            int nodesQntyNU = NUGrid.Length;
            int respCode = 0;

            try
            {
                MKLSplines(nodesQnty, nodesQntyNU, Bounds, UGrid.MES1, UGrid.MES2, Deriv, NUGrid, NUGridMES1,
                    NUGridMES2, NUGridDER1, NUGridDER2, NUGridINTG, ref respCode);
            }
            catch(Exception exc)
            {
                Console.WriteLine($"ERROR occurred while calculating splines: {exc}\n");
            }
            Console.WriteLine($"BuildSpline exited with code: {respCode}\n");
            return;
        }
        public void Save(string filename, string format)
        {
            string text = ToLongString(format);
            FileStream? fs = null;
            try
            {
                if (File.Exists(filename))
                {
                    File.AppendAllText(filename, text + "------------------------------------------------\n");
                }
                else
                {
                    File.WriteAllText(filename, text +  "------------------------------------------------\n");
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
        public string ToLongString(string format)
        {
            string info = $"{UGrid.ToLongString(format)}\n" +
                $"Left and right derivatives: {Deriv[0]}, {Deriv[1]}\n" +
                $"The segment of integration is [{Bounds[0]}, {Bounds[1]}]\n" +
                $"The value of integrals:\n" +
                $"MES1: {NUGridINTG[0]}\n" +
                $"MES2: {NUGridINTG[1]}\n\n" +
                $"Field values on the new non-uniform grid: \n";

            for (int i = 0; i < NUGrid.Length; ++i)
            {
                info += String.Format("x = {0, 5}, value = ({1, 5}, {2, 5})," +
                    " der = ({3, 6}, {4, 6})\n", NUGrid[i].ToString(format), NUGridMES1[i].ToString(format), 
                    NUGridMES2[i].ToString(format), NUGridDER1[i].ToString(format), NUGridDER2[i].ToString(format));
            }
            return info;
        }
    }
}
