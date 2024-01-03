using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Euclide
    {
        public static int Pgcd(int a, int b)
        {
            int r = b;
             while (true)
            {
                b = r;
                r = a % b;
                a = b;
                if (r ==0) { return b; }
            }
        }

        public static int PgcdRecursive(int a, int b)
        {
            if (b == 0)
            {
                return a; // par convention PGCD(n,0) = n 
            }
            else
            {
                return PgcdRecursive(b, a % b);
            }
        
        }
    }
}
