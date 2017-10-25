using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Abalone
{
    public class marbleFunctions
    {
        internal static void marbleInit(ref Marble[][] a, ref Marble[][] b)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int z = 0; z < i + 5; z++)
                {
                    a[i][z] = new Marble(true, i, z);
                    a[8 - i][z] = new Marble(true, 8 - i, z);

                    b[i][z] = new Marble(false, i, z);
                    b[8 - i][z] = new Marble(false, 8 - i, z);
                }

            }
            for (int z = 0; z < 9; z++)
            {
                a[4][z] = new Marble(true, 4, z);
            }
        }

        internal static void boardSetup(ref Marble[][] a, ref Marble[][] b, ref Options op)
        {
            foreach (Marble[] i in a) foreach (Marble z in i) z.changeType(0, op.noPlayers);
            foreach (Marble[] i in b) foreach (Marble z in i) z.changeType(0, op.noPlayers);

            //board type check
            //board set up function
        }
    }
}
