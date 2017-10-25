using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Abalone
{
    public class Options
    {
        //static string[] players = { "2 Players", "4 Players" };
        public int noPlayers;

        //static string[] boardType = { "Standard", "German Daisy" };
        public int boardType;

        public int saved;

        private Color[] ColorA = null;

        public Options()
        {            
            boardType = 0;
            noPlayers = 4;
            ColorA = new Color[5];
            ColorA[0] = new Color(255,255,255);
            ColorA[1] = new Color(255, 30, 0);
            ColorA[2] = new Color(0, 191, 50);
            ColorA[3] = new Color(66, 18, 175);
            ColorA[4] = new Color(255, 218, 0);
            saved = 0;
            //load game options
        }

        public Color getColor(int type)
        {
            return ColorA[type];
        }

        public Color getColorAlpha(int type)
        {
            Color a = ColorA[type];
            a.A = (byte) 0.3f;
            return a;
        }
    }
}
