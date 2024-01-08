using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public struct PclData
    {
        /// <summary>
        /// Moyenne 
        /// </summary>
        public double Mean { get; set; }
        /// <summary>
        /// Ecart-type
        /// </summary>
        public double StandardDeviation { get; set; }
        /// <summary>
        /// Fraction
        /// </summary>
        public double Fraction { get; set; }
    }

    public class PercolationSimulation
    {
        public PclData MeanPercolationValue(int size, int t)
        {
            double mean = 0;
            double ecty = 0;
            for (int i = 0; i< t; i++)
            {
                double temp = PercolationValue(size);
                mean += temp;
                ecty += temp * temp;
            }
            mean = mean / t;
            ecty = Math.Sqrt((ecty - mean * mean)/(double)t);
            PclData pclData = new PclData();
            pclData.Mean = mean;
            pclData.StandardDeviation = ecty;
            pclData.Fraction = ecty == 0 ? -1 : (double)(mean / ecty);
            return pclData;
        }

        public double PercolationValue(int size)
        {
            int count = 0;
            Percolation perco = new Percolation(size);
            var ran = new Random();
            while (!perco.Percolate())
            {
                int i = ran.Next(0, size);
                int j = ran.Next(0, size);
                if (!perco.IsOpen(i, j))
                {
                    perco.Open(i, j);
                    count += 1;
                }
            }
            double frac = (double)(count / (double)(size * size));
            return frac;
        }
    }
}
