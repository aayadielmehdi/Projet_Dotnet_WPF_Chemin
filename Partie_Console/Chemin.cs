using System;
using System.Text;
using System.Collections.Generic;

namespace Partie_Console
{
    class Chemin
    {
        private List<Ville> lesVilles;

        public Chemin(List<Ville> lesVilles)
        {
            this.lesVilles = lesVilles;
        }

        public List<Ville> MesVilles{
            get {
                return lesVilles;
            }
            set {
                lesVilles = value;
            }
        }
        public double Score
        {
            get
            {
                int taille = lesVilles.Count;
                double score = 0F;
                for (int i = 0; i < taille - 1; i++)
                {
                    Ville v1 = lesVilles[i];
                    Ville v2 = lesVilles[i + 1];
                    double x = Math.Abs(v1.XVille - v2.XVille);
                    double y = Math.Abs(v1.YVille - v2.YVille);
                    //double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

                    double distance = Math.Sqrt(Math.Pow(v1.XVille - v2.XVille, 2) + Math.Pow(v1.YVille - v2.YVille, 2));
                    score += distance;
                }

                return score;
            }

        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Ville v in lesVilles){
                sb.Append(v.NomVille);
                sb.Append("-");
            }
            return sb.ToString();
        }

    }
}