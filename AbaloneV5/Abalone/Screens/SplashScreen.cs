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
    class SplashScreen : Screen
    {
        private SpriteFont font;
        private String text;
        private Vector2 textCenter;
        public SplashScreen(GraphicsDevice device, ContentManager theContent, EventHandler theScreenEvent)
            : base(device, theScreenEvent)
        {
            font = theContent.Load<SpriteFont>("Font");
            text = "Press Start";
            Vector2 textSize = font.MeasureString(text);
            textCenter = new Vector2(device.Viewport.Width / 2, device.Viewport.Height / 2);
            textCenter -= textSize / 2;
            WVP = new Matrix[3];
            aspectRatio = device.Viewport.AspectRatio;
            distance = 30;
            cameraPosition = distance * Vector3.UnitZ;
            cameraRotation = new Vector3(0, 1, 1);
            WVP[0] = Matrix.Identity;
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            WVP[2] = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(55), aspectRatio, 0.1f, 100f);

            op = new Options();
            a = new Marble[9][];
            b = new Marble[9][];
            for (int i = 0; i < 4; i++)
            {
                a[i] = new Marble[i + 5];
                b[i] = new Marble[i + 5];
                a[8 - i] = new Marble[i + 5];
                b[8 - i] = new Marble[i + 5];
            }
            a[4] = new Marble[9];
            b[4] = a[4];
            for (int i = 0; i < 4; i++)
            {
                for (int z = 0; z < i + 5; z++)
                {
                    a[i][z] = new Marble(true, i, z);
                    a[8 - i][z] = new Marble(true, 8 - i, z);

                    b[i][z] = new Marble(false, i, z);
                    b[8 - i][z] = new Marble(false, 8 - i, z);
                }
            }
            for (int z = 0; z < 9; z++)
            {
                a[4][z] = new Marble(true, 4, z);
            }
            Board.SetUp(op.boardType, a, b, op.noPlayers);
        }

        public override void Update(GameTime theTime)
        {
            for (int i = 0; i < 4;i++)
            {
                if (GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Pressed)
                {
                    LogicalGamer.SetPlayerIndex(LogicalGamerIndex.One, (PlayerIndex)0);
                    LogicalGamer.SetPlayerIndex(LogicalGamerIndex.Two, (PlayerIndex)1);
                    LogicalGamer.SetPlayerIndex(LogicalGamerIndex.Three, (PlayerIndex)0);
                    LogicalGamer.SetPlayerIndex(LogicalGamerIndex.Four, (PlayerIndex)1);
                    ScreenEvent.Invoke(1, new EventArgs());
                    return;
                }
            }          
            base.Update(theTime);
        }

        public override void Draw(GameTime gameTime)
        {
            device.Clear(Color.Transparent);
            sprBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sprBatch.DrawString(font, text, textCenter, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            sprBatch.End();
            base.Draw(gameTime);
        }
    }
}
