using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzoABGyak
{
    class Kolcsonzo
    {
        List<Jarmu> jarmuvek;
        string megnevezes;
        int? id;
        Cim cim;

        public string Megnevezes
        {
            get => megnevezes;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length <= 64)
                {
                    megnevezes = value;
                }
                else
                {
                    throw new ArgumentException("A megnevezes nem lehet ures es maximum 64 karakter lehet!");
                }
            }
        }
        public int? Id
        {
            get => id;
            set
            {
                if (id == 0)
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("Az ID csak egyszer kaphat erteket!");
                }
            }
        }
        internal List<Jarmu> Jarmuvek { get => jarmuvek;}
        internal Cim Cim { get => cim; set => cim = value; }

        public Kolcsonzo(string megnevezes, Cim cim)
        {
            Megnevezes = megnevezes;
            Cim = cim;
            jarmuvek = new List<Jarmu>();
        }

        public Kolcsonzo(int? id, string megnevezes, Cim cim) : this(megnevezes, cim)
        {
            Id = id;
        }

        public override string ToString()
        {
            return megnevezes;
        }
    }
}
