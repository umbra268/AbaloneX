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
        public MenuScreen(GraphicsDevice device, ref Marble[][] a, ref Marble[][] b, ref Options op, ref Model MarbleModel)
            : base(device, "Menu", a, b)
        {
            marbleModel = MarbleModel;
            for (int i = 0; i < 4; i++)
            {
                for (int z = 0; z < i + 5; z++)
                {
                    _a[i][z] = new Marble(true, i, z, ref op);
                    _a[8 - i][z] = new Marble(true, 8 - i, z, ref op);

                    _b[i][z] = new Marble(false, i, z, ref op);
                    _b[8 - i][z] = new Marble(false, 8 - i, z, ref op);
                }

            }
            for (int z = 0; z < 9; z++)
            {
                _a[4][z] = new Marble(true, 4, z, ref op);
            }

            foreach (Marble[] i in a) foreach (Marble z in i) z.changeType(0);
            foreach (Marble[] i in b) foreach (Marble z in i) z.changeType(0);

            if (op.currentBoardType == 0) Board1();
            else if (op.currentBoardType == 1) Board2();
        }

        Matrix[] WVP;
        Vector3 cameraPosition, cameraRotation;
        float angle, aspectRatio, distance;
        Model marbleModel;
        public override bool Init()
        {
            WVP = new Matrix[3];
            aspectRatio = _device.Viewport.AspectRatio;
            angle = 0;
            distance = 30;
            cameraPosition = distance * Vector3.UnitZ;
            cameraRotation = new Vector3(0, 1, 1);
            WVP[0] = Matrix.Identity;
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            WVP[2] = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(55), aspectRatio, 0.1f, 100f);
            
            return base.Init();
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public override void Draw(GameTime gameTime)
        {
            _device.Clear(Color.Transparent);
            _device.BlendState = BlendState.AlphaBlend;
            foreach (Marble[] i in _a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, z.color(), false);
            foreach (Marble[] i in _b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, z.color(), false);
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

        protected void Board1()
        {
            foreach (Marble i in _a[0]) i.changeType(1);
            foreach (Marble i in _a[1]) i.changeType(1);
            _a[2][2].changeType(1);
            _a[2][3].changeType(1);
            _a[2][4].changeType(1);

            foreach (Marble i in _a[8]) i.changeType(2);
            foreach (Marble i in _a[7]) i.changeType(2);
            _a[6][2].changeType(2);
            _a[6][3].changeType(2);
            _a[6][4].changeType(2);

            foreach (Marble i in _b[0]) i.changeType(3);
            foreach (Marble i in _b[1]) i.changeType(3);
            _b[2][2].changeType(3);
            _b[2][3].changeType(3);
            _b[2][4].changeType(3);

            foreach (Marble i in _b[8]) i.changeType(4);
            foreach (Marble i in _b[7]) i.changeType(4);
            _b[6][2].changeType(4);
            _b[6][3].changeType(4);
            _b[6][4].changeType(4);
        }

        protected void Board2()
        {
            for (int i = 0; i < 2; i++)
            {
                _a[1][i].changeType(1);
                _a[1][4 + i].changeType(2);
                _a[3][1 + i].changeType(1);
                _a[3][5 + i].changeType(2);

                _a[7][i].changeType(4);
                _a[7][4 + i].changeType(3);
                _a[5][1 + i].changeType(4);
                _a[5][5 + i].changeType(3);


                _b[1][i].changeType(3);
                _b[1][4 + i].changeType(4);
                _b[3][1 + i].changeType(3);
                _b[3][5 + i].changeType(4);

                _b[7][i].changeType(2);
                _b[7][4 + i].changeType(1);
                _b[5][1 + i].changeType(2);
                _b[5][5 + i].changeType(1);
            }
            for (int i = 0; i < 2; i++)
            {
                _a[2][0 + (4 * i)].changeType(1 + i);
                _a[2][1 + (4 * i)].changeType(4 - i);
                _a[2][2 + (4 * i)].changeType(1 + i);

                _b[2][0 + (4 * i)].changeType(3 + i);
                _b[2][1 + (4 * i)].changeType(2 - i);
                _b[2][2 + (4 * i)].changeType(3 + i);

                _a[6][0 + (4 * i)].changeType(4 - i);
                _a[6][1 + (4 * i)].changeType(1 + i);
                _a[6][2 + (4 * i)].changeType(4 - i);

                _b[6][0 + (4 * i)].changeType(2 - i);
                _b[6][1 + (4 * i)].changeType(3 + i);
                _b[6][2 + (4 * i)].changeType(2 - i);
            }

        }
    }
}
