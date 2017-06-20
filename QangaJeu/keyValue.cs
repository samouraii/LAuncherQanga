using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace laucherQange
{

    public class keyValue
    {
       
       public int key;
      
        public int value;

        public keyValue(int key,int value)
        {
            this.key = key;
            this.value = value;
        }

        public int Key
        {
            get
            {
                return key;
            }

            set
            {
                key = value;
            }
        }

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
            }
        }
    }
}
