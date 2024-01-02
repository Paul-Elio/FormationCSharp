using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Factorial
    {
        public static int Factorial_(int n)
        {
            if (n >= 0)
            {
                int res = 1;
                for (int i = n; i > 0; i--)
                {
                    res *= i;
                }
                return res;
            }
            else
            {
                Console.WriteLine("Opération invalide ");

            }
            return -1;
        }

        public static int FactorialRecursive(int n)
        {
            if (n > 0)
            {
                int res = n * FactorialRecursive(n - 1);
                return res;
               
            }
            else if (n == 0)
            {
                return 1;
            }
            else
            {
                Console.WriteLine("Opération invalide ");

            }
            return -1;
        }
    }
}
