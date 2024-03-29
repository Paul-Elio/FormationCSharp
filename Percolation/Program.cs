﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    class Program
    {
        static void Main(string[] args)
        {
            PercolationSimulation perco = new PercolationSimulation();
            PclData pcldata = perco.MeanPercolationValue(15, 50);
            Console.Write("Pour 50 tentative de taille 15 :");
            Console.WriteLine($"Mean = {pcldata.Mean} STD = {pcldata.StandardDeviation} Fraction = {pcldata.Fraction}");
            // Keep the console window open
            Console.WriteLine("----------------------");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
/**
 * Réponses aux questions :
 * 3) b) Dans le pire des cas on ouvre toutes les cases de la grilles sauf celles d'une ligne qui est ouverte à la fin
 *       dans ce cas là le nombre d'opération serai en O((N-1)^2) pour une grille de taille NxN 
 *    c) Plus la taille de la grille sera grande, moins on a de chance d'esquiver une même ligne à chaque fois comme l'ouverture des cases est Random.
 *    
 */