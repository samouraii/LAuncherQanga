
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace laucherQange
{
    

    public class Jupdate
    {

        public int src;

        public int dest;

        public string action;

        public int lengt;
        //public keyValue[] toto;

        public List<keyValue> modif;
        public Jupdate()
        {

        }

        public Jupdate(int src, int dest, string action, int lengt)
        {


            this.src = src;
            this.dest = dest;
            this.action = action;
            this.lengt = lengt;
            if (action == "AddDestination" || action == "Replace")
            {
                modif = new List<keyValue>();
            }
        }



        public void AddModif(int key, int value)
        {
            modif.Add(new keyValue(key, value));
            //toto = modif.ToArray();
        }


        public List<keyValue> getModif()
        {
            return modif;
        }

    }
}
