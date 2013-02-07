using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaiDSyM
{
    class KontrolaParzystosci
    {

        public static byte[] liczBityParzystości(byte[] pasekZDysk1, byte[] pasekZDysk2, byte[] pasekZDysk3)
        {
            byte[] wynik = new byte[32];

            for (int i = 0; i < 32; i++)                
                wynik[i] = (byte)(pasekZDysk1[i] ^ pasekZDysk2[i] ^ pasekZDysk3[i]);             

            return wynik;
        }
    }
}
