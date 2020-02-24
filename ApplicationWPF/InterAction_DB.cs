using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationWPF
{
    class InterAction_DB
    {
        // class singleton
        private static InterAction_DB d;
        
        public static InterAction_DB getData()
        {
            if (d == null)
            {
                d = new InterAction_DB();
            }
            return d;
        }


        //// ici c'est la partie base de donnée
        /// constructeur 
        /// select
        /// insert
        /// create table
        /// demete
        /// 

    }
}
