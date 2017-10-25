#region("Import Statements")
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#endregion()

namespace Abalone
{
    public partial class Game3 : Microsoft.Xna.Framework.Game
    {
        bool CheckMarblePos(Vector2 p)
        {
            if (p.X < 0 || p.X > 8) return false;
            else if (p.X == 0 && (p.Y < 0 || p.Y > 4)) return false;
            else if (p.X == 1 && (p.Y < 0 || p.Y > 5)) return false;
            else if (p.X == 2 && (p.Y < 0 || p.Y > 6)) return false;
            else if (p.X == 3 && (p.Y < 0 || p.Y > 7)) return false;
            else if (p.X == 4 && (p.Y < 0 || p.Y > 8)) return false;
            else if (p.X == 8 && (p.Y < 0 || p.Y > 4)) return false;
            else if (p.X == 7 && (p.Y < 0 || p.Y > 5)) return false;
            else if (p.X == 6 && (p.Y < 0 || p.Y > 6)) return false;
            else if (p.X == 5 && (p.Y < 0 || p.Y > 7)) return false;
            else return true;
        }

        bool CheckMarbleType(Vector2 p, int type)
        {
            if (screen.Y == 1 && a[(int)(p.X)][(int)(p.Y)].getType() == type) return true;
            else if (screen.Y == 2 && b[(int)(p.X)][(int)(p.Y)].getType() == type) return true;
            else return false;
        }

        int GetMarbleType(Vector2 p)
        {
            if (screen.Y == 1) return a[(int)(p.X)][(int)(p.Y)].getType();
            else if (screen.Y == 2) return b[(int)(p.X)][(int)(p.Y)].getType();
            else return -1;
        }

        void MoveMarble(Vector2 point1, Vector2 point2)
        {
            int x1 = 0;
            int x2 = 0;
            if (screen.Y == 1)
            {
                x1 = a[(int)(point2.X)][(int)(point2.Y)].getType();
                x2 = a[(int)(point1.X)][(int)(point1.Y)].getType();
                a[(int)(point1.X)][(int)(point1.Y)].changeType2(x1);
                a[(int)(point2.X)][(int)(point2.Y)].changeType2(x2);
            }
            else if (screen.Y == 2)
            {
                x1 = b[(int)(point2.X)][(int)(point2.Y)].getType();
                x2 = b[(int)(point1.X)][(int)(point1.Y)].getType();
                b[(int)(point1.X)][(int)(point1.Y)].changeType2(x1);
                b[(int)(point2.X)][(int)(point2.Y)].changeType2(x2);
            }
        }

        void ChangeMarble(Vector2 po, int type)
        {
            if (screen.Y == 1)
            {
                a[(int)(po.X)][(int)(po.Y)].changeType(type);
            }
            else if (screen.Y == 2)
            {
                b[(int)(po.X)][(int)(po.Y)].changeType(type);
            }
        }
    }
}
