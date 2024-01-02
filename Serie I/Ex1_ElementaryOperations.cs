using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class ElementaryOperations
    {
        public static void BasicOperation(int a, int b, char operation)
        {
            switch (operation)
            {
                case '/' :
                    if (b != 0)
                    {
                        Console.WriteLine($"{a} {operation} {b} = {a / b}");
                    }
                    else
                    {
                        Console.WriteLine($"{a} {operation} {b} = Opération invalide.");
                    }
                    break;

                case '+':
                    Console.WriteLine($"{a} {operation} {b} = {a + b}");
                    break;

                case '-':
                    Console.WriteLine($"{a} {operation} {b} = {a - b}");
                    break;

                case '*':
                    Console.WriteLine($"{a} {operation} {b} = {a * b}");
                    break;

                default:
                    Console.WriteLine($"{a} {operation} {b} = Opération invalide.");
                    break;

            }

        }

        public static void IntegerDivision(int a, int b)
        {
            if (b != 0)
            {
                Console.WriteLine($"{a} = {a / b} * {b} + {a%b}");
            }
            else
            {
                Console.WriteLine($"{a} : 0 = Opération invalide.");
            }
        }

        public static void Pow(int a, int b)
        {
            if (b >= 0)
            {
                int res = 1;
                for (int i = b; i>0; i--)
                {
                    res *= a;
                }               
                Console.WriteLine($"{a} ^ {b} = {res}"); 

            }
            else
            {
                Console.WriteLine($"{a} ^ {b} = Opération invalide.");
            }
        }
    }
}
