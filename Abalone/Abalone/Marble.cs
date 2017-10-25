using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Abalone {
    class Marble {
        public Vector3 position;
        private int type = 0;

        public Marble(Boolean b, float r, float c) {
            if (b) position = new Vector3(GameFunctions.calcPos(r, c), 0);
            else position = new Vector3(GameFunctions.calcPos(r, c).X, 0, GameFunctions.calcPos(r, c).Y);
        }

        public Vector4 color() {
            
            switch (type)
            {
                case 1:
                    return GameConstants.Color1;
                case 2:
                    return GameConstants.Color2;
                case 3:
                    return GameConstants.Color3;
                case 4:
                    return GameConstants.Color4;
                default:
                    return GameConstants.Color0;
            }
        }

        public void changeType(int i)
        {
            if (GameConstants.numPlayers == 2 && i > 2) i -= 2;
            type = i;
        }

        public void changeType2(int i)
        {
            changeType(i + 1);
        }

        public int getType()
        {
            return type - 1;
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
