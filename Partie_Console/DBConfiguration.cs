using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.ObjectModel;


namespace Partie_Console
{
    public class DBConfiguration
    {

        public static DBConfiguration instance = null;
        private static SQLiteConnection con = null;

        private DBConfiguration()
        {


            if (con == null)
            {
                con = new SQLiteConnection("MaBaseSqLite");

                if (con.GetTableInfo("Ville").Count == 0)
                {
                    con.CreateTable<Ville>();
                }
                if (con.GetTableInfo("Parametrage").Count == 0)
                {
                    con.CreateTable<Parametrage>();
                }

            }
            instance = this;
        }

        public static DBConfiguration GetInstance()
        {
            if (instance == null)
            {
                new DBConfiguration();
            }
            return instance;
        }


        public void SaveVille(Ville v)
        {
            con.Insert(v);
        }

        public ObservableCollection<Ville> GetVilles()
        {
            return new ObservableCollection<Ville>(con.Table<Ville>().ToList());
        }

        public void DeleteVille(Ville v)
        {
            con.Delete<Ville>(v.NomVille);
        }

        public void DeleteAllVille()
        {
            con.DeleteAll<Ville>();
        }



        public void SaveParametrage(Parametrage p)
        {
            //con.DeleteAll<Parametrage>();
            //con.Insert(p);
            if (this.VerificationExistanceParametrage() == false)
                con.Insert(p);
            else
                con.Update(p);

        }

        public Parametrage GetParametrage()
        {
            if (con.Table<Parametrage>().Count() != 0)
            {
                return con.Table<Parametrage>().First();
            }
            return new Parametrage();
        }

        public bool VerificationExistanceParametrage()
        {
            if (con.Table<Parametrage>().Count() != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void KillConnection()
        {
            con.Close();
            instance = null;
        }
    }
}
