using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public static class Eratosthene
    {
        public static int[] EratosthenesSieve(int n)
        {
            int[] crible = new int[n];
            for (int i = 0; i< crible.Length; i++)
            {
                crible[i] = i+1;
            }

            int M = (int)Math.Sqrt(crible[crible.Length - 1]);
            crible[0] = int.MinValue;
            for (int i = 1; i < M; i++)
            {
                if (crible[i] != int.MinValue)
                {
                    for (int j = crible[i] +1; j < crible.Length; j++)
                    {
                        if (crible[j]%crible[i] == 0)
                        {
                            crible[j] = int.MinValue;
                        }
                    }
                }
            }
            return crible;
        }
    }
}
