using System;
using System.Collections.Generic;

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

            //Chemin chemin1 = new Chemin(mesVilles);

            Population p = new Population(10, mesVilles);
            Generation g = new Generation();
            g.Play(30,10,mesVilles,30,30);
            //p.AlgoRun(30,30);
            //p.GetFirstGen();
            //p.XOver(30);
            //p.Mutation(30);
            //p.Elite();

            /*
            Population p2 = p.AlgoRun(5, 5);
            Population p3 = p2.AlgoRun(5, 5);
            Population p4 = p3.AlgoRun(5, 5);
            */
            Console.WriteLine(g.getGenerations.Count);
            int i = 1;
            foreach(Population po in g.getGenerations){
                Console.WriteLine("--------Generation "+i+"------");
                foreach(Chemin c in po.getGeneration){
                    Console.WriteLine("--------Chemin------");
                    Console.WriteLine(c);
                }
                i++;
            }
            Console.WriteLine(p.getGeneration.Count);
            //Console.WriteLine(p.getElites.Count);


            /*
            Console.WriteLine("--- P1----");
            foreach (Chemin c in p.getPopulation)
            {
                Console.WriteLine(c.ToString());
                //Console.WriteLine(c.Score);
            }
            
            Console.WriteLine("--- P2----");
            foreach (Chemin c in p2.getGeneration)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("--- P3----");
            foreach (Chemin c in p3.getGeneration)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("--- P4----");
            foreach (Chemin c in p4.getGeneration)
            {
                Console.WriteLine(c);
            }
            */

            Console.ReadKey();
        }
    }
}
