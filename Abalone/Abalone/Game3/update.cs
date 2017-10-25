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
using System.Diagnostics;

namespace Abalone
{
    public partial class Game3 : Microsoft.Xna.Framework.Game
    {
        #region("Update Functions")
        protected override void Update(GameTime gameTime)
        {
            newState[0] = GamePad.GetState(PlayerIndex.One);
            newState[1] = GamePad.GetState(PlayerIndex.Two);
            if (GameConstants.numPlayers == 4)
            {
                newState[2] = GamePad.GetState(PlayerIndex.Three);
                newState[3] = GamePad.GetState(PlayerIndex.Four);
            }
            #region("Rotate 3D board")
            if (screen.X == 0 || screen.Y == 0)
            {
                angle += 0.001f;
                cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            }
            #endregion()
            if (newState[0].Buttons.Back == ButtonState.Pressed || newState[1].Buttons.Back == ButtonState.Pressed) this.Exit();
            if (GameConstants.numPlayers == 4) if (newState[2].Buttons.Back == ButtonState.Pressed || newState[3].Buttons.Back == ButtonState.Pressed) this.Exit();
            #region("MainMenu")
            if (screen.X == 0)
            {
                if ((newState[0].Buttons.Start == ButtonState.Pressed || newState[1].Buttons.Start == ButtonState.Pressed) || (GameConstants.numPlayers == 4 && (newState[2].Buttons.Start == ButtonState.Pressed || newState[3].Buttons.Start == ButtonState.Pressed)))
                {
                    screen.X = 1;
                    currentPlayer = 0;

                    p = new Vector2[GameConstants.numPlayers][][];
                    for (int i = 0; i < GameConstants.numPlayers; i++)
                    {
                        p[i] = new Vector2[2][];
                        p[i][0] = new Vector2[2];
                        p[i][1] = new Vector2[2];
                    }

                    if (GameConstants.boardType == 1)
                    {

                        p[0][0][0] = new Vector2(0, 2);

                        p[1][0][0] = new Vector2(8, 2);

                        if (GameConstants.numPlayers == 2)
                        {
                            p[0][1][0] = new Vector2(0, 2);
                            p[1][1][0] = new Vector2(8, 2);
                        }
                        else if (GameConstants.numPlayers == 4)
                        {
                            p[0][1][0] = new Vector2(4, 4);
                            p[1][1][0] = new Vector2(4, 4);

                            p[2][0][0] = new Vector2(4, 4);
                            p[3][0][0] = new Vector2(4, 4);

                            p[2][1][0] = new Vector2(0, 2);
                            p[3][1][0] = new Vector2(8, 2);
                        }
                    }
                    else if (GameConstants.boardType == 2)
                    {
                        for (int i = 0; i < GameConstants.numPlayers; i++)
                        {
                            p[i][0][0] = new Vector2(4, 4);
                            p[i][1][0] = new Vector2(4, 4);
                        }
                    }
                }
            }
            #endregion()
            #region("Error Screen")
            else if (screen.W != 0)
            {
                if (newState[currentPlayer].Buttons.B == ButtonState.Pressed) screen.W = 0;
            }
            #endregion()
            #region("Game Play")
            else if (screen.X == 1)
            {
                #region("Check if the first Marble can be selected")
                if (screen.Z == 1)
                {
                    bool error = false;
                    if (screen.Y == 1 && a[(int)(p[currentPlayer][(int)(screen.Y - 1)][0].X)][(int)(p[currentPlayer][(int)(screen.Y - 1)][0].Y)].getType() != currentPlayer) error = true;
                    else if (screen.Y == 2 && b[(int)(p[currentPlayer][(int)(screen.Y - 1)][0].X)][(int)(p[currentPlayer][(int)(screen.Y - 1)][0].Y)].getType() != currentPlayer) error = true;
                    if (error == true)
                    {
                        screen.W = 1;
                        screen.Z = 0;
                    }
                    else screen.Z = 2;
                }
                #endregion()
                #region("Change selection around selected marble")
                else if (screen.Z == 2)
                {
                    angleMove(newState[currentPlayer], oldState[currentPlayer]);
                    if (newState[currentPlayer].Buttons.A == ButtonState.Pressed && oldState[currentPlayer].Buttons.A == ButtonState.Released) screen.Z = 3;
                    if (newState[currentPlayer].Buttons.B == ButtonState.Pressed && oldState[currentPlayer].Buttons.B == ButtonState.Released)
                    {
                        screen.Z = 0;
                        p[currentPlayer][(int)(screen.Y - 1)][1] = Vector2.Zero;
                    }
                }
                #endregion()
                #region("Check only your marbles are selected")
                else if (screen.Z == 3)
                {
                    bool legalMove;
                    Vector2[] marbles;
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y > 0) marbles = new Vector2[(int)(p[currentPlayer][(int)(screen.Y - 1)][1].Y) + 1];
                    else marbles = new Vector2[1];
                    marbles[0] = p[currentPlayer][(int)(screen.Y - 1)][0];
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y >= 1) marbles[1] = angleChangePoint(p[currentPlayer][(int)(screen.Y - 1)][0], (int)(p[currentPlayer][(int)(screen.Y - 1)][1].X));
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y == 2) marbles[2] = angleChangePoint(marbles[1], (int)(p[currentPlayer][(int)(screen.Y - 1)][1].X));
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y >= 1)
                    {
                        marbles[1] = angleChangePoint(p[currentPlayer][(int)(screen.Y - 1)][0], (int)(p[currentPlayer][(int)(screen.Y - 1)][1].X));
                        legalMove = CheckMarblePos(marbles[1]);
                        if (legalMove == false) p[currentPlayer][(int)(screen.Y - 1)][1] = Vector2.Zero;

                        if (p[currentPlayer][(int)(screen.Y - 1)][1].Y == 2)
                        {
                            legalMove = CheckMarblePos(marbles[1]);
                            if (legalMove == false) p[currentPlayer][(int)(screen.Y - 1)][1].Y--;
                        }
                    }

