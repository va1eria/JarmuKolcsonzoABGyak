using System;

namespace JarmuKolcsonzoABGyak
{
    enum KozteruletJelleg
    {
        utca,
        ut,
        ter,
        koz
    }
    struct Cim
    {
        string telepules, kozterulet, hazszam;
        short irsz;
        KozteruletJelleg kozteruletJellege;

        public string Telepules
        {
            get => telepules;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 64)
                {
                    telepules = value;
                }
                else
                {
                    throw new ArgumentException("A telepules neve nem lehet hosszabb mint 64 karakter es nem lehet ures!");
                }
            }
        }
        public string Kozterulet
        {
            get => kozterulet;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 64)
                {
                    kozterulet = value;
                }
                else
                {
                    throw new ArgumentException("A kozterulet neve nem lehet hosszabb mint 64 karakter es nem lehet ures!");
                }
            }
        }
        public string Hazszam
        {
            get => hazszam;
            private set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 16)
                {
                    hazszam = value;
                }
                else
                {
                    throw new ArgumentException("A hazszam neve nem lehet hosszabb mint 16 karakter es nem lehet ures!");
                }
            }
        }
        public short Irsz
        {
            get => irsz;
            private set
            {
                if (value.ToString().Length == 4)
                {
                    irsz = value;
                }
                else
                {
                    throw new ArgumentException("Az iranyitoszam egy pontosan 4 karakter hosszu szam kell legyen!");
                }
            }
        }
        internal KozteruletJelleg KozteruletJellege { get => kozteruletJellege; private set => kozteruletJellege = value; }

        public Cim(string telepules, string kozterulet, string hazszam, short irsz, KozteruletJelleg kozteruletJellege) : this()
        {
            Telepules = telepules;
            Kozterulet = kozterulet;
            Hazszam = hazszam;
            Irsz = irsz;
            KozteruletJellege = kozteruletJellege;
        }

        public override string ToString()
        {
            return $"{irsz} {telepules} {kozterulet} {kozteruletJellege} {hazszam}";
        }
    }
}