using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Abalone {
    public class Marble {
        public Vector3 position;
        private int Type = 0;

        public Marble(Boolean b, float r, float c) {
            if (b) position = new Vector3(GameFunctions.calcPos(r, c), 0);
            else position = new Vector3(GameFunctions.calcPos(r, c).X, 0, GameFunctions.calcPos(r, c).Y);
        }

        public void changeType(int type, int noPlayers)
        {
            if (noPlayers == 2 && type > 2) type -= 2;
            Type = type;
        }

        public int getType()
        {
            return Type;
        }

        public Vector2 getPosition()
        {
            if (position.Z != 0) return new Vector2(position.X, position.Z);
            else return new Vector2(position.X, position.Y);
        }

        public bool isSelected(Vector2 point, int angle, int distance)
        {
            angle *= 45;
            if (GameFunctions.isTheSame(GameFunctions.calcPos(point), getPosition())) return true;
            else if (GameFunctions.isTheSame((GameFunctions.calcPos(point) + distance * GameFunctions.Vector2FromAngle(angle)), getPosition())) return true;
            else if (GameFunctions.isTheSame((2 * GameFunctions.calcPos(point) + distance * GameFunctions.Vector2FromAngle(angle)), 2 * getPosition())) return true;
            else return false;
        }

        public bool isSelected(Vector2[] a)
        {
            return isSelected(a[0], (int)(a[1].X), (int)(a[1].Y));
        }   
    }
}