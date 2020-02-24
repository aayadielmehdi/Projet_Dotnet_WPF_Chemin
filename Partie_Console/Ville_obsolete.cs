using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_Console
{
    public class Ville_obsolete
    {
        private String nom;
        private double X;
        private double Y;
        public Ville_obsolete(String _n, double _x, double _y)
        {
            this.nom = _n;
            this.X = _x;
            this.Y = _y;
        }
        public String NVile
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public double XVille
        {
            get { return this.X; }
            set { this.X = value; }
        }
        public double YVille
        {
            get { return this.Y; }
            set { this.Y = value; }
        }
    }
}
