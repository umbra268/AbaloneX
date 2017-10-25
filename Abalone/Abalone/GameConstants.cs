//-----------------------------------------------------------------------------
// GameConstants.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Abalone
{
    public class GameConstants
    {
        //marble colours
        public static int numPlayers = 4;
        public static int boardType = 2;
        public static Vector4 Color0 = new Vector4(255, 255, 255, 0.3f);
        public static Vector4 Color1 = new Vector4(255, 30, 0, 0.7f);
        public static Vector4 Color2 = new Vector4(0, 191, 50, 0.7f);
        public static Vector4 Color3 = new Vector4(66, 18, 175, 0.7f);
        public static Vector4 Color4 = new Vector4(255, 218, 0, 0.7f);
        public static float   MarbleA= (float)Math.Sqrt(9 - Math.Pow(1.5f, 2));
//(9-1.5^2=6.75)
    }
}
