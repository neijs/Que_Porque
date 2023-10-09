using Prac1;

internal class Program
{
    private static void Main(string[] args)
    {
        Test1();
        //Test2();
    }

    public static void Test1()
    {
        Console.WriteLine(" *** TESTING SAVE/LOAD V3DATANUGRID MECHANICS *** ");

        double[] nodes1 = { -10, -4, -1,  0, 34, 56, 1045 };
        double[] nodes2 = {};

        string existing_file_name = "existingFile.json";
        string not_existing_file_name1 = "sukablya.json";
        string not_existing_file_name2 = "notExistingFile2.json";

        V3DataNUGrid nugrid_save = new("saved_one", DateTime.Now, nodes1, V3Data.MeasureField);
        V3DataNUGrid nugrid_load = new("loaded_one", DateTime.Now, nodes2, V3Data.MeasureField);

        Console.WriteLine(nugrid_save.ToLongString("f2"));
        Console.WriteLine(nugrid_load.ToLongString("f2"));


        V3DataNUGrid.Save(null, nugrid_save);                        /* отсутствие файла          */
        V3DataNUGrid.Save(not_existing_file_name1, nugrid_save);     /* несуществующий файл       */
        V3DataNUGrid.Save(existing_file_name, nugrid_save);          /* всё в порядке             */

        V3DataNUGrid.Load(null, ref nugrid_load);                    /* отсутствие файла          */
        V3DataNUGrid.Load(not_existing_file_name2, ref nugrid_load); /* несуществующий файл       */
        //V3DataNUGrid.Load(existing_file_name, ref nugrid_load);      /* всё в порядке             */
        V3DataNUGrid.Load("Prac1.dll", ref nugrid_load);      /* всё в порядке             */

        Console.WriteLine(nugrid_load.ToLongString("f2"));


        return;    
    }

    public static void Test2()
    {
        Console.WriteLine(" *** TESTING LINQ PROPERTIES *** ");

        /* ----------------------------- Empty collection ------------------------------ */

        V3DataCollection collection = new();

        V3DataList empty_list = new("Empty_V3DataList", new DateTime(2022, 11, 15));
        V3DataUGrid empty_ugrid = new("Empty_V3DataUGrid", new DateTime(2022, 11, 15));
        V3DataNUGrid empty_nugrid = new("Empty_V3DataNUGrid", new DateTime(2022, 11, 15));

        collection.Add(empty_list);
        collection.Add(empty_ugrid);
        collection.Add(empty_nugrid);

        Console.WriteLine("Empty collection :\n");
        Console.WriteLine(collection.ToLongString("f2"));

        Console.WriteLine("Testing LINQ 1 property (must be null) : ");
        if (collection.MaxAbs == null) { Console.WriteLine("null"); }
        Console.WriteLine("Testing LINQ 2 property (must be null) : ");
        if (collection.UniqueCoord == null) { Console.WriteLine("null");  }
        Console.WriteLine("Testing LINQ 3 property (must be null) : ");
        if (collection.CoordGroups == null) { Console.WriteLine("null"); }

        /* ------------------------------------------------------------------------------ */

        /* ---------------------------- Filled collection ------------------------------- */

        collection.AddDefaults();
        Console.WriteLine("\n\n\nFilled collection :\n");
        Console.WriteLine(collection.ToLongString("f2"));

        Console.WriteLine($"Testing LINQ 1 property : {collection.MaxAbs}\n");
        Console.WriteLine("Testing LINQ 2 property : \n");
        foreach(var unique in collection.UniqueCoord!)
        {
            Console.WriteLine($"{unique}");
        }
        Console.WriteLine("Testing LINQ 3 property :\n");
        foreach (var group in collection.CoordGroups!)
        {
            Console.WriteLine($"Groups coordinate is {group.Key}");
            foreach (var item in group)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine();
        }

        /* ------------------------------------------------------------------------------ */

        return;
    }

}