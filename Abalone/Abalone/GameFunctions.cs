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
    public class GameFunctions
    {
        public static Vector2 calcPos(Vector2 rc)
        {
            return calcPos(rc.X, rc.Y);
        }

        public static Vector2 calcPos(float r, float c)
        {
            float i, z;
            if (r <= 4) z = 2 - (r / 2);
            else z = 2 - ((8 - r) / 2);
            i = z - 4;
            i += c;
            if (r > 4) z *= -1;
            return new Vector2(i * 3, 2 * z * GameConstants.MarbleA);
        }

        public static bool isTheSame(float a, float b)
        {
            if (a > b) return (a - b) <= 0.00001;
            else return (b - a) <= 0.00001;
        }

        public static bool isTheSame(Vector2 a, Vector2 b)
        {
            return isTheSame(a.X, b.X) && isTheSame(a.Y, b.Y);
        }

        public static Vector2 Vector2FromAngle(int angle)
        {
            float ang = MathHelper.ToRadians(angle);
            if (angle == 90 || angle == 270) return new Vector2((float)(3 * Math.Sin(ang)), 0);
            else return new Vector2((float)(Math.Sqrt(4.5) * Math.Sin(ang)), (float)(Math.Sqrt(13.5) * Math.Cos(ang)));
        }
    }
}
