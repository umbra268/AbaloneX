using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Abalone
{
    delegate void board(Marble[][] a, Marble[][] b, int noPlayers);
    delegate void point(Vector2[][][] p, int noPlayers);
    public static class Board
    {
        private static List<board> Boards = new List<board>();
        private static List<point> Points = new List<point>();
        public static void SetUp(int type, Marble[][] a, Marble[][] b, int noPlayers)
        {
            foreach (Marble[] i in a) foreach (Marble z in i) z.changeType(0, noPlayers);
            foreach (Marble[] i in b) foreach (Marble z in i) z.changeType(0, noPlayers);
            Boards[type](a, b, noPlayers);
        }

        public static void init() {
            Boards.Add((Marble[][] a, Marble[][] b, int noPlayers) =>
            {
                foreach (Marble i in a[0]) i.changeType(1, noPlayers);
                foreach (Marble i in a[1]) i.changeType(1, noPlayers);
                a[2][2].changeType(1, noPlayers);
                a[2][3].changeType(1, noPlayers);
                a[2][4].changeType(1, noPlayers);

                foreach (Marble i in a[8]) i.changeType(2, noPlayers);
                foreach (Marble i in a[7]) i.changeType(2, noPlayers);
                a[6][2].changeType(2, noPlayers);
                a[6][3].changeType(2, noPlayers);
                a[6][4].changeType(2, noPlayers);

                foreach (Marble i in b[0]) i.changeType(3, noPlayers);
                foreach (Marble i in b[1]) i.changeType(3, noPlayers);
                b[2][2].changeType(3, noPlayers);
                b[2][3].changeType(3, noPlayers);
                b[2][4].changeType(3, noPlayers);

                foreach (Marble i in b[8]) i.changeType(4, noPlayers);
                foreach (Marble i in b[7]) i.changeType(4, noPlayers);
                b[6][2].changeType(4, noPlayers);
                b[6][3].changeType(4, noPlayers);
                b[6][4].changeType(4, noPlayers);
            });

            Boards.Add((Marble[][] a, Marble[][] b, int noPlayers) =>
            {
                for (int i = 0; i < 2; i++)
                {
                    a[1][i].changeType(1, noPlayers);
                    a[1][4 + i].changeType(2, noPlayers);
                    a[3][1 + i].changeType(1, noPlayers);
                    a[3][5 + i].changeType(2, noPlayers);

                    a[7][i].changeType(4, noPlayers);
                    a[7][4 + i].changeType(3, noPlayers);
                    a[5][1 + i].changeType(4, noPlayers);
                    a[5][5 + i].changeType(3, noPlayers);


                    b[1][i].changeType(3, noPlayers);
                    b[1][4 + i].changeType(4, noPlayers);
                    b[3][1 + i].changeType(3, noPlayers);
                    b[3][5 + i].changeType(4, noPlayers);

                    b[7][i].changeType(2, noPlayers);
                    b[7][4 + i].changeType(1, noPlayers);
                    b[5][1 + i].changeType(2, noPlayers);
                    b[5][5 + i].changeType(1, noPlayers);
                }
                for (int i = 0; i < 2; i++)
                {
                    a[2][0 + (4 * i)].changeType(1 + i, noPlayers);
                    a[2][1 + (4 * i)].changeType(4 - i, noPlayers);
                    a[2][2 + (4 * i)].changeType(1 + i, noPlayers);

                    b[2][0 + (4 * i)].changeType(3 + i, noPlayers);
                    b[2][1 + (4 * i)].changeType(2 - i, noPlayers);
                    b[2][2 + (4 * i)].changeType(3 + i, noPlayers);

                    a[6][0 + (4 * i)].changeType(4 - i, noPlayers);
                    a[6][1 + (4 * i)].changeType(1 + i, noPlayers);
                    a[6][2 + (4 * i)].changeType(4 - i, noPlayers);

                    b[6][0 + (4 * i)].changeType(2 - i, noPlayers);
                    b[6][1 + (4 * i)].changeType(3 + i, noPlayers);
                    b[6][2 + (4 * i)].changeType(2 - i, noPlayers);
                }
            });

            Points.Add((Vector2[][][] p, int noPlayers) =>
            {
                p[0][0][0] = new Vector2(0, 2);
                p[1][0][0] = new Vector2(8, 2);

                if (noPlayers == 2)
                {
                    p[0][1][0] = new Vector2(0, 2);
                    p[1][1][0] = new Vector2(8, 2);
                }
                else if (noPlayers == 4)
                {
                    p[0][1][0] = new Vector2(4, 4);
                    p[1][1][0] = new Vector2(4, 4);

                    p[2][0][0] = new Vector2(4, 4);
                    p[3][0][0] = new Vector2(4, 4);

                    p[2][1][0] = new Vector2(0, 2);
                    p[3][1][0] = new Vector2(8, 2);
                }
            });

            Points.Add((Vector2[][][] p, int noPlayers) =>
            {
                for (int i = 0; i < noPlayers; i++)
                {
                    p[i][0][0] = new Vector2(4, 4);
                    p[i][1][0] = new Vector2(4, 4);
                }
            });
        }

        public static void PointSetUp(int noPlayers, int boardType, ref Vector2[][][] p)
        {
            p = new Vector2[noPlayers][][];
            for (int i = 0; i < noPlayers; i++)
            {
                p[i] = new Vector2[2][];
                p[i][0] = new Vector2[2];
                p[i][1] = new Vector2[2];
            }

            Points[boardType](p, noPlayers);
        }
    }
}
