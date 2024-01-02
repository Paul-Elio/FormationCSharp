using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_intm1
{
    class Program
    {
        static void Main(string[] args)
        {
            string txt1 = "hello world";
            Console.WriteLine(txt1);
            txt1 = "ho hi mark";
            string txt2 = "  heyyy";
            Console.WriteLine(txt1+txt2);
            Console.WriteLine("press any key to continue");
            Console.ReadKey()
                ;
        }
    }
}
