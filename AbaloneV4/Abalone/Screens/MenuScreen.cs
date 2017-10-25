using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Abalone
{
    class MenuScreen : Screen
    {
        public MenuScreen(GraphicsDevice device, SpriteBatch sprBatch, ref Options op, ref Marble[][] a, ref Marble[][] b, ref Model marble)
            : base("Menu", device, sprBatch, op, a, b, marble)
        {
            marbleFunctions.marbleInit(ref a, ref b);
            marbleFunctions.boardSetup(ref a, ref b, ref op);
        }

        Matrix[] WVP;
        Vector3 cameraPosition, cameraRotation;
        float angle, aspectRatio, distance;
        Model marbleModel;
        Texture2D onePixel;
        Texture2D radialTex;
        Rectangle menu;
        Rectangle menu2;
        Rectangle button;
        public override bool Init()
        {
            _device.BlendState = BlendState.AlphaBlend;
            WVP = new Matrix[3];
            aspectRatio = _device.Viewport.AspectRatio;
            angle = 0;
            distance = 30;
            cameraPosition = distance * Vector3.UnitZ;
            cameraRotation = new Vector3(0, 1, 1);
            WVP[0] = Matrix.Identity;
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            WVP[2] = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(55), aspectRatio, 0.1f, 100f);

            onePixel = new Texture2D(_device, 1, 1);
            onePixel.SetData(new[] { Color.White });

            menu = new Rectangle(_device.Viewport.Width / 10, _device.Viewport.Height / 10, (_device.Viewport.Width * 4) / 5, (_device.Viewport.Height * 4) / 5);
            menu2 = new Rectangle(menu.Width / 40 + menu.X, menu.Height / 40 + menu.Y, (menu.Width * 19) / 20, (menu.Height * 19) / 20);
            button = new Rectangle(menu2.X, menu2.Y, menu2.Width, menu2.Height/4);

            radialTex = new Texture2D(_device, 512, 512);
            Color[] rad = new Color[512 * 512];
            Vector2 xy;
            Vector2 center = new Vector2(512 / 2, 512 / 2);
            float col;
            for (int i = 0; i < rad.Length; i++)
            {
                xy = new Vector2(i % 512, (float)Math.Floor((double)i/512));
                col = (float) (1 - (Math.Abs(Vector2.Distance(center, xy)) / (0.5 * 512)));
                rad[i] = new Color(col, col, col, col);
            }
            radialTex.SetData(rad);

            return base.Init();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Draw(GameTime gameTime)
        {
            _device.Clear(Color.Transparent);            
            //foreach (Marble[] i in _a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, _op.getColorAlpha(z.getType()), false);
            //foreach (Marble[] i in _b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, _op.getColorAlpha(z.getType()), false);
            _sprBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            _sprBatch.Draw(radialTex, _device.Viewport.Bounds, Color.DarkOrange);
            _sprBatch.Draw(onePixel, menu, new Color(0.1f, 0.1f, 0.1f, 0.1f));
            _sprBatch.Draw(onePixel, menu2, new Color(0.1f, 0.1f, 0.1f, 0.1f));
            for(int i=0;i<4;i++) {
                _sprBatch.Draw(onePixel, new Rectangle(button.X, button.Y + (i * button.Height), button.Width, button.Height), _op.getColorAlpha(i + 1));
            }
            
            //draw menu-like stuff
            _sprBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            angle += 0.001f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            if(GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed) SCREEN_MANAGER.goto_screen("Game");
            base.Update(gameTime);
        }

        public void DrawModel(Model model, Matrix[] wvp, Vector3 position, Color color, bool selected)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.Alpha = color.A;
                    effect.DiffuseColor = new Vector3(color.R, color.G, color.B);
                    if (selected == true)
                    {
                        effect.AmbientLightColor = new Vector3(color.R, color.G, color.B);
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
