using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Partie_Console
{
    public class Parametrage 
    {        
        /// <summary>
        /// specification des parametrages de l'application
        /// </summary>
        /// <param name="_taille"></param> taille de la population c'est le nombre de generation qu'on va aboutir
        /// <param name="_nbchemin"></param> nbr de chemin au debut de chaque generation
        /// <param name="_xover"></param>
        /// <param name="_mutation"></param>
        /// <param name="_elite"></param>
        public Parametrage(int _taille , int _nbchemin , int _xover, int _mutation, int _elite)
        {
            this.mutation = _mutation;
            this.nbrCheminInGeneration = _nbchemin;
            this.taille_population = _taille;
            this.crossover = _xover;
            this.elite = _elite;
        }

        private int mutation;

        public int Mutation
        {
            get { return this.mutation; }
            set
            {
                if (this.mutation != value)
                {
                    this.mutation = value;
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
                }
            }
        }

        private int nbrCheminInGeneration;
        public int NbrCheminInGeneration
        {
            get { return this.nbrCheminInGeneration; }
            set
            {
                if (this.nbrCheminInGeneration != value)
                {
                    this.nbrCheminInGeneration = value;
                }
            }
        }
    }
}
