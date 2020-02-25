using System;
using System.Collections.Generic;
using System.Linq;

namespace Partie_Console
{
    public class Population
    {
        private List<Generation> mesGenerations;

        public Population()
        {
            this.mesGenerations = new List<Generation>();
        }

        public List<Generation> GetGenerations
        {
            get
            {
                return this.mesGenerations;
            }
        }

        /// <summary>
        /// la fonction permettant lancement de la creation des generations
        /// </summary>
        /// <param name="taillePopulation"></param> taille de la population
        /// <param name="nombreIndividu"></param> nombre de chemin au debut de chaque generation
        /// <param name="mesVilles"></param>
        /// <param name="nombreCross"></param>
        /// <param name="nombreMutation"></param>
        /// <param name="nbrElite"></param>
        public void Play(int taillePopulation, int nombreIndividu, List<Ville> mesVilles, int nombreCross, int nombreMutation, int nbrElite)
        {
            int cntGeneration = 1;
            int pointArret = 0;
            Generation firstPopulation = new Generation(nombreIndividu, mesVilles , "G�n�ration" + cntGeneration);
            firstPopulation.AlgoRun(nombreCross, nombreMutation, nbrElite, "G�n�ration" + cntGeneration);
            this.mesGenerations.Add(firstPopulation);

            while (pointArret < taillePopulation - 1)
            {
                cntGeneration++;
                this.mesGenerations.Add(this.mesGenerations.Last().AlgoRun(nombreCross, nombreMutation, nbrElite, "G�n�ration" + cntGeneration));
                if (this.mesGenerations.Last().GetGeneration[0].Score == mesGenerations[this.mesGenerations.Count - 1].GetGeneration[0].Score)
                {
                    pointArret++;
                }
                else
                {
                    pointArret = 0;
                }
            }

            ResultAffichage();
        }

        public void ResultAffichage()
        {
            int i = 1;
            foreach (Generation po in this.mesGenerations)
            {
                Console.WriteLine("--------Generation " + i + "------ MOYEN: " + po.GetMoyenScoreGeneration + " M.SCORE: " + po.GetTopScoreGeneration);
                //foreach(Chemin c in po.GetGeneration){
                //    Console.WriteLine("--------Chemin------");
                //    Console.WriteLine(c + c.Score.ToString());
                //}
                i++;
            }
            Console.WriteLine("Affiche de meilleur chemin de la population");
            Console.WriteLine(GetMeilleurCheminDeLaPopulation() + " " + GetMeilleurCheminDeLaPopulation().Score.ToString());
        }

        public Chemin GetMeilleurCheminDeLaPopulation()
        {
            Chemin top = null;
            foreach (Generation gs in this.mesGenerations)
            {
                foreach (Chemin c in gs.GetGeneration)
                {
                    if (top is null)
                    {
                        top = c;
                    }
                    else
                    {
                        if (c.Score < top.Score) top = c;
                    }


                }
            }
            return top;
        }

    }
}