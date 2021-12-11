using System;
using System.Runtime.Serialization;

namespace JarmuKolcsonzoABGyak
{
    [Serializable]
    internal class ABKivetel : Exception
    {
        public ABKivetel(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}