using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    class Program
    {
        static void Main(string[] args)
        {
            Gestion_Compte banque = new Gestion_Compte();
            banque.Create_compte(0, 100);
            banque.Create_compte(0, 100000);
            banque.Create_compte(1, 100);
            banque.Create_compte(2);
            banque.Display_comptes();
            banque.Depot(0, 500);
            banque.Retrait(1, 50);
            banque.Retrait(2, 500);
            banque.Display_comptes();
            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
