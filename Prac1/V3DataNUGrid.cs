using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Prac1
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class V3DataNUGrid : V3Data
    {
        [JsonProperty]
        double[] Nodes { get; set; }
        [JsonProperty]
        double[] FirstCoord { get; set; } 
        [JsonProperty]
        double[] SecondCoord { get; set; }

        [JsonProperty]
        public override double MaxDistance
        {
            get
            {
                if (Nodes.Length == 0)
                    return 0;
                double max = Nodes[0];
                double min = Nodes[0];
                for (int i = 0; i < Nodes.Length; ++i)
                {
                    if (Nodes[i] > max)
                        max = Nodes[i];
                    else if (Nodes[i] < min)
                        min = Nodes[i];
                }
                return max - min;
            }
        }
        public V3DataNUGrid(string ID, DateTime time)
            : base(ID, time)
        {
            Nodes = Array.Empty<double>();
            FirstCoord = Array.Empty<double>();
            SecondCoord = Array.Empty<double>();
        }

        public V3DataNUGrid(string ID, DateTime time, double[] nodes, F2Double F)
            : base(ID, time)
        {
            Nodes = nodes;
            FirstCoord = new double[Nodes.Length];
            SecondCoord = new double[Nodes.Length];
            for (int i = 0; i < Nodes.Length; ++i)
            {
                FirstCoord[i] = F(Nodes[i])[0];
                SecondCoord[i] = F(Nodes[i])[1];
            }
        }

        public V3DataNUGrid(double[] nodes, double[] firstCoord, double[] secondCoord, string id, DateTime time)
            : base(id, time)
        {
            Nodes = new double[nodes.Length];
            FirstCoord = new double[nodes.Length];
            SecondCoord = new double[nodes.Length];

            for (int i = 0; i < nodes.Length; ++i)
            {
                Nodes[i] = nodes[i];
                FirstCoord[i] = firstCoord[i];
                SecondCoord[i] = secondCoord[i];
            }

        }
        
        public static explicit operator V3DataList(V3DataNUGrid source)
        {
            V3DataList dataList = new(source.ID, source.Time);
            for (int i = 0; i < source.Nodes.Length; ++i)
            {
                dataList.Add(source.Nodes[i], source.FirstCoord[i], source.SecondCoord[i]);
            }
            return dataList;
        }

        public override string ToString()
        {
            return $"V3DataNUGrid {base.ToString()}, the node quantity is {Nodes.Length}";
        }

        public override string ToLongString(string format)
        {
            string info = $"{ToString()}\n";
            for (int i = 0; i < Nodes.Length; ++i)
            {
                info += $" X  = {Nodes[i].ToString(format)},"      + 
                        $" Y0 = {FirstCoord[i].ToString(format)}," +
                        $" Y1 = {SecondCoord[i].ToString(format)}\n";
            }
            return info;
        }

        public override IEnumerator<DataItem> GetEnumerator()
        {
            for (int i = 0; i < Nodes.Length; ++i)
            {
                yield return new DataItem(Nodes[i], FirstCoord[i], SecondCoord[i]);
            }
        }

        public static bool Save(string filename, V3DataNUGrid V3)
        {
            FileStream? fileStream = null;
            try
            {
                var jsonText = JsonConvert.SerializeObject(V3);
                if (File.Exists(filename))
                {
                    File.Delete(filename);

                }
                File.WriteAllText(filename, jsonText);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"[Json Serialization ERROR] : {e.Message}");
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public static bool Load(string filename, ref V3DataNUGrid V3)
        {
            FileStream? fileStream = null;
            try
            {
                var jsonText = File.ReadAllText(filename);
                V3 = JsonConvert.DeserializeObject<V3DataNUGrid>(jsonText)!;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Json Deserialization ERROR] : {e.Message}");
                return false;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
    }
}
