using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_Console
{
    public class Algorithme_obsolete
    {
        private List<Ville_obsolete> mesVilles;
        private int FirstGeneration;
        private List<Chemin_obsolete> mesChemins;
        public Algorithme_obsolete(int firstGeneration, List<Ville_obsolete> mesVilles)
        {
            this.mesVilles = mesVilles;
            this.FirstGeneration = firstGeneration;
        }
        public List<Chemin_obsolete> getFirstGen()
        {
            int combinaisons = this.FirstGeneration;
            List<Chemin_obsolete> maGeneration = new List<Chemin_obsolete>();
            while (combinaisons != 0)
            {
                Chemin_obsolete chemin = new Chemin_obsolete(melangeList<Ville_obsolete>(this.mesVilles));

                var cnt = from c in maGeneration
                          where c.ToString() == chemin.ToString()
                          select c;

                if (cnt.Count() == 0)
                {
                    maGeneration.Add(chemin);
                    combinaisons--;
                }

            }
            this.mesChemins = maGeneration;
            return maGeneration;
        }
        private List<Ville> melangeList<Ville>(List<Ville> inputList)
        {
            List<Ville> randomList = new List<Ville>();
            List<Ville> inputListCopie = new List<Ville>(inputList);

            Random r = new Random();
            int randomIndex = 0;
            while (inputListCopie.Count > 0)
            {
                randomIndex = r.Next(0, inputListCopie.Count); //Choose a random object in the list
                randomList.Add(inputListCopie[randomIndex]); //add it to the new, random list
                inputListCopie.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
        public override string ToString()
        {
            string chaine = null;
            foreach (Chemin_obsolete c in this.mesChemins)
            {
                chaine += c.ToString() + "\n" + c.Score + "\n";
            }
            return chaine;
        }
    }
}
