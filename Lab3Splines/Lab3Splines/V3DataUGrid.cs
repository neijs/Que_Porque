using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3Splines
{
    internal class V3DataUGrid : V3Data
    {
        public UniformGrid Grid { get; set; }
        public double[] MES1 { get; set; }
        public double[] MES2 { get; set; }
        public override double MaxDistance
        {
            get
            {
                return Grid.Right - Grid.Left;
            }
        }
        public V3DataUGrid(string ID, DateTime time) : base(ID, time)
        {
            MES1 = Array.Empty<double>();
            MES2 = Array.Empty<double>();
        }
        public V3DataUGrid(string ID, DateTime time, UniformGrid grid, F2Double F) : base(ID, time)
        {
            Grid = grid;
            MES1 = new double[Grid.Amount];
            MES2 = new double[Grid.Amount];


            for (int i = 0; i < Grid.Amount; ++i)
            {
                MES1[i] = F(Grid.Left + i * Grid.Step)[0];
                MES2[i] = F(Grid.Left + i * Grid.Step)[1];
            }

        }
        public override string ToString()
        {
            return $"V3DataUGrid: {base.ToString()}{Grid}";
        }
        public override string ToLongString(string format)
        {
            string info = $"{ToString()}";
            for (int i = 0; i < Grid.Amount; ++i)
            {
                double x = Grid.Left + i * Grid.Step;
                double y0 = MES1[i];
                double y1 = MES2[i];
                info += String.Format($"X = {x.ToString(format), 5}, FIELD = ({y0.ToString(format), 5}, {y1.ToString(format), 5})\n");
            }
            return info;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Grid.Amount; ++i)
            {
                yield return new DataItem(Grid.Left + i * Grid.Step, MES1[i], MES2[i]);
            }
        }
    }
}