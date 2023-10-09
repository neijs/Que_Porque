using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prac1
{
    internal class V3DataUGrid : V3Data
    {
        public UniformGrid Grid { get; set; }
        double[] FirstCoord { get; set; }
        double[] SecondCoord { get; set; }
        public override double MaxDistance
        {
            get
            {
                return Grid.Right - Grid.Left;
            }
        }
        public V3DataUGrid(string ID, DateTime time) : base(ID, time)
        {
            FirstCoord = Array.Empty<double>();
            SecondCoord = Array.Empty<double>();
        }
        public V3DataUGrid(string ID, DateTime time, UniformGrid grid, F2Double F) : base(ID, time)
        {
            Grid = grid;
            FirstCoord = new double[Grid.Amount];
            SecondCoord = new double[Grid.Amount];
            

            for (int i = 0; i < Grid.Amount; ++i)
            {
                FirstCoord[i] = F(Grid.Left + i * Grid.Step)[0];
                SecondCoord[i] = F(Grid.Left + i * Grid.Step)[1];
            }

        }
        public override string ToString()
        {
            return $"V3DataUGrid {base.ToString()} {Grid}";
        }
        public override string ToLongString(string format)
        {
            string info = $"{ToString()}\n";
            for (int i = 0; i < Grid.Amount; ++i)
            {
                double x = Grid.Left + i * Grid.Step;
                double y0 = FirstCoord[i];
                double y1 = SecondCoord[i];
                info += $"X = {x.ToString(format)}, Y0 = {y0.ToString(format)}, Y1 = {y1.ToString(format)}\n";
            }
            return info;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Grid.Amount; ++i)
            {
                yield return new DataItem(Grid.Left + i * Grid.Step, FirstCoord[i], SecondCoord[i]);
            }
        }
    }
}
