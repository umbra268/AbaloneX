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
        #region("Draw Function")
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            //skybox.Draw(WVP[1], WVP[2], cameraPosition);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            #region("Marbles")
            if (screen.Y == 0)
            {
                foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, z.color(), false);
                foreach (Marble[] i in b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, z.color(), false);
            }
            if (screen.Y != 0)
            {
                if (screen.Y == 1) foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, z.color(), z.isSelected(p[currentPlayer][0]));
                else if (screen.Y == 2) foreach (Marble[] i in b) foreach (Marble z in i) DrawModel(marbleModel, WVP, new Vector3(z.getPosition(), 0), z.color(), z.isSelected(p[currentPlayer][1]));
            }
            #endregion()
            #region("HUD")
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            foreach (Rectangle z in hud) spriteBatch.Draw(hudTexture, z, z, Color.White);
            if (screen.W != 0)
            {
                string myText = "Error:\n\n";
                if (screen.W == 1) myText += "Choose one of your Marbles.";
                if (screen.W == 2) myText += "Whatcha doin'?";
                if (screen.W == 3) myText += "You can only select up to 3 marbles.";
                if (screen.W == 4) myText += "Illegal move.";
                if (screen.W == 5) myText += "You can only select your marbles.";

                Vector2 textSize = Font.MeasureString(myText);
                Vector2 textCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                Rectangle rectA = new Rectangle((int)(textCenter.X - (textSize.X / 2) - 1), (int)(textCenter.Y - (textSize.Y / 2) - 1), (int)(textSize.X + 2), (int)(textSize.Y + 2));
                Rectangle rectB = new Rectangle((int)(textCenter.X - (textSize.X / 2) - 6), (int)(textCenter.Y - (textSize.Y / 2) - 6), (int)(textSize.X + 12), (int)(textSize.Y + 12));

                spriteBatch.Draw(msgTexture, rectB, rectB, new Color(255 / 2, 255 / 2, 255 / 2, 255 / 2));
                spriteBatch.Draw(msgTexture, rectA, rectA, new Color(255, 255, 255, 2 * (255 / 3)));
                spriteBatch.DrawString(Font, myText, textCenter - (textSize / 2), Color.Black);
            }
            else if (screen.X == 0)
            {
                //MainMenu
                spriteBatch.Draw(hudTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                String myText = "Main Menu\nPress the Start button \nTo Start a Game";
                Vector2 textSize = Font.MeasureString(myText);
                Vector2 textCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                spriteBatch.DrawString(Font, myText, textCenter - (textSize / 2), Color.Black);
            }
            else
            {
                String myText = "";
                if (screen.Y == 0 || screen.Y == 2) myText += "Press the Left Trigger to switch to Board 1.";
                if (screen.Y == 0) myText += "\n";
                if (screen.Y == 0 || screen.Y == 1) myText += "Press the Right Trigger to switch to Board 2.";
                Vector2 textSize = Font.MeasureString(myText);
                Vector2 textCenter = new Vector2(GraphicsDevice.Viewport.Width / 2, 100 / 2);
                spriteBatch.DrawString(Font, myText, textCenter - (textSize / 2), Color.Black);

                string status = "";
                if (screen.Y == 1) status = "Board1";
                if (screen.Y == 2) status = "Board2";
                if (screen.Y != 0)
                {
                    status += "\n" + p[currentPlayer][(int)(screen.Y - 1)][0];
                    status += "\n" + p[currentPlayer][(int)(screen.Y - 1)][1];
                }
                //status += "\n Screen States=" + screen + "\n CurrentPlayer=" + currentPlayer;
                status += "\n" + numOff[0] + "\n" + numOff[1];
                if (GameConstants.numPlayers == 4) status += "\n" + numOff[2] + "\n" + numOff[3];
                spriteBatch.DrawString(Font, status, Vector2.Zero, Color.Red);
            }
            spriteBatch.End();
            #endregion() */
            base.Draw(gameTime);
        }
        #endregion()
        #region("Extra Drawing Functions")
        public void DrawModel(Model model, Matrix[] wvp, Vector3 position, Vector4 color, bool selected)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.Alpha = color.W;
                    effect.DiffuseColor = new Vector3(color.X / 255, color.Y / 255, color.Z / 255);
                    if (selected == true)
                    {
                        effect.AmbientLightColor = new Vector3(color.X / 255, color.Y / 255, color.Z / 255);
                    }
                    effect.World = wvp[0] * mesh.ParentBone.Transform * Matrix.CreateTranslation(position) * Matrix.CreateRotationX(angle);
                    effect.View = wvp[1];
                    effect.Projection = wvp[2];
                }
                mesh.Draw();
            }
        }
        #endregion()
    }
}
