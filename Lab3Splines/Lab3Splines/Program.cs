using System.Runtime.InteropServices;
using Lab3Splines;
internal class Program
{
    private static void Main(string[] args)
    {
        /* ---------------------------------- ( 1 ) ------------------------------------------ */ 
        const int oldNodes = 11, newNodes = 15;
        UniformGrid ugrid = new(-1.0, 1.0 / 5.0, oldNodes);
        V3DataUGrid v3dataugrid = new("DataUGrid", DateTime.Now, ugrid, V3Data.MeasureField);

        double[] deriv = { -34.0, 26.0 }; // left, right
        double[] nugrid = new double[newNodes] 
        { 
            -1.00, -0.80, -0.75, -0.60, -0.40, // -0.75          added
            -0.20, -0.09,  0.00,  0.20,  0.34, // -0.09 and 0.34 added
             0.40,  0.60,  0.80,  0.96,  1.00, //  0.96          added
        };
        double[] bounds = new double[2] { -1.0, 1.0 };

        V3DataUGridSpline v3dataugridspline = new(ref v3dataugrid, deriv, nugrid, bounds);
        v3dataugridspline.BuildSplines();
        v3dataugridspline.Save("SplinesLOG.txt", "f");

        /* ---------------------------------- ( 2 ) ------------------------------------------ */
        double[] derivA = { -34.0, 26.0 }; // left, right
        double[] derivB = {   0.0,  0.0 }; // left, right

        V3DataUGridSpline splineA = new(ref v3dataugrid, derivA, nugrid, bounds);
        splineA.BuildSplines();
        V3DataUGridSpline splineB = new(ref v3dataugrid, derivB, nugrid, bounds);
        splineB.BuildSplines();
        
        string info = "Let's compare the results of calculating the measurement values and the " +
            "second derivatives of spline A and spline B:\n\n" +
            "The difference between VALUE calculations of the FIRST measurment:\n";
        for (int i = 0; i < newNodes; ++i)
        {
            if (i == 0)
            {
                info += "[";
            }
            info += $"{splineA.NUGridMES1[i] - splineB.NUGridMES1[i]:f5}";
            info += (i == newNodes - 1) ? "]\n\n" : ", ";
        }
        info += "The difference between VALUE calculations of the SECOND measurment:\n";
        for (int i = 0; i < newNodes; ++i)
        {
            if (i == 0)
            {
                info += "[";
            }
            info += $"{splineA.NUGridMES2[i] - splineB.NUGridMES2[i]:f5}";
            info += (i == newNodes - 1) ? "]\n\n" : ", ";
        }
        info += "The difference between DERIVATIVE of the FIRST measurment:\n";
        for (int i = 0; i < newNodes; ++i)
        {
            if (i == 0)
            {
                info += "[";
            }
            info += $"{splineA.NUGridDER1[i] - splineB.NUGridDER1[i]:f5}";
            info += (i == newNodes - 1) ? "]\n\n" : ", ";
        }
        info += "The difference between DERIVATIVE of the SECOND measurment:\n";
        for (int i = 0; i < newNodes; ++i)
        {
            if (i == 0)
            {
                info += "[";
            }
            info += $"{splineA.NUGridDER2[i] - splineB.NUGridDER2[i]:f5}";
            info += (i == newNodes - 1) ? "]\n\n" : ", ";
        }
        info += "The difference between integrals:\n" +
            $"FIRST  measurment: {splineA.NUGridINTG[0] - splineB.NUGridINTG[0]}\n" +
            $"SECOND measurment: {splineA.NUGridINTG[1] - splineB.NUGridINTG[1]}\n";
        Console.WriteLine(info);
    }
}