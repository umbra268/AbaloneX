using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Abalone
{
    class GameScreen : Screen
    {
        private Model marbleModel;
        private SpriteFont Font;
        private float angle;
        Vector4 screen;
        int currentPlayer, angle2;
        GamePadState[] newState, oldState;
        Vector2[][][] p;
        int[] numOff;
        Vector2 point;
        public GameScreen(GraphicsDevice device, ContentManager theContent, EventHandler theScreenEvent)
            : base(device, theScreenEvent)
        {
            marbleModel = theContent.Load<Model>("sphere");
            Font = theContent.Load<SpriteFont>("FontSmall");
            angle = 0;
            screen = Vector4.Zero;
            newState = new GamePadState[op.noPlayers];
            oldState = new GamePadState[op.noPlayers];
            numOff = new int[op.noPlayers];
            screen.X = 1;
            currentPlayer = 0;
        }

        public override void Init() {
            Board.PointSetUp(op.noPlayers, op.boardType, ref p);
        }

        public override void Update(GameTime theTime)
        {
            newState[0] = GamePad.GetState(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One));
            newState[1] = GamePad.GetState(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Two));
            if (op.noPlayers == 4)
            {
                newState[2] = GamePad.GetState(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Three));
                newState[3] = GamePad.GetState(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Four));
            }

            if (screen.X == 0 || screen.Y == 0)
            {
                angle += 0.001f;
                cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
                WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            }

            if (screen.W != 0)
            {
                if (newState[currentPlayer].Buttons.B == ButtonState.Pressed) screen.W = 0;
            }
            else if (screen.X == 1)
            {
                if (screen.Z == 1)
                {
                    bool error = false;
                    if (screen.Y == 1 && a[(int)(p[currentPlayer][(int)(screen.Y - 1)][0].X)][(int)(p[currentPlayer][(int)(screen.Y - 1)][0].Y)].getType() != currentPlayer + 1) error = true;
                    else if (screen.Y == 2 && b[(int)(p[currentPlayer][(int)(screen.Y - 1)][0].X)][(int)(p[currentPlayer][(int)(screen.Y - 1)][0].Y)].getType() != currentPlayer + 1) error = true;
                    if (error == true)
                    {
                        screen.W = 1;
                        screen.Z = 0;
                    }
                    else screen.Z = 2;
                }
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
                else if (screen.Z == 4)
                {

                }
                else
                {
                    if (screen.Y != 0)
                    {
                        playersMove(newState[currentPlayer], oldState[currentPlayer]);
                        if (newState[currentPlayer].Buttons.A == ButtonState.Pressed && oldState[currentPlayer].Buttons.A == ButtonState.Released) screen.Z = 1;
                    }
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
                }
            }
            oldState = newState.ToArray();
            base.Update(theTime);
        }

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
            if (screen.Y == 1 && a[(int)(p.X)][(int)(p.Y)].getType() == type + 1) return true;
            else if (screen.Y == 2 && b[(int)(p.X)][(int)(p.Y)].getType() == type + 1) return true;
            else return false;
        }

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

        public override void Draw(GameTime gameTime)
        {
            device.Clear(op.getColor(currentPlayer+1)*0.7f);
            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.Default;

            if (screen.Y == 0 && screen.W == 0)
            {
                foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, op.getColorV(z.getType()), false);
                foreach (Marble[] i in b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, op.getColorV(z.getType()), false);
            }
            if (screen.Y != 0 && screen.W == 0)
            {
                if (screen.Y == 1) foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, op.getColorV(z.getType()), z.isSelected(p[currentPlayer][0]));
                else if (screen.Y == 2) foreach (Marble[] i in b) foreach (Marble z in i) DrawModel(marbleModel, WVP, new Vector3(z.getPosition(), 0), op.getColorV(z.getType()), z.isSelected(p[currentPlayer][1]));
            }

            sprBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            if (screen.W != 0)
            {
                //ErrorScreen
            }
            else
            {
                String myText = "";
                if (screen.Y == 0 || screen.Y == 2) myText += "Press the Left Trigger to switch to Board 1.";
                if (screen.Y == 0) myText += "\n";
                if (screen.Y == 0 || screen.Y == 1) myText += "Press the Right Trigger to switch to Board 2.";
                Vector2 textSize = Font.MeasureString(myText);
                Vector2 textCenter = new Vector2(device.Viewport.Width / 2, 100 / 2);
                sprBatch.DrawString(Font, myText, textCenter - (textSize / 2), Color.Black);

                string status = "";
                if (screen.Y == 1) status = "Board1";
                if (screen.Y == 2) status = "Board2";
                if (screen.Y != 0)
                {
                    status += "\n" + p[currentPlayer][(int)(screen.Y - 1)][0];
                    status += "\n" + p[currentPlayer][(int)(screen.Y - 1)][1];
                }
                status += "\n Screen States=" + screen + "\n CurrentPlayer=" + currentPlayer + "\n CurrentController=" + LogicalGamer.GetPlayerIndex((LogicalGamerIndex) currentPlayer);
                status += "\n" + numOff[0] + "\n" + numOff[1];
                if (op.noPlayers == 4) status += "\n" + numOff[2] + "\n" + numOff[3];
                sprBatch.DrawString(Font, status, Vector2.Zero, Color.Red);
            }
            sprBatch.End();


            base.Draw(gameTime);
        }

        private void DrawModel(Model model, Matrix[] wvp, Vector3 position, Vector4 color, bool selected)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.Alpha = color.W;
                    effect.DiffuseColor = new Vector3(color.X, color.Y, color.Z);
                    if (selected == true)
                    {
                        effect.AmbientLightColor = new Vector3(color.X, color.Y, color.Z);
                    }
                    effect.World = wvp[0] * mesh.ParentBone.Transform * Matrix.CreateTranslation(position) * Matrix.CreateRotationX(angle);
                    effect.View = wvp[1];
                    effect.Projection = wvp[2];
                }
                mesh.Draw();
            }
        }
    }
}
