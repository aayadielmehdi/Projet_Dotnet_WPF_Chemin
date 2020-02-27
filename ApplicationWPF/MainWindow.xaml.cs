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
        private DBConfiguration monInstancePrincipale;

        public MainWindow()
        {
            InitializeComponent();

            // la connection a sqlite
            monInstancePrincipale = DBConfiguration.GetInstance();

            // get parametrage application 
            this.paramApplication = monInstancePrincipale.GetParametrage();

            // dessiner les elipse en cas d'existance ville au lancement du programme
            this.DessinerEllipseVilles();

            this.DataContext = this;

        }

        private Parametrage paramApplication;

        public Parametrage MesParams
        {
            get
            {
                this.monInstancePrincipale.SaveParametrage(this.paramApplication);
                return this.paramApplication;
            }

            set
            {
                this.paramApplication = value;
                this.monInstancePrincipale.SaveParametrage(this.paramApplication);
                this.NotifyPropertyChanged("MesParams");
            }
        }

        // dictionnaire pour mettre ville et ellipse correspondant ( je sais pas pourquoi il marche pas a la suppresion de ville , j'ai fait autrement en verifiant la ville.nom )
        private IDictionary<Ville, Ellipse> Dictionnaire_ville_ellipse = new Dictionary<Ville, Ellipse>();

        private ObservableCollection<Generation> mes_generations = new ObservableCollection<Generation>();

        public ObservableCollection<Generation> MesGenerations
        {
            get
            {
                //return this.mes_generations;
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
                // return une liste , car des fois je fais la recherche selon des critère.
                return this.MaListe_avec_critere();
            }
        }

        private void Choix_ville(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas_carte);
            double x = p.X;
            double y = p.Y;

            var dialog = new Input_NomVille();
            string name_ville = null;
            if (dialog.ShowDialog() == true)
            {
                name_ville = dialog.ResponseText;
            }
            if (name_ville == null || name_ville == "")
            {
                MessageBox.Show("Un nom de ville sera choisi par default");
                name_ville = "V" + (this.paramApplication.ConteurVille).ToString();
                this.paramApplication.ConteurVille++;
                this.monInstancePrincipale.SaveParametrage(this.paramApplication);
            }


            if (ExistanceNameVille(name_ville))
            {
                MessageBox.Show("Ce nom de ville existe déjà !! ", "Attention", MessageBoxButton.OK);
            }
            else
            {
                // faut faire peut être apres la verification si le nom de la ville existe déjà
                Ville v = new Ville(name_ville, (float)x, (float)y);

                this.DessinerEllipse(v);

                //villes_choisie.Add(v);                
                this.monInstancePrincipale.SaveVille(v);

                NotifyPropertyChanged("Liste_Ville");
            }

        }

        private void Supprimer_ville(object sender, MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Etes vous sur de vouloir supprimer cette ville", "Information", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Ville currentVille = (e.Source as DataGrid).CurrentItem as Ville;


                // supprimer l'ellipse du canvas aussi 

                // la notion dictionnaire ne marche pas (exeption key not found alors qu'il existe) ??
                //canvas_carte.Children.Remove(this.Dictionnaire_ville_ellipse[currentVille]);

                foreach (KeyValuePair<Ville, Ellipse> item in this.Dictionnaire_ville_ellipse)
                {
                    if ((item.Key.NomVille).Equals(currentVille.NomVille))              // (item.key).Equals(currentVille) est tjr en false je comprends pas la raison
                    {
                        canvas_carte.Children.Remove(item.Value);
                    }
                }


                this.monInstancePrincipale.DeleteVille(currentVille);

                NotifyPropertyChanged("Liste_Ville");
            }
        }

        public ObservableCollection<Ville> MaListe_avec_critere()
        {
            if (name_critere.Text == "")
            {
                return this.monInstancePrincipale.GetVilles();
            }
            else
            {
                // recherche de ville selon le nom saisie 
                var result = from v in this.monInstancePrincipale.GetVilles()
                             where v.NomVille.ToString().ToUpper() == name_critere.Text.ToUpper()
                             select v;

                return new ObservableCollection<Ville>(result);
            }
        }

        private void Recherche(object sender, RoutedEventArgs e)
        {
            NotifyPropertyChanged("Liste_Ville");
        }

        // dessiner chemin apres la fin du traitement pour montrer le plus petit chemin
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

        /// <summary>
        /// griser application en temps du run 
        /// </summary>
        private void GriserTT()
        {
            canvas_carte.IsEnabled = false;
            grid_ville.IsEnabled = false;
            grid_seconde.IsEnabled = false;
            panel_parametrage.IsEnabled = false;
            btn_run.IsEnabled = false;
            menuresetdb.IsEnabled = false;
        }

        /// <summary>
        /// donner l'acces au utilisateur pour reutiliser l'application
        /// </summary>
        private void Reset()
        {
            canvas_carte.IsEnabled = true;
            grid_ville.IsEnabled = true;
            grid_seconde.IsEnabled = true;
            panel_parametrage.IsEnabled = true;
            btn_run.IsEnabled = true;
            menuresetdb.IsEnabled = true;


            if (this.mes_generations != null) this.mes_generations.Clear();

            this.NotifyPropertyChanged("MesGenerations");

            txt_meilleur_chemin.Text = "Meilleur Chemin ??";

            // suppression des lignes 

            for (int i = 0; i < canvas_carte.Children.Count; i++)
            {
                if (canvas_carte.Children[i].GetType().Equals(typeof(Line)))
                {
                    canvas_carte.Children.Remove((Line)canvas_carte.Children[i]);
                    i--;
                }
            }

            tab_global.SelectedIndex = 0;

        }

        /// <summary>
        /// lancement du programme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunProgramme(object sender, RoutedEventArgs e)
        {

            if (Verification() == true)
            {
                if (MessageBox.Show("Vos paramétrage :\nNb de chemin: " + this.paramApplication.NbrCheminInGeneration + "\nTaille Population: " + this.paramApplication.Taille_population +
                    "\nElite: " + this.paramApplication.Elite +
                    "\nMutation: " + this.paramApplication.Mutation +
                    "\nXOver: " + this.paramApplication.Crossover +
                    "\nContinuer ou pas ?", "Information", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;

                Mouse.OverrideCursor = Cursors.Wait;

                Population pop = new Population();

                pop.Play(this.paramApplication.Taille_population, this.paramApplication.NbrCheminInGeneration, new List<Ville>(this.monInstancePrincipale.GetVilles()), this.paramApplication.Crossover, this.paramApplication.Mutation, this.paramApplication.Elite);

                this.mes_generations = new ObservableCollection<Generation>(pop.GetGenerations);

                this.DessinerChemin(pop.GetMeilleurCheminDeLaPopulation());

                this.txt_meilleur_chemin.Text = "Le Meilleur Chemin est : " + pop.GetMeilleurCheminDeLaPopulation().ToString() + " avec un score de : " + pop.GetMeilleurCheminDeLaPopulation().Score.ToString();

                this.GriserTT();

                Mouse.OverrideCursor = null;

                this.NotifyPropertyChanged("MesGenerations");
            }

        }

        //toute verification avant lancemment du programme
        private bool Verification()
        {
            //verification des paramettre 
            if (this.monInstancePrincipale.GetVilles().Count == 0)
            {
                MessageBox.Show("Ya plus de ville dans votre liste !! \nVeuillez choisir des villes :)", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 0;
                return false;
            }
            else if (this.paramApplication.Elite > this.paramApplication.NbrCheminInGeneration)
            {
                MessageBox.Show("Le parametre Elite est sup au nombre de chemin par génération", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 3;
                txt_elite.Focus();
                return false;
            }
            else if (this.paramApplication.NbrCheminInGeneration == 0)
            {
                MessageBox.Show("Le parametre Nbr chemin Start n'est Signalé", "Vérification", MessageBoxButton.OK);
                tab_global.SelectedIndex = 3;
                txt_nbrchemin.Focus();
                return false;
            }

            // faire la verification des parametres saisie pour ne pas cause des boucles infini 
            // nbrchemin ! > nbrcross + nbrmutation + elite
            // c'est importante

            return true;
        }

        private void ResetRun(object sender, RoutedEventArgs e)
        {
            this.Reset();
        }

        /// <summary>
        /// reset vider liste ville de la base
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetDB(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Etes vous sur de vouloir Reset Programme ?", "Vérification", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // supprimer ville 
                this.Dictionnaire_ville_ellipse.Clear();
                this.monInstancePrincipale.DeleteAllVille();
                NotifyPropertyChanged("Liste_Ville");

                // supprimer children du canvsa
                ViderCanvas();

                // mettre parametrage de l'application en reset
                this.paramApplication.NbrCheminInGeneration = 0;
                this.paramApplication.Elite = 0;
                this.paramApplication.Mutation = 0;
                this.paramApplication.Crossover = 0;
                this.paramApplication.Taille_population = 0;
                this.paramApplication.ConteurVille = 1;
                this.monInstancePrincipale.SaveParametrage(this.paramApplication);
                this.NotifyPropertyChanged("MesParams");

                tab_global.SelectedIndex = 0;

                MessageBox.Show("Reset programme est réussi", "Information", MessageBoxButton.OK);
            }
        }

        // vérification des données saisie au niveau des parametrages
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }

        // verificaiton existance du name ville ( cle primaire pas de doublons )
        private bool ExistanceNameVille(string vll)
        {
            var result = from v in this.monInstancePrincipale.GetVilles()
                         where v.NomVille.ToString().ToUpper() == vll.ToUpper()
                         select v;

            if (result.Count() != 0)
            {
                return true;
            }
            return false;
        }

        private void DessinerEllipse(Ville v)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 10;
            ellipse.Width = 10;
            //SolidColorBrush rouge = new SolidColorBrush();
            //rouge.Color = Colors.Red;
            ellipse.Fill = Brushes.Red;
            Canvas.SetTop(ellipse, v.YVille - 5);
            Canvas.SetLeft(ellipse, v.XVille - 5);
            canvas_carte.Children.Add(ellipse);
            // ajout de la ville dans dictionnaire afin de supprimer le ellipse a la suppression de la ville
            Dictionnaire_ville_ellipse.Add(v, ellipse);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            monInstancePrincipale.KillConnection();
        }

        // vider canvas apres suppression de tout les villes 
        private void ViderCanvas()
        {
            // supprimer les ellipse et les chemin en cas d'existance
            for (int i = 0; i < canvas_carte.Children.Count; i++)
            {
                if (canvas_carte.Children[i].GetType().Equals(typeof(Image)) == false)
                {
                    canvas_carte.Children.Remove(canvas_carte.Children[i]);

                    i--;
                }
            }
        }

        // dessiner ellipse des villes au lancement du programme
        private void DessinerEllipseVilles()
        {
            foreach (Ville v in this.monInstancePrincipale.GetVilles())
            {
                this.DessinerEllipse(v);
            }
        }

    }

}
