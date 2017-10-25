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
    class TitleScreen : Screen
    {
        private SpriteFont font;
        private Texture2D onePixel;
        private Texture2D radialTex;
        private Rectangle menu;
        private Rectangle menu2;
        private Rectangle button;
        private Model marbleModel;
        private float angle;
        private String[] menuItems;
        private Vector2[] menuItemsCenter;
        private String[] playGameCheck;
        private Vector2[] playGameCheckCenter;
        private Vector2 screen;
        private float scale1, scale2;
        public TitleScreen(GraphicsDevice device, ContentManager theContent, EventHandler theScreenEvent)
            : base(device, theScreenEvent)
        {
            font = theContent.Load<SpriteFont>("Font");
            menuItems = new String[4];
            menuItemsCenter = new Vector2[4];
            screen = new Vector2();
            menuItems[0] = "Play";
            menuItems[1] = "Options";
            menuItems[2] = "Instructions";
            menuItems[3] = "Exit";
            menuItemsCenter[0] = font.MeasureString(menuItems[0]) / 2;
            menuItemsCenter[1] = font.MeasureString(menuItems[1]) / 2;
            menuItemsCenter[2] = font.MeasureString(menuItems[2]) / 2;
            menuItemsCenter[3] = font.MeasureString(menuItems[3]) / 2;
            playGameCheck = new String[2];
            playGameCheckCenter = new Vector2[2];
            playGameCheck[0] = "";
            playGameCheck[1] = "Press A to start the game.\nPress B to go back to the Main Menu.\nPress Y to go to the Options Menu.";
            playGameCheckCenter[1] = font.MeasureString(playGameCheck[1]) / 2;

            marbleModel = theContent.Load<Model>("sphere");
            onePixel = new Texture2D(device, 1, 1);
            onePixel.SetData(new[] { Color.White });

            menu = new Rectangle(device.Viewport.Width / 10, 0, (device.Viewport.Width * 4) / 5, device.Viewport.Height);
            menu2 = new Rectangle(menu.Width / 40 + menu.X, 0, (menu.Width * 19) / 20, menu.Height);
            button = new Rectangle(menu2.X, menu2.Y, menu2.Width, menu2.Height / 4);

            angle = 0;
            radialTex = new Texture2D(device, 512, 512);
            Color[] rad = new Color[512 * 512];
            Vector2 xy;
            Vector2 center = new Vector2(512 / 2, 512 / 2);
            float col;
            for (int i = 0; i < rad.Length; i++)
            {
                xy = new Vector2(i % 512, (float)Math.Floor((double)i / 512));
                col = (float)(1 - (Math.Abs(Vector2.Distance(center, xy)) / (0.5 * 512)));
                rad[i] = new Color(col, col, col, col);
            }
            radialTex.SetData(rad);
        }

        public override void Init() {
            op.Load();
            Board.SetUp(op.boardType, a, b, op.noPlayers);
            op.Save();
        }

        private GamePadState newState;
        private GamePadState oldState;
        public override void Update(GameTime theTime)
        {
            newState = GamePad.GetState(LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One));
            angle += 0.001f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            if (screen.X == 0)
            {
                if (newState.DPad.Up == ButtonState.Pressed && oldState.DPad.Up == ButtonState.Released) screen.Y--;
                if (newState.DPad.Down == ButtonState.Pressed && oldState.DPad.Down == ButtonState.Released) screen.Y++;
                if (screen.Y == -1) screen.Y = 3;
                else if (screen.Y == 4) screen.Y = 0;
                if (newState.Buttons.A == ButtonState.Pressed && oldState.Buttons.A == ButtonState.Released)
                {
                    if (screen.Y == 0)
                    {
                        screen.X = 1;
                        playGameCheck[0] = "Review the options below\n\n";
                        playGameCheck[0] += "Number of Players: "+op.noPlayers+"\n";
                        playGameCheck[0] += "Player 1: Controller "+LogicalGamer.GetPlayerIndex(LogicalGamerIndex.One)+"\n";
                        playGameCheck[0] += "Player 2: Controller "+LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Two)+"\n";
                        if (op.noPlayers == 4)
                        {
                            playGameCheck[0] += "Player 3: Controller "+LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Three)+"\n";
                            playGameCheck[0] += "Player 4: Controller "+LogicalGamer.GetPlayerIndex(LogicalGamerIndex.Four)+"\n";
                        }
                        playGameCheck[0] += "\nBoard Setup: "+op.boardTypeName();
                        playGameCheckCenter[0] = font.MeasureString(playGameCheck[0]) / 2;
                        float scaleX;
                        float scaleY;
                        scaleY = (button.Height * 3) / (playGameCheckCenter[0].Y * 2.5f);
                        scaleX = (button.Width) / (playGameCheckCenter[0].X * 2.5f);
                        if (scaleY < scaleX) scale1 = scaleY;
                        else scale1 = scaleX;
                        scaleY = button.Height / (playGameCheckCenter[1].Y * 2.5f);
                        scaleY = button.Width / (playGameCheckCenter[1].X * 2.5f);
                        if (scaleY < scaleX) scale2 = scaleY;
                        else scale2 = scaleX;
                    }
                    else if (screen.Y == 1)
                    {
                        //Options
                    }
                    else if (screen.Y == 2)
                    {
                        //Instructions
                    }
                    else if (screen.Y == 3)
                    {
                        ScreenEvent.Invoke(0, new EventArgs());
                    }
                };
            }
            else if (screen.X == 1)
            {
                if (newState.Buttons.A == ButtonState.Pressed && oldState.Buttons.A == ButtonState.Released) ScreenEvent.Invoke(2, new EventArgs()); //start game
                else if (newState.Buttons.B == ButtonState.Pressed && oldState.Buttons.B == ButtonState.Released) screen.X = 0;
                else if (newState.Buttons.Y == ButtonState.Pressed && oldState.Buttons.Y == ButtonState.Released); //options
                playGameCheck[1] = "Press A to start the game.\nPress B to go back to the Main Menu.\nPress Y to go to the Options Menu.";
            
            }
            oldState = newState;
            base.Update(theTime);
        }

        public override void Draw(GameTime gameTime)
        {
            device.Clear(Color.Transparent);
            device.BlendState = BlendState.AlphaBlend;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, op.getColorV(z.getType()), false);
            foreach (Marble[] i in b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, op.getColorV(z.getType()), false);
            
            sprBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            sprBatch.Draw(onePixel, menu, new Color(0.1f, 0.1f, 0.1f, 0.1f));
            sprBatch.Draw(onePixel, menu2, new Color(0.1f, 0.1f, 0.1f, 0.1f));
            if (screen.X == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    //sprBatch.Draw(onePixel, new Rectangle(button.X, button.Y + (i * button.Height), button.Width, button.Height), op.getColor(i+1));
                    if (screen.Y == i) sprBatch.Draw(radialTex, new Rectangle(button.X, button.Y + (i * button.Height), button.Width, button.Height), null, op.getColor(i + 1), 0, Vector2.Zero, SpriteEffects.None, 0);
                    sprBatch.DrawString(font, menuItems[i], new Vector2(device.Viewport.Width / 2, button.Y + (i * button.Height) + button.Height / 2), Color.Black, 0, menuItemsCenter[i], 1, SpriteEffects.None, 0);
                }
            }
            else if (screen.X == 1)
            {                
                sprBatch.DrawString(font, playGameCheck[0], new Vector2(device.Viewport.Width / 2, button.Y + button.Height + button.Height / 2), Color.Black, 0, playGameCheckCenter[0], scale1, SpriteEffects.None, 0);
                sprBatch.DrawString(font, playGameCheck[1], new Vector2(device.Viewport.Width / 2, button.Y + (3 * button.Height) + button.Height / 2), Color.Black, 0, playGameCheckCenter[1], scale2, SpriteEffects.None, 0);
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
                    effect.World = wvp[0] * mesh.ParentBone.Transform * Matrix.CreateTranslation(position) * Matrix.CreateRotationX(angle);
                    effect.View = wvp[1];
                    effect.Projection = wvp[2];
                }
                mesh.Draw();
            }
        }
    }
}
