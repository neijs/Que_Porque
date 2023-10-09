using Labb3;
internal class Program
{
    private static void Main(string[] args)
    {
        /* ------------------- Calculating tan of SMALL amount of arguments --------------------- */
        double[] nodes = { 0, Math.PI / 1000, Math.PI / 6, Math.PI / 4, Math.PI / 3, Math.PI / 2 };
        VML_Test test1 = new(ref nodes);
        test1.VML_Init();
        Console.WriteLine(test1.ToLongString());

        /* ------------------- Calculating tan of LARGE amount of arguments --------------------- */
        VML_Test test1000 = new(1000);
        test1000.VML_Init();
        Console.WriteLine(test1000.ToString());

        VML_Test test100000 = new(100000);
        test100000.VML_Init();
        Console.WriteLine(test100000.ToString());

        VML_Test test10000000 = new(10000000);
        test10000000.VML_Init();
        Console.WriteLine(test10000000.ToString());
    }
}