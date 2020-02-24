using System;
using System.Collections.Generic;
using System.Linq;
namespace Partie_Console
{
    class Generation
    {
        private List<Population> mesGenerations;
        //private int pointArret;
        public Generation()
        {
            mesGenerations = new List<Population>();
            //pointArret = 0;
        }

        public List<Population> getGenerations
        {
            get
            {
                return mesGenerations;
            }
        }

        /*
        public int getPointArret
        {
            get
            {
                return pointArret;
            }
        }
        */

        public void Play(int arret, int nombreIndividu, List<Ville> mesVilles, int nombreCross, int nombreMutation)
        {
            int pointArret = 0;
            Population firstPopulation = new Population(nombreIndividu, mesVilles);
            firstPopulation.AlgoRun(nombreCross,nombreMutation);
            mesGenerations.Add(firstPopulation);
            while (pointArret < arret)
            {
                Console.WriteLine(mesGenerations.Last().getGeneration[0].Score);
                mesGenerations.Add(mesGenerations.Last().AlgoRun(nombreCross, nombreMutation));
                if(mesGenerations.Last().getGeneration[0].Score == mesGenerations[mesGenerations.Count - 1].getGeneration[0].Score){
                    pointArret++;
                }
                else{
                    pointArret = 0;
                }
            }
           
        }
    }
}