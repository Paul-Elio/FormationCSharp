using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class Pyramid
    {
        public static void PyramidConstruction(int n, bool isSmooth)
        {
            if (isSmooth)
            {
                for (int i=0; i < n; i++)
                {
                    for (int j = 1; j < n - i; j++)
                    {
                        Console.Write(" ");
                    }
                    for (int j = 0; j <= 2*i ; j++)
                    {
                        Console.Write("+");
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 1; j < n - i; j++)
                    {
                        Console.Write(" ");
                    }
                    if (i % 2 == 0)
                    {
                        for (int j = 0; j <= 2 * i; j++)
                        {
                            Console.Write("+");
                        }
                    }
                    else
                    {
                        for (int j = 0; j <= 2 * i; j++)
                        {
                            Console.Write("-");
                        }
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
