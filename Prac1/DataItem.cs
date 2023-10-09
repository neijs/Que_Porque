using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prac1
{
    internal struct DataItem
    {
        public double   X { get; set; }
        public double[] Y { get; set; }

        public DataItem(double x, double y0, double y1) {
            X = x;
            Y = new double[2];
            Y[0] = y0;
            Y[1] = y1;
        }

        public override string ToString()
        {
            return $"X = {X}, Y0 = {Y[0]}, Y1 = {Y[1]}";
        }

        public string ToLongString(string format) 
        {
            return $"X = {X.ToString(format)}, Y0 = {Y[0].ToString(format)}, Y1 = {Y[1].ToString(format)}.";
        }
    }

}
