using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace Prac1
{
    internal class V3DataCollection : ObservableCollection<V3Data>
    {
        public new V3Data this[int index] {
            get
            {
                return base[index];
            }
        }
        public bool Contains(string ID)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (base[i].ID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(string ID)
        {
            if (Contains(ID) == false)
            {
                return false;
            }
            int index = 0;
            while (base[index].ID != ID)
            {
                ++index;
            }
            for (int i = index; i < Count - 1; ++i)
            {
                base[i] = base[i + 1];
            }
            return Remove(base[Count]);
        }

        public new bool Add(V3Data v3Data)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (base[i].ID == v3Data.ID)
                {
                    return false;
                }
            }
            base.Add(v3Data);
            return true;
        }

        public void AddDefaults()
        {
            double[] nodes = { 1, 2, 3, 4, 5 };
            V3DataList v3DataList = new("001", new(2022, 04, 10));
            v3DataList.AddDefaults(5, V3Data.MeasureField);
            V3DataUGrid v3DataUGrid = new("002", new(2022, 04, 10), new(-10, 5, 5), V3Data.MeasureField);
            V3DataNUGrid v3DataNUGrid = new("003", new(2022, 04, 10), nodes, V3Data.MeasureField);
            Add(v3DataList);
            Add(v3DataUGrid);
            Add(v3DataNUGrid);
        }

        public string ToLongString(string format)
        {
            string info = "";
            for (int i = 0; i < Count; ++i)
            {
                info += $"{base[i].ToLongString(format)}\n";
            }
            return info;
        }

        public override string ToString()
        {
            string info = "";
            for (int i = 0; i < Count; ++i)
            {
                info += $"{base[i].ToString}";
            }
            return info;
        }
        
        public DataItem? MaxAbs {
            get {
                var data = from list in this
                           from item in list
                           select item;

                if (!data.Any()) return null;
               
                var maximum = data.Max(item => Math.Abs(item.X));
                var result = from res in data
                             where (Math.Abs(res.X) == maximum)
                             select res;
                return result.Last();
            }
        }

        
        public IEnumerable<double>? UniqueCoord
        {
            get
            {
                var data = from list in this
                           from item in list
                           select item;

                if (!data.Any()) return null;

                var groups = from item in data
                             orderby item.X
                             group item by item.X into grp
                             select new { key = grp.Key, cnt = grp.Count() };

                var unique = from set in groups
                             where set.cnt > 1
                             select set.key;
                return unique;  
            }
        }

        
        public IEnumerable<IGrouping<double, DataItem>>? CoordGroups
        {
            get
            {
                var data = from list in this
                           from item in list
                           select item;

                if (!data.Any()) return null;

                var groups = from item in data
                             group item by item.X;
                return groups;
            }
        }
        
    }
}
