using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public static class Search
    {
        public static int LinearSearch(int[] tableau, int valeur)
        {
            for (int i=0; i< tableau.Length; i++)
            {
                if (tableau[i] == valeur)
                {
                    return i;
                }
            }
            Console.WriteLine("Valeur non présente dans le tableau");
            return -1;
        }

        public static int BinarySearch(int[] tableau, int valeur)
        {
            int n = 0;
            int m = tableau.Length;
            while (true)
            { 
                int i = (n + m) / 2;
                if (tableau[i] > valeur)
                {
                    m = i;
                }
                else if (tableau[i] < valeur)
                {
                    n = i;
                }
                else if (tableau[i] == valeur)
                {
                    return i;
                }
                else
                {
                    Console.WriteLine("Valeur non présente dans le tableau");
                    return -1;
                }
            }
            return -1;
        }
    }
}