                    legalMove = true;
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y >= 1)
                    {
                        if (CheckMarbleType(marbles[1], currentPlayer) == false) legalMove = false;
                        if (p[currentPlayer][(int)(screen.Y - 1)][1].Y == 2)
                        {
                            if (CheckMarbleType(marbles[2], currentPlayer) == false) legalMove = false;
                        }
                    }

                    if (legalMove == false)
                    {
                        screen.W = 5;
                        screen.Z = 2;
                    }
                    else
                    {
                        screen.Z = 4;
                        angle2 = 0;
                    }
                }
                #endregion()
                #region("Get angle for moving")
                else if (screen.Z == 4)
                {
                    angle2 = subAngleMove(newState[currentPlayer], oldState[currentPlayer]);
                    if (angle2 != 0) screen.Z = 5;
                    if (newState[currentPlayer].Buttons.B == ButtonState.Pressed && oldState[currentPlayer].Buttons.B == ButtonState.Released) screen.Z = 2;
                }
                #endregion()
                #region("Check Move & Move")
                else if (screen.Z == 5)
                {
                    bool legal = true;
                    #region("Move single marble")
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y == 0)
                    {
                        point = angleChangePoint(p[currentPlayer][(int)(screen.Y - 1)][0], angle2);
                        if (CheckMarblePos(point))
                        {
                            if (CheckMarbleType(point, -1)) MoveMarble(p[currentPlayer][(int)(screen.Y - 1)][0], point);
                            else legal = false;
                        }
                        else legal = false;
                    }
                    #endregion()
                    #region("Moving and pushing marbles")
                    else if (p[currentPlayer][(int)(screen.Y - 1)][1].X == angle2 || p[currentPlayer][(int)(screen.Y - 1)][1].X == ((angle2 + 4) % 8))
                    {
                        Vector2[] px = new Vector2[2];
                        int y = (int)(p[currentPlayer][(int)(screen.Y - 1)][1].Y + 1);
                        px[0] = p[currentPlayer][(int)(screen.Y - 1)][0];
                        px[1] = p[currentPlayer][(int)(screen.Y - 1)][1];
                        if (px[1].X == ((angle2 + 4) % 8))
                        {
                            for (int i = y - 1; i > 0; i--)
                            {
                                px[0] = angleChangePoint(px[0], (int)(px[1].X));
                            }
                            px[1].X = angle2;
                        }
                        Vector2[] points = new Vector2[y];
                        points[0] = px[0];
                        points[1] = angleChangePoint(px[0], (int)(px[1].X));
                        if (y == 3) points[2] = angleChangePoint(points[1], (int)(px[1].X));
                        Vector2[] points2 = new Vector2[y];

                        for (int i = 0; i < y; i++)
                        {
                            points2[i] = angleChangePoint(points[i], angle2);
                        }

                        if (CheckMarblePos(points2[y - 1]))
                        {
                            if (CheckMarbleType(points2[y - 1], -1))
                            {
                                MoveMarble(points2[y - 1], points[0]);
                            }
                            else if (y == 2)
                            {
                                if ((!CheckMarbleType(points2[y - 1], currentPlayer)) && CheckMarblePos(angleChangePoint(points2[y - 1], angle2)))
                                {
                                    if (CheckMarbleType(angleChangePoint(points2[y - 1], angle2), -1))
                                    {
                                        MoveMarble(points2[y - 1], angleChangePoint(points2[y - 1], angle2));
                                        MoveMarble(points2[y - 1], points[0]);
                                    }
                                    else legal = false;
                                }
                                else if ((!CheckMarbleType(points2[y - 1], currentPlayer)) && !CheckMarblePos(angleChangePoint(points2[y - 1], angle2)))
                                {
                                    numOff[GetMarbleType(points2[y - 1])]++;
                                    ChangeMarble(points2[y - 1], 0);
                                    MoveMarble(points2[y - 1], points[0]);
                                }
                                else legal = false;
                            }
                            else if (y == 3)
                            {
                                Vector2 p1 = points2[y - 1];
                                Vector2 p2 = angleChangePoint(p1, angle2);
                                Vector2 p3 = angleChangePoint(p2, angle2);

                                if (!CheckMarbleType(p1, currentPlayer) && CheckMarblePos(p2) && CheckMarblePos(p3))
                                {
                                    if (!CheckMarbleType(p2, currentPlayer) && !CheckMarbleType(p2, -1) && CheckMarbleType(p3, -1))
                                    {
                                        MoveMarble(p2, p3);
                                        MoveMarble(p1, p2);
                                        MoveMarble(p1, points[0]);
                                    }
                                    else if (CheckMarbleType(p2, -1))
                                    {
                                        MoveMarble(p1, p2);
                                        MoveMarble(p1, points[0]);
                                    }
                                    else legal = false;
                                }
                                else if (!CheckMarbleType(p1, currentPlayer) && CheckMarblePos(p2) && !CheckMarblePos(p3))
                                {
                                    if (!CheckMarbleType(p2, currentPlayer) && !CheckMarbleType(p2, -1))
                                    {
                                        numOff[GetMarbleType(p2)]++;
                                        ChangeMarble(p2, 0);
                                        MoveMarble(p2, p1);
                                        MoveMarble(p1, points[0]);
                                    }
                                    else if (CheckMarbleType(p2, -1))
                                    {
                                        MoveMarble(p1, p2);
                                        MoveMarble(p1, points[0]);
                                    }
                                    else legal = false;
                                }
                                else if ((!CheckMarbleType(p1, currentPlayer)) && !CheckMarblePos(p2))
                                {
                                    numOff[GetMarbleType(p1)]++;
                                    ChangeMarble(p1, 0);
                                    MoveMarble(p1, points[0]);
                                }
                                else legal = false;
                            }
                            else legal = false;
                        }
                        else legal = false;
                    }
                    #endregion()
                    #region("Move Marbles with no pushing")
                    else
                    {
                        int y = (int)(p[currentPlayer][(int)(screen.Y - 1)][1].Y + 1);
                        Vector2[] points = new Vector2[y];
                        points[0] = p[currentPlayer][(int)(screen.Y - 1)][0];
                        points[1] = angleChangePoint(p[currentPlayer][(int)(screen.Y - 1)][0], (int)(p[currentPlayer][(int)(screen.Y - 1)][1].X));
                        if (y == 3) points[2] = angleChangePoint(points[1], (int)(p[currentPlayer][(int)(screen.Y - 1)][1].X));
                        Vector2[] points2 = new Vector2[y];

                        for (int i = 0; i < y; i++)
                        {
                            points2[i] = angleChangePoint(points[i], angle2);
                        }

                        foreach (Vector2 n in points2) if (!CheckMarblePos(n)) legal = false;

                        if (legal == true)
                        {
                            foreach (Vector2 n in points2) if (!CheckMarbleType(n, -1)) legal = false;
                            if (legal == true)
                            {
                                for (int i = 0; i < y; i++)
                                {
                                    MoveMarble(points[i], points2[i]);
                                }
                            }
                        }
                    }
                    #endregion()


                    if (legal == false)
                    {
                        screen.W = 4;
                        screen.Z = 4;
                    }
                    else
                    {
                        for (int i = 0; i < GameConstants.numPlayers; i++)
                        {
                            if (numOff[i] == 6)
                            {
                                foreach (Marble[] z in a)
                                {
                                    foreach (Marble x in z)
                                    {
                                        if (x.getType() == i) x.changeType(0);
                                    }
                                }
                                foreach (Marble[] z in b)
                                {
                                    foreach (Marble x in z)
                                    {
                                        if (x.getType() == i) x.changeType(0);
                                    }
                                }
                                numOff[i] *= 10;
                            }
                        }
                        p[currentPlayer][(int)(screen.Y - 1)][1] = Vector2.Zero;
                        screen.Y = 0;
                        screen.Z = 0;
                        bool flag = true;
                        while (flag == true)
                        {
                            flag = false;
                            currentPlayer++;
                            currentPlayer %= GameConstants.numPlayers;
                            if (numOff[currentPlayer] >= 6) flag = true;
                        }
                    }
                }
                #endregion()
                #region("Else something else happens")
                else
                {
                    #region("All board types")
                    if (newState[currentPlayer].Triggers.Left != 0 && screen.Y != 1)
                    {
                        screen.Y = 1;
                        angle = 0;
                        cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                        WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
                    }
                    else if (newState[currentPlayer].Triggers.Right != 0 && screen.Y != 2)
                    {
                        screen.Y = 2;
                        angle = 0;
                        cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                        WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
                    }
                    #endregion()
                    #region("Board Type != 0")
                    if (screen.Y != 0)
                    {
                        playersMove(newState[currentPlayer], oldState[currentPlayer]);
                        if (newState[currentPlayer].Buttons.A == ButtonState.Pressed && oldState[currentPlayer].Buttons.A == ButtonState.Released) screen.Z = 1;
                    }
                    #endregion()
                }
                #endregion()
            }
            #endregion()
            oldState[0] = newState[0];
            oldState[1] = newState[1];
            if (GameConstants.numPlayers == 4)
            {
                oldState[2] = newState[2];
                oldState[3] = newState[3];
            }
            base.Update(gameTime);
        }
        #endregion()
        #region("Extra User Input Functions")
        #region("Move Selected Marble")
        private void playersMove(GamePadState newState, GamePadState oldState)
        {
            bool up = newState.DPad.Up == ButtonState.Pressed;
            bool down = newState.DPad.Down == ButtonState.Pressed;
            bool left = (newState.DPad.Left == ButtonState.Pressed) && (oldState.DPad.Left == ButtonState.Released);
            bool right = (newState.DPad.Right == ButtonState.Pressed) && (oldState.DPad.Right == ButtonState.Released);
            point = p[currentPlayer][(int)(screen.Y - 1)][0];

            if (down && left)
            {
                point.X++;
                if (point.X > 4) point.Y--;
            }
            else if (down && right)
            {
                point.X++;
                point.Y++;
                if (point.X > 4) point.Y--;
            }
            else if (up && left)
            {
                point.X--;
                if (point.X < 4) point.Y--;
            }
            else if (up && right)
            {
                point.X--;
                point.Y++;
                if (point.X < 4) point.Y--;
            }
            else if (left && newState.DPad.Up == ButtonState.Released && newState.DPad.Down == ButtonState.Released)
            {
                point.Y--;
            }
            else if (right && newState.DPad.Up == ButtonState.Released && newState.DPad.Down == ButtonState.Released)
            {
                point.Y++;
            }
            if (point != p[currentPlayer][(int)(screen.Y - 1)][0])
            {
                if ((point.X >= 0 && point.X <= 8 && point.Y >= 0) &&
                    (((point.X == 0 || point.X == 8) && point.Y < 5) ||
                    ((point.X == 1 || point.X == 7) && point.Y < 6) ||
                    ((point.X == 2 || point.X == 6) && point.Y < 7) ||
                    ((point.X == 3 || point.X == 5) && point.Y < 8) ||
                    (point.X == 4 && point.Y < 9)))
                {
                    p[currentPlayer][(int)(screen.Y - 1)][0] = point;
                }
            }
        }
        #endregion()
        #region("Change direction and angle of selection")
        private void angleMove(GamePadState newState, GamePadState oldState)
        {
            if (p[currentPlayer][(int)(screen.Y - 1)][1].X == 0)
            {
                point = Vector2.Zero;
                point.X = subAngleMove(newState, oldState);
                if (point.X != 0)
                {
                    point.Y = 1;
                    p[currentPlayer][(int)(screen.Y - 1)][1] = point;
                }
            }
            else if (p[currentPlayer][(int)(screen.Y - 1)][1].X != 0)
            {
                point = Vector2.Zero;
                point.X = subAngleMove(newState, oldState);
                if (point.X != 0)
                {
                    if (point.X == p[currentPlayer][(int)(screen.Y - 1)][1].X) p[currentPlayer][(int)(screen.Y - 1)][1].Y++;
                    else if ((point.X + 4) % 8 == p[currentPlayer][(int)(screen.Y - 1)][1].X) p[currentPlayer][(int)(screen.Y - 1)][1].Y--;
                    else
                    {
                        screen.W = 2;
                    }
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y > 2)
                    {
                        p[currentPlayer][(int)(screen.Y - 1)][1].Y--;
                        screen.W = 3;
                    }
                    if (p[currentPlayer][(int)(screen.Y - 1)][1].Y == 0) p[currentPlayer][(int)(screen.Y - 1)][1].X = 0;
                }
            }
        }
        private int subAngleMove(GamePadState newState, GamePadState oldState)
        {
            bool up = newState.DPad.Up == ButtonState.Pressed;
            bool down = newState.DPad.Down == ButtonState.Pressed;
            bool left = (newState.DPad.Left == ButtonState.Pressed) && (oldState.DPad.Left == ButtonState.Released);
            bool right = (newState.DPad.Right == ButtonState.Pressed) && (oldState.DPad.Right == ButtonState.Released);

            int tmp = 0;

            if (down && left)
            {
                tmp = 5;
            }
            else if (down && right)
            {
                tmp = 3;
            }
            else if (up && left)
            {
                tmp = 7;
            }
            else if (up && right)
            {
                tmp = 1;
            }
            else if (left && newState.DPad.Up == ButtonState.Released && newState.DPad.Down == ButtonState.Released)
            {
                tmp = 6;
            }
            else if (right && newState.DPad.Up == ButtonState.Released && newState.DPad.Down == ButtonState.Released)
            {
                tmp = 2;
            }

            return tmp;
        }
        #endregion()
        #region("Move selected marble based on angle")
        private Vector2 angleChangePoint(Vector2 pointI, int angle)
        {
            point = new Vector2(pointI.X, pointI.Y);
            if (angle == 6)
            {
                point.Y--;
            }
            else if (angle == 2)
            {
                point.Y++;
            }
            else if (angle == 5)
            {
                point.X++;
                if (point.X > 4) point.Y--;
            }
            else if (angle == 3)
            {
                point.X++;
                point.Y++;
                if (point.X > 4) point.Y--;
            }
            else if (angle == 7)
            {
                point.X--;
                if (point.X < 4) point.Y--;
            }
            else if (angle == 1)
            {
                point.X--;
                point.Y++;
                if (point.X < 4) point.Y--;
            }
            return point;
        }
        #endregion()
        #endregion()
    }
}