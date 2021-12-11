using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzoABGyak
{

    enum AutoTipus
    {
        Csaladi,
        Kisteher,
        Sport
    }
    class Auto : Jarmu
    {
        AutoTipus autoTipusa;
        byte szallithatoSzemSzam;

        public byte SzallithatoSzemSzam 
        { 
            get => szallithatoSzemSzam; 
            private set => szallithatoSzemSzam = value; 
        }
        internal AutoTipus AutoTipusa
        { 
            get => autoTipusa; 
            private set => autoTipusa = value; 
        }

        public Auto(string rendszam, string marka, string tipus, int futottKM, bool kolcsonozve, AutoTipus autoTipusa, byte szallithatoSzemSzam) : base(rendszam, marka, tipus, futottKM, kolcsonozve)
        {
            SzallithatoSzemSzam = szallithatoSzemSzam;
            AutoTipusa = autoTipusa;
        }
    }
}
