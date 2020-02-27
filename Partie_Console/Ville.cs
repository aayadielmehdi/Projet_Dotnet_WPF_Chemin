using System;
using System.Text;
using SQLite;

namespace Partie_Console
{
    [Table("Ville")]
    public class Ville
    {
        private string Nom;
        private float X;
        private float Y;

        public Ville()
        {

        }
        public Ville(string nom, float x, float y)
        {
            this.Nom = nom;
            this.X = x;
            this.Y = y;
        }

        [Column("NomVille"), PrimaryKey] // le nom de la ville est ma cle primaire aussi 
        public string NomVille
        {
            get
            {
                return Nom;
            }
            set
            {
                Nom = value;
            }
        }

        [Column("XVille")]
        public float XVille
        {
            get
            {
                return X;
            }
            set
            {
                X = value;
            }
        }

        [Column("YVille")]
        public float YVille
        {
            get
            {
                return Y;
            }
            set
            {
                Y = value;
            }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(NomVille + "(" + XVille + ";" + YVille + ")");
            return sb.ToString();
        }

    }
}