using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_Console
{
    public class Chemin_obsolete
    {
        private List<Ville_obsolete> listeVille; //= new List<Ville>();
        public List<Ville_obsolete> villes
        {
            get { return this.listeVille; }
            set { this.listeVille = value; }
        }
        public Chemin_obsolete(List<Ville_obsolete> liste)
        {
            this.listeVille = liste;
        }
        public double Score
        {
            get
            {
                return CalculScore();
            }

        }
        public double CalculScore()
        {
            double result = 0;
            // cas ou on a qu'une seule ville
            if (this.listeVille.Count == 1)
            {
                return 0;
            }

            for (int i = 0; i < this.listeVille.Count - 1; i++)
            {
                result += calculCheminEntreDeuxVille(this.listeVille[i], this.listeVille[i + 1]);
            }

            return result;
        }
        public double calculCheminEntreDeuxVille(Ville_obsolete v1, Ville_obsolete v2)
        {
            double x = v1.XVille - v2.XVille;
            double y = v1.YVille - v2.YVille;
            double result = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            return result;

        }

        /// utiliser linq pour : 
        /// toute les villes qui commencent par a ou A
        /// recuperer les 10 meilleir chemin d'une liste de chemin

        public IEnumerable<Ville_obsolete> villeCommencePar(char c)
        {
            IEnumerable<Ville_obsolete> listeVilleLINQ = from v in this.listeVille
                                                where v.NVile.ToLower()[0] == c.ToString().ToLower()[0]
                                                select v;
            foreach (Ville_obsolete v in listeVilleLINQ)
            {
                Console.WriteLine(v.NVile);
            }
            return listeVilleLINQ;
        }
        public override string ToString()
        {
            string chaine = null;
            foreach (Ville_obsolete v in this.listeVille)
            {
                chaine += v.NVile +"-";
            }
            chaine = chaine.Substring(0, chaine.Length - 1); // enlever dernier caractere '-' de la chaine
            return chaine;
        }
    }
}
