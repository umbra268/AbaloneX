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
        public int currentNoPlayers;

        //static string[] boardType = { "Standard", "German Daisy" };
        public int currentBoardType;

        public Vector4 Color0;
        public Vector4 Color1;
        public Vector4 Color2;
        public Vector4 Color3;
        public Vector4 Color4;

        public Options()
        {            
            currentBoardType = 1;
            currentNoPlayers = 4;
            Color0 = new Vector4(255, 255, 255, 0.3f);
            Color1 = new Vector4(255, 30, 0, 0.8f);
            Color2 = new Vector4(0, 191, 50, 0.8f);
            Color3 = new Vector4(66, 18, 175, 0.8f);
            Color4 = new Vector4(255, 218, 0, 0.8f);
            //load game options
        }

        public Color getColor(int i)
        {
            Vector4 b = new Vector4();
            if (i == 0) b = Color0 / 255;
            if (i == 1) b = Color1 / 255;
            if (i == 2) b = Color2 / 255;
            if (i == 3) b = Color3 / 255;
            if (i == 4) b = Color4 / 255;
            Color a = new Color(b.X, b.Y, b.Z, b.W);
            return a;
        }
    }
}
