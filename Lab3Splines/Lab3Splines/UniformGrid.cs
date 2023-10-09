using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lab3Splines
{
    internal struct UniformGrid
    {
        public double Left { get; set; }

        public double Step { get; set; }

        public int Amount { get; set; }

        public double Right
        {
            get
            {
                return Left + (Amount - 1) * Step;
            }
        }

        public UniformGrid(double left, double step, int amount)
        {
            Left = left;
            Step = step;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"Left border is {Left}\n" +
                   $"Step is {Step}\n" +
                   $"Number of nodes is {Amount}\n";
        }

        public string ToLongString(string format)
        {
            return $"Left border is {Left.ToString(format)}\n" +
                   $"Step is {Step.ToString(format)}\n" +
                   $"Number of nodes is {Amount.ToString(format)}\n";
        }
    }
}