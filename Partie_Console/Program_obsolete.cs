using System;
using System.Collections.Generic;

namespace Partie_Console
{
    public class Program_obsolete
    {
        static void Main(string[] args)
        {
            Ville_obsolete v1 = new Ville_obsolete("A", 10, 10);
            Ville_obsolete v2 = new Ville_obsolete("B", 20, 20);
            Ville_obsolete v3 = new Ville_obsolete("C", 30, 30);
            Ville_obsolete v4 = new Ville_obsolete("D", 40, 40);
            Ville_obsolete v5 = new Ville_obsolete("E", 50, 50);
            Ville_obsolete v6 = new Ville_obsolete("F", 60, 60);

            List<Ville_obsolete> lv = new List<Ville_obsolete>();
            lv.Add(v1);
            lv.Add(v2);
            lv.Add(v3);
            lv.Add(v4);
            lv.Add(v5);
            lv.Add(v6);

            // calcul du score d'un chemin 
            //Chemin ch1 = new Chemin(lv);
            //Console.WriteLine("le score global est de " + ch1.CalculScore());

            // liste des villes commancants par un caractère
            //ch1.villeCommencePar('a');

            // first gen
            Console.WriteLine("Algo first gen");
            Algorithme_obsolete algo = new Algorithme_obsolete(10, lv);
            algo.getFirstGen();
            Console.WriteLine(algo.ToString());




            Console.ReadKey();
        }
    }
}
