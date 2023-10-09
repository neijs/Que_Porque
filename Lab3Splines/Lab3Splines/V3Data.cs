using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Lab3Splines
{
    delegate double[] F2Double(double x);
    internal abstract class V3Data : IEnumerable<DataItem> // interface added
    {
        public string ID { get; set; } //set added 
        public DateTime Time { get; set; } //set added
        public abstract double MaxDistance { get; }

        public V3Data(string id, DateTime time)
        {
            ID = id;
            Time = time;
        }

        public abstract string ToLongString(string format);

        public override string ToString()
        {
            return String.Format("Data ID: {0}\n" +
                   "Data timestamp: {1:t} {1:d}\n", ID, Time);
        }
        public static double[] MeasureField(double p)
        {
            double[] measurment = new double[2];
            measurment[0] = 5 * p * p * p - 2 * p * p - 7 * p + 4; //  5p^3 - 2p^2 - 7p + 4
            measurment[1] = -2 * p * p * p + p * p + 7 * p - 1;    // -2p^3 + 1p^2 + 7p - 1
            return measurment;
        }

        public abstract IEnumerator<DataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
