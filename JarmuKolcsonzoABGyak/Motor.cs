using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzoABGyak
{
    class Motor : Jarmu
    {
        double hengerurtartalom;


        public double Hengerurtartalom 
        { 
            get => hengerurtartalom;
            private set
            {
                if (value > 0)
                {
                    hengerurtartalom = value;
                }
                else
                {
                    throw new ArgumentException("A hengerurtartalom nem lehet negativ!");
                }
            }
        }

        public Motor(string rendszam, string marka, string tipus, int futottKM, bool kolcsonozve, double hengerurtartalom) : base(rendszam, marka, tipus, futottKM, kolcsonozve)
        {
            Hengerurtartalom = hengerurtartalom;
        }

    }
}
