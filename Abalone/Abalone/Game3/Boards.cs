namespace Abalone
{
    public partial class Game3 : Microsoft.Xna.Framework.Game
    {
        protected void Board1()
        {
            foreach (Marble i in a[0]) i.changeType(1);
            foreach (Marble i in a[1]) i.changeType(1);
            a[2][2].changeType(1);
            a[2][3].changeType(1);
            a[2][4].changeType(1);

            foreach (Marble i in a[8]) i.changeType(2);
            foreach (Marble i in a[7]) i.changeType(2);
            a[6][2].changeType(2);
            a[6][3].changeType(2);
            a[6][4].changeType(2);

            foreach (Marble i in b[0]) i.changeType(3);
            foreach (Marble i in b[1]) i.changeType(3);
            b[2][2].changeType(3);
            b[2][3].changeType(3);
            b[2][4].changeType(3);

            foreach (Marble i in b[8]) i.changeType(4);
            foreach (Marble i in b[7]) i.changeType(4);
            b[6][2].changeType(4);
            b[6][3].changeType(4);
            b[6][4].changeType(4);
        }

        protected void Board2()
        {
            for (int i = 0; i < 2; i++)
            {
                a[1][i].changeType(1);
                a[1][4 + i].changeType(2);
                a[3][1 + i].changeType(1);
                a[3][5 + i].changeType(2);

                a[7][i].changeType(4);
                a[7][4 + i].changeType(3);
                a[5][1 + i].changeType(4);
                a[5][5 + i].changeType(3);


                b[1][i].changeType(3);
                b[1][4 + i].changeType(4);
                b[3][1 + i].changeType(3);
                b[3][5 + i].changeType(4);

                b[7][i].changeType(2);
                b[7][4 + i].changeType(1);
                b[5][1 + i].changeType(2);
                b[5][5 + i].changeType(1);
            }
            for (int i = 0; i < 2; i++)
            {
                a[2][0 + (4 * i)].changeType(1 + i);
                a[2][1 + (4 * i)].changeType(4 - i);
                a[2][2 + (4 * i)].changeType(1 + i);

                b[2][0 + (4 * i)].changeType(3 + i);
                b[2][1 + (4 * i)].changeType(2 - i);
                b[2][2 + (4 * i)].changeType(3 + i);

                a[6][0 + (4 * i)].changeType(4 - i);
                a[6][1 + (4 * i)].changeType(1 + i);
                a[6][2 + (4 * i)].changeType(4 - i);

                b[6][0 + (4 * i)].changeType(2 - i);
                b[6][1 + (4 * i)].changeType(3 + i);
                b[6][2 + (4 * i)].changeType(2 - i);
            }

        }
    }
}
