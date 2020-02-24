using System;
using System.Collections.Generic;
using System.Threading;

namespace Partie_Console
{
     class Program
    {
        static void Main(string[] args)
        {
            Ville v1 = new Ville("A", 22F, 4F);
            Ville v2 = new Ville("B", 3110F, 70F);
            Ville v3 = new Ville("C", 6.0F, 1.25F);
            Ville v4 = new Ville("D", 14.21F, 14.23F);
            Ville v5 = new Ville("E", 211.21F, 51.23F);
            Ville v6 = new Ville("F",140.21F,14.23F);
            Ville v7 = new Ville("G",11.21F,51.23F);

            List<Ville> mesVilles = new List<Ville>();
            mesVilles.Add(v1);
            mesVilles.Add(v2);
            mesVilles.Add(v3);
            mesVilles.Add(v4);
            mesVilles.Add(v5);
            mesVilles.Add(v6);
            mesVilles.Add(v7);

            Parametrage param = new Parametrage(30, 10, 30, 30, 2);
            Generation g = new Generation(10, mesVilles);
            Population p = new Population();

            // executer la fonction principale en thread
            Thread thread = new Thread(new ThreadStart(() => p.Play(param.Taille_population, param.NbrCheminInGeneration, mesVilles, param.Crossover, param.Mutation, param.Elite) ));
            thread.Start();


            Console.ReadKey();
        }
    }
}
