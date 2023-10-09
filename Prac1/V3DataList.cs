using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prac1
{
    internal class V3DataList : V3Data
    {
        public List<DataItem> Items { get; set; }

        public override double MaxDistance {
            get
            {
                if (Items.Count == 0)
                {
                    return 0;
                }
                double max = Items[0].X;
                double min = Items[0].X;
                foreach (DataItem item in Items)
                {
                    if (item.X > max)
                    {
                        max = item.X;
                    }
                    else if (item.X < min)
                    {
                        min = item.X;
                    }
                }
                return max - min;
            }
        }

        public V3DataList(string ID, DateTime time) : base(ID, time)
        {
            Items = new List<DataItem>();
        }

        public bool Add(double x, double v1, double v2)
        {
            foreach (DataItem item in Items)
            {
                if (item.X == x)
                {
                    return false;
                }
            }
            DataItem dataItem = new (x, v1, v2);
            Items.Add(dataItem);
            return true;
        }

        public void AddDefaults(int nItems, F2Double F)
        {
            double x;

            for (int i = 0; i < nItems; ++i)
            {
                x = Math.Cosh(i);
                Add(x, F(x)[0], F(x)[1]);
            }
        } 
        public override string ToString()
        {
            return $"V3DataList {base.ToString()}, quantity is {Items.Count}";
        }

        public override string ToLongString(string format)
        {
            string info = $"{ToString()}\n";
            foreach (DataItem item in Items)
            {
                info += $"{item.ToLongString(format)}\n";
            }
            return info;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Items.Count; ++i)
            {
                yield return Items[i];
            }
        }
    }
}
