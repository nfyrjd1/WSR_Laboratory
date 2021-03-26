using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSR_Laboratory.Service
{
    public class Levenshtein
    {
        public static int Distance(string s1, string s2)
        {
            int l1 = s1.Length;
            int l2 = s2.Length;

            if (l1 == 0)
            {
                return l2;
            }

            if (l1 == 0)
            {
                return l1;
            }

            int[,] m = new int[l1+1, l2+1];

            for (int i = 0; i <= l1; i++)
            {
                m[i, 0] = i;
            }

            for (int i = 0; i <= l2; i++)
            {
                m[0, i] = i;
            }

            for(int i = 1; i <= l1; i++)
            {
                for(int j = 1; j <= l2; j++)
                {
                    int cost = s2[j-1] == s1[i-1] ? 0 : 1;

                    m[i,j] = Math.Min(
                        Math.Min(m[i - 1, j] + 1, m[i, j - 1] + 1),
                        m[i-1, j-1] + cost
                    );
                }
            }

            return m[l1, l2];
        }
    }
}
