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

        private int nbrCheminParGenerationdepart;

        public int NbrCheminParGenerationDepart
        {
            get { return this.nbrCheminParGenerationdepart; }
            set{
                if (this.nbrCheminParGenerationdepart != value)
                {
                    this.nbrCheminParGenerationdepart = value;
                    this.NotifyPropertyChanged("Taille_population");
                }

            }
        }

        private Dictionary<Ville,Ellipse> Dictionnaire_ville_ellipse = new Dictionary<Ville, Ellipse>();

        static int ascii = 1; // valeur pour creer le nom des villes

        private ObservableCollection<Ville> villes_choisie = new ObservableCollection<Ville>();

        private ObservableCollection<Generation> mes_generations;

        public ObservableCollection<Generation> MesGenerations
        {
            get
            {
                return this.mes_generations;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        
        public ObservableCollection<Ville> Liste_Ville
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
                name_ville = "V" + (ascii).ToString();
                ascii++;
            }
            
            // faut faire peut être apres la verification si le nom de la ville existe déjà
            Ville v = new Ville(name_ville, (float)x, (float)y);
            villes_choisie.Add(v);

            // ajout de la ville dans dictionnaire afin de supprimer le ellipse a la suppression de la ville
            Dictionnaire_ville_ellipse.Add(v, ellipse);


            NotifyPropertyChanged("Liste_Ville");
        }

        private void Supprimer_ville(object sender, MouseButtonEventArgs e)
        {            
            if (MessageBox.Show("Etes vous sur de vouloir supprimer cette ville", "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Ville currentVille = (e.Source as DataGrid).CurrentItem as Ville;
                
                // supprimer l'ellipse du canvas aussi 
                canvas_carte.Children.Remove(this.Dictionnaire_ville_ellipse[currentVille]);
                
                villes_choisie.Remove(currentVille);
                NotifyPropertyChanged("Liste_Ville");
            }
        }

        public ObservableCollection<Ville> MaListe_avec_critere()
        {
            if (name_critere.Text == "")
            {
                return this.villes_choisie;
            }
            else
            {
                var result = from v in this.villes_choisie
                             where v.NomVille.ToString().ToUpper() == name_critere.Text.ToUpper()
                             select v;

                return new ObservableCollection<Ville>(result);
            }
        }

        private void Recherche(object sender, RoutedEventArgs e)
        {
            NotifyPropertyChanged("Liste_Ville");
        }
               
        private void DessinerChemin(Chemin c)
        {
            for (int i = 0; i < c.MesVilles.Count() - 1; i++)
            {
                Ville v1 = c.MesVilles[i];
                Ville v2 = c.MesVilles[i + 1];

                var uneLigne = new Line
                {
                    X1 = v1.XVille,
                    Y1 = v1.YVille,
                    X2 = v2.XVille,
                    Y2 = v2.YVille,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };

                canvas_carte.Children.Add(uneLigne);
                
            }
        }

        private void GriserTT()
        {
            canvas_carte.IsEnabled = false;
            grid_ville.IsEnabled = false;
            grid_seconde.IsEnabled = false;
            panel_parametrage.IsEnabled = false;
            btn_run.IsEnabled = false;
        }

        private void Reset()
        {
            canvas_carte.IsEnabled = true;
            grid_ville.IsEnabled = true;
            grid_seconde.IsEnabled = true;
            panel_parametrage.IsEnabled = true;
            btn_run.IsEnabled = true;

            this.mes_generations.Clear();

            this.NotifyPropertyChanged("MesGenerations");

            txt_meilleur_chemin.Text = "Meilleur Chemin ??";

            // suppression des lignes 
            foreach (object o in canvas_carte.Children)
            {
                if (o.GetType() == typeof(Line))
                {
                    canvas_carte.Children.Remove((Line) o);
                }
            }
            tab_global.SelectedIndex = 0;

        }

        private void RunProgramme(object sender, RoutedEventArgs e)
        {
            
            if (Verification() == true)
            {
                if (MessageBox.Show("Vos paramétrage :\nNb de chemin: " + this.nbrCheminParGenerationdepart + "\nTaille Population: " + this.taille_population +
                    "\nElite: " + this.elite +
                    "\nMutation: " + this.mutation +
                    "\nXOver: " + this.crossover +
                    "\nContinuer ou pas ?", "Information", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return; 
                    
                Mouse.OverrideCursor = Cursors.Wait;

                Population pop = new Population();

                pop.Play(this.taille_population, this.nbrCheminParGenerationdepart, new List<Ville>(this.villes_choisie), this.crossover, this.mutation, this.elite);

                this.mes_generations = new ObservableCollection<Generation>(pop.GetGenerations);

                this.DessinerChemin(pop.GetMeilleurCheminDeLaPopulation());

                this.txt_meilleur_chemin.Text = "Le Meilleur Chemin est : " + pop.GetMeilleurCheminDeLaPopulation().ToString() + " avec un score de : " + pop.GetMeilleurCheminDeLaPopulation().Score.ToString();

                this.GriserTT();

                Mouse.OverrideCursor = null;

                this.NotifyPropertyChanged("MesGenerations");
            }
        }

        private bool Verification()
        {
            //verification des paramettre 
            if (this.villes_choisie.Count == 0)
            {
                MessageBox.Show("Ya plus de ville dans votre liste !! \nVeuillez choisir des villes :)", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 0;
                return false;
            }
            else if (this.elite > this.nbrCheminParGenerationdepart)
            {
                MessageBox.Show("Le parametre Elite est sup au nombre de chemin par génération", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 3;
                txt_elite.Focus();
                return false;
            }
            else if (this.nbrCheminParGenerationdepart == 0 )
            {
                MessageBox.Show("Le parametre Nbr chemin Start n'est Signalé", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 3;
                txt_nbrchemin.Focus();
                return false;
            }
            return true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }
    }

}
