using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JarmuKolcsonzoABGyak
{
    abstract class Jarmu
    {
        string rendszam, marka, tipus;
        int futottKM;
        bool kolcsonozve;

        public string Rendszam 
        { 
            get => rendszam;
            private set
            {
                //if(Regex.IsMatch(value, "[A-Z]{3}-[0-9]{3}"))
                if (!string.IsNullOrWhiteSpace(value) && value.Length == 7)
                {
                    rendszam = value;
                }
                else
                {
                    throw new ArgumentException("A rendszam nem lehet ures es pontosan 7 karakteres legyen!");
                }
            }
        }
        public string Marka 
        { 
            get => marka;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 64)
                {
                    marka = value;
                }
                else
                {
                    throw new ArgumentException("A marka nem lehet ures es maximum 64 karakter lehet!");
                }
            }
        }
        public string Tipus
        {
            get => tipus;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 64)
                {
                    tipus = value;
                }
                else
                {
                    throw new ArgumentException("A tipus nem lehet ures es maximum 64 karakter lehet!");
                }
            }
        }
        public int FutottKM 
        { 
            get => futottKM;
            set
            {
                if (value >= futottKM)
                {
                    futottKM = value;
                }
                else
                {
                    throw new ArgumentException("A futott km nem lehet kisebb, mint az elozoleg beallitott ertek!");
                }
            }
        }
        public bool Kolcsonozve { get => kolcsonozve; set => kolcsonozve = value; }

        protected Jarmu(string rendszam, string marka, string tipus, int futottKM, bool kolcsonozve)
        {
            Rendszam = rendszam;
            Marka = marka;
            Tipus = tipus;
            FutottKM = futottKM;
            Kolcsonozve = kolcsonozve;
        }

        public override string ToString()
        {
            return $"[{rendszam}] {marka} {tipus}";
        }

        public override bool Equals(object obj)
        {
            return obj is Jarmu masik && masik.marka == marka;
        }
    }
}
