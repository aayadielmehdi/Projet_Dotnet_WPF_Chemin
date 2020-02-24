using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Partie_Console;


namespace ApplicationWPF
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int mutation;
        public int Mutation
        {
            get { return this.mutation; }
            set
            {
                if (this.mutation != value)
                {
                    this.mutation = value;
                    this.NotifyPropertyChanged("Mutation");
                }
            }
        }

        private int crossover;
        public int Crossover
        {
            get { return this.crossover; }
            set
            {
                if (this.crossover != value)
                {
                    this.crossover = value;
                    this.NotifyPropertyChanged("Crossover");
                }
            }
        }

        private int elite;
        public int Elite
        {
            get { return this.elite; }
            set
            {
                if (this.elite != value)
                {
                    this.elite = value;
                    this.NotifyPropertyChanged("Elite");
                }
            }
        }

        private int taille_population;
        public int Taille_population
        {
            get { return this.taille_population; }
            set
            {
                if (this.taille_population != value)
                {
                    this.taille_population = value;
                    this.NotifyPropertyChanged("Taille_population");
                }
            }
        }

        private Dictionary<Ville_obsolete,Ellipse> Dictionnaire_ville_ellipse = new Dictionary<Ville_obsolete, Ellipse>();

        static int ascii = 65; // valeur pour creer le nom des villes
        private ObservableCollection<Ville_obsolete> villes_choisie = new ObservableCollection<Ville_obsolete>();
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        
        public ObservableCollection<Ville_obsolete> Liste_Ville
        {
            get
            {
                return this.MaListe_avec_critere();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private void Choix_ville(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas_carte);
            double x = p.X;
            double y = p.Y;
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 10;
            ellipse.Width = 10;
            //SolidColorBrush rouge = new SolidColorBrush();
            //rouge.Color = Colors.Red;
            ellipse.Fill = Brushes.Red;
            Canvas.SetTop(ellipse, y);
            Canvas.SetLeft(ellipse, x);
            canvas_carte.Children.Add(ellipse);

            var dialog = new Input_NomVille();
            string name_ville = null;
            if (dialog.ShowDialog() == true)
            {
                name_ville = dialog.ResponseText;
            }
            if (name_ville == null || name_ville == "")
            {
                MessageBox.Show("Un nom de ville sera choisi par default");
                name_ville = ((char)ascii).ToString();
                ascii++;
            }
            
            // faut faire peut être apres la verification si le nom de la ville existe déjà
            Ville_obsolete v = new Ville_obsolete(name_ville, x, y);
            villes_choisie.Add(v);

            // ajout de la ville dans dictionnaire afin de supprimer le ellipse a la suppression de la ville
            Dictionnaire_ville_ellipse.Add(v, ellipse);


            NotifyPropertyChanged("Liste_Ville");
        }
        private void Supprimer_ville(object sender, MouseButtonEventArgs e)
        {            
            if (MessageBox.Show("Etes vous sur de vouloir supprimer cette ville", "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Ville_obsolete currentVille = (e.Source as DataGrid).CurrentItem as Ville_obsolete;
                
                // supprimer l'ellipse du canvas aussi 
                canvas_carte.Children.Remove(this.Dictionnaire_ville_ellipse[currentVille]);
                
                villes_choisie.Remove(currentVille);
                NotifyPropertyChanged("Liste_Ville");
            }
        }
        public ObservableCollection<Ville_obsolete> MaListe_avec_critere()
        {
            if (name_critere.Text == "")
            {
                return this.villes_choisie;
            }
            else
            {
                var result = from v in this.villes_choisie
                             where v.NVile.ToString().ToUpper() == name_critere.Text.ToUpper()
                             select v;

                return new ObservableCollection<Ville_obsolete>(result);
            }
        }
        private void Recherche(object sender, RoutedEventArgs e)
        {
            NotifyPropertyChanged("Liste_Ville");
        }
    
    }


    // a la fin du travail faire des liens entre les villes avec des fleches pour connaitre le plus court chemin.
    // n'oublie pas threads
    // ajouter un parametre au niveau pour choisir le nombre  de chemin en chaque generation
}
