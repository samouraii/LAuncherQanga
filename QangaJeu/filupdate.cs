
using laucherQange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherQangaClient
{
    class filupdate
    {
        //private List<Jupdate> jupdateList = new List<Jupdate>();
        private string fichier;
        private string updateur;

        // 0 a cree, 1 modifier, 2 a supprimer
        private int type;


        public string Fichier
        {
            get
            {
                return fichier;
            }

            set
            {
                fichier = value;
            }
        }



        public int Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public string Updateur
        {
            get
            {
                return updateur;
            }

            set
            {
                updateur = value;
            }
        }


    }
}
