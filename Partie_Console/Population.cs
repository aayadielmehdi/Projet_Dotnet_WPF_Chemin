using System;
using System.Collections.Generic;
using System.Linq;
namespace Partie_Console
{
    class Population
    {
        private List<Ville> mesVilles;
        private int FirstGeneration;

        //private List<Chemin> mesElites;
        private List<Chemin> maGeneration;
        private List<Chemin> maPopulation;

        public Population(int firstGeneration, List<Chemin> mesChemins)
        {
            this.FirstGeneration = firstGeneration;
            this.maGeneration = new List<Chemin>(mesChemins);
            //this.mesElites = new List<Chemin>();
            this.maPopulation = new List<Chemin>();
            //GetFirstGen();
        }
        public Population(int firstGeneration, List<Ville> mesVilles)
        {
            this.mesVilles = mesVilles;
            this.FirstGeneration = firstGeneration;
            this.maGeneration = new List<Chemin>();
            //this.mesElites = new List<Chemin>();
            this.maPopulation = new List<Chemin>();
            GetFirstGen();
        }

        public List<Chemin> getPopulation
        {
            get
            {
                return maPopulation;
            }
        }

        public List<Chemin> getGeneration
        {
            get
            {
                return maGeneration;
            }
            set
            {
                maGeneration = value;
            }
        }

        /*
        public List<Chemin> getElites
        {
            get
            {
                return mesElites;
            }
        }
        */

        public int nombreIndividu
        {
            get
            {
                return FirstGeneration;
            }
            set
            {
                FirstGeneration = value;
            }
        }

        // Générer la première génration de population
        public void GetFirstGen()
        {
            int combinaisons = FirstGeneration;
            //List<Chemin> chemins = new List<Chemin>();
            while (combinaisons != 0)
            {
                //Chemin chemin = new Chemin(melangeList<Ville>(mesVilles));
                Chemin chemin = new Chemin(mesVilles.OrderBy(a => Guid.NewGuid()).ToList());
                if (comparerChemin(chemin, maGeneration) == 0)
                {
                    maGeneration.Add(chemin);
                    //maPopulation.Add(chemin);
                    combinaisons--;
                }

            }
        }

        // Faire un mélange de la génération à l'aide d'un CrossOver
        public void XOver(int nombreCross)
        {
            for (int index = 0; index < nombreCross; index++)
            {
                int taille = maGeneration[0].MesVilles.Count;
                int partition1 = taille / 2;
                int reste = taille - partition1;
                List<Ville> CrossVille = new List<Ville>();
                List<Chemin> CrossChemin = new List<Chemin>();
                List<Ville> portionDroite = new List<Ville>();
                Random r = new Random();
                int nombreChemin = 2;
                int randomIndex1 = r.Next(0, maGeneration.Count);
                Chemin chemin1 = maGeneration[randomIndex1];
                Chemin chemin2;
                CrossChemin.Add(chemin1);
                while (nombreChemin != 1)
                {
                    int randomIndex2 = r.Next(0, maGeneration.Count);
                    if (randomIndex2 != randomIndex1)
                    {
                        chemin2 = maGeneration[randomIndex2];
                        CrossChemin.Add(chemin2);
                        nombreChemin--;
                    }
                }
                for (int i = 0; i < partition1; i++)
                {
                    CrossVille.Add(CrossChemin[0].MesVilles[i]);
                    //resultVilles.Add(CrossChemin[0].MesVilles[i]);
                }
                for (int i = partition1; i < taille; i++)
                {
                    CrossVille.Add(CrossChemin[1].MesVilles[i]);
                    portionDroite.Add(CrossChemin[0].MesVilles[i]);
                }
                List<Ville> VillesDoublons = checkDoublon(CrossVille);

                List<Ville> resultVilles = CheminSansDoublon(VillesDoublons, portionDroite, CrossVille);
                Chemin resultChemin = new Chemin(resultVilles);

                if (comparerChemin(resultChemin, maPopulation) == 0)
                {
                    maPopulation.Add(resultChemin);
                }

            }
        }

        // Mutation de quelques gènes de la population pour en produire de nouvelles
        public void Mutation(int nombreMutation)
        {
            int i = 0 ;
            while(i < nombreMutation)
            {
                Random r = new Random();
                int randomIndex = r.Next(0, maGeneration.Count);
                Chemin chemin = maGeneration[randomIndex];
                List<Ville> mesVilles = new List<Ville>(chemin.MesVilles);
                int nombreVille = mesVilles.Count;
                int indexVille1 = r.Next(0, nombreVille);
                int indexVille2 = r.Next(0, nombreVille);
                while (indexVille2 == indexVille1)
                {
                    indexVille2 = r.Next(nombreVille);
                }
                List<Ville> resultVilles = Swap(mesVilles, indexVille1, indexVille2);
                Chemin cheminFinal = new Chemin(resultVilles);
                if (comparerChemin(cheminFinal, maPopulation) == 0)
                {
                    maPopulation.Add(cheminFinal);
                    i++;
                }
            }
        }

        public void Elite()
        {
            int nombreElite = maGeneration.Count / 5;
            var elite = from e in maGeneration orderby e.Score select e;
            //int i = 0;
            for (int i = 0; i < nombreElite; i++)
            {
                if (comparerChemin(elite.ElementAt(i), maPopulation) == 0)
                {
                    maPopulation.Add(elite.ElementAt(i));
                    //i++;
                }
                //maPopulation.Add(elite.ElementAt(i));
                //mesElites.Add(elite.ElementAt(i));
            }
        }

        public List<Chemin> BestChemin()
        {
            List<Chemin> bestChemins = new List<Chemin>();
            var elite = from e in maPopulation orderby e.Score select e;
            for (int i = 0; i < FirstGeneration; i++)
            {
                bestChemins.Add(elite.ElementAt(i));
            }
            /*
            foreach(Chemin c in bestChemins){
                Console.WriteLine(c);
            }
            */
            return bestChemins;
        }

        public Population AlgoRun(int nombreCross, int nombreMutation)
        {
            //GetFirstGen();
            XOver(nombreCross);
            Mutation(nombreMutation);
            Elite();
            Population p = new Population(FirstGeneration, BestChemin());
            //p.nombreIndividu = FirstGeneration;

            return p;
        }

        public int comparerChemin(Chemin chemin, List<Chemin> maListe)
        {
            var cnt = from c in maListe
                      where c.ToString() == chemin.ToString()
                      select c;
            return cnt.Count();
        }

        public int comparerVille(Ville ville, List<Ville> maListe)
        {
            var cnt = from v in maListe
                      where v.ToString() == ville.ToString()
                      select v;
            return cnt.Count();
        }

        public List<Ville> checkDoublon(List<Ville> ListeATester)
        {
            List<Ville> maListe = new List<Ville>();
            foreach (Ville v in ListeATester)
            {
                if (comparerVille(v, ListeATester) == 2)
                {
                    maListe.Add(v);
                }
            }
            return maListe;
        }

        public List<Ville> CheminSansDoublon(List<Ville> listeDoublon, List<Ville> portionDroite, List<Ville> ListeComplete)
        {

            foreach (Ville v in listeDoublon)
            {
                int doublon = ListeComplete.LastIndexOf(v);
                foreach (Ville pd in portionDroite)
                {
                    if (comparerVille(pd, ListeComplete) == 0)
                    {
                        ListeComplete[doublon] = pd;
                        break;
                    }
                }
            }

            return ListeComplete;
        }

        public List<Ville> Swap(List<Ville> list, int indexA, int indexB)
        {
            Ville tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }
    }
}