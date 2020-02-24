using System;
using System.Text;

namespace Partie_Console
{
    class Ville
    {
        private string Nom;
        private float X;
        private float Y;

        public Ville(string nom, float x, float y)
        {
            this.Nom = nom;
            this.X = x;
            this.Y = y;
        }

        public string NomVille {
            get {
                return Nom;
            }
            set {
                Nom = value;
            }
        }

         public float XVille {
            get {
                return X;
            }
            set {
                X = value;
            }
        }

         public float YVille {
            get {
                return Y;
            }
            set {
                Y = value;
            }
        }

        public override String ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append(NomVille+"("+XVille+";"+YVille+")");
            return sb.ToString();
        }

    }
}