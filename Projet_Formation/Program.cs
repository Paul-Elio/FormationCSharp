using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Formation
{
    class Program
    {
        static void Main(string[] args)
        {
            Gestion_Compte banque = new Gestion_Compte();
            string path = Directory.GetCurrentDirectory();
            #region Files
            // Input
            string acctPath = path + @"\Comptes_1.csv";
            string trxnPath = path + @"\Transactions_1.csv";
            // Output
            string sttsPath = path + @"\Statut_1.csv";
            #endregion
            banque.Gestion(acctPath, trxnPath, sttsPath);
            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
