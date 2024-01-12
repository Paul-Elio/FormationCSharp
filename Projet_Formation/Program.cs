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

            string path = Directory.GetCurrentDirectory();
            for (int i = 1; i < 7; i++)
            {
                #region Files
                // Input
                string mngrPath = path + $@"\Gestionnaires_{i}.txt";
                string oprtPath = path + $@"\Comptes_{i}.txt";
                string trxnPath = path + $@"\Transactions_{i}.txt";
                // Output
                string sttsOprtPath = path + $@"\StatutOpe_{i}.txt";
                string sttsTrxnPath = path + $@"\StatutTra_{i}.txt";
                string mtrlPath = path + $@"\Metrologie_{i}.txt";
                #endregion

                #region Main
                if (File.Exists(mngrPath) && File.Exists(oprtPath) && File.Exists(trxnPath))
                {
                    Gestion_Compte banque = new Gestion_Compte();
                    banque.Gestion(mngrPath, trxnPath, oprtPath, sttsOprtPath, sttsTrxnPath, mtrlPath);

                }
                #endregion
            }
            
            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
