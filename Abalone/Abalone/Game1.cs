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
using System.Diagnostics;

namespace Abalone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix[] WVP;
        Vector3 cameraPosition, cameraRotation;
        float angle, aspectRatio, distance;
        Model marbleModel;
        Marble[][] a;
        Marble[][] b;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {            
            WVP = new Matrix[3];
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            angle = 0;
            distance = 30;
            cameraPosition = distance * Vector3.UnitZ;
            cameraRotation = new Vector3(0, 1, 1);
            WVP[0] = Matrix.Identity;
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            WVP[2] = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(55), aspectRatio, 0.1f, 100f);

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

            foreach (Marble[] i in a) foreach (Marble z in i) z.changeType(0);
            foreach (Marble[] i in b) foreach (Marble z in i) z.changeType(0);

            foreach (Marble i in a[0]) i.changeType(1);
            foreach (Marble i in a[1]) i.changeType(1);
            a[2][2].changeType(1);
            a[2][3].changeType(1);
            a[2][4].changeType(1);

            foreach (Marble i in a[8]) i.changeType(2);
            foreach (Marble i in a[7]) i.changeType(2);
            a[6][2].changeType(2);
            a[6][3].changeType(2);
            a[6][4].changeType(2);

            foreach (Marble i in b[0]) i.changeType(3);
            foreach (Marble i in b[1]) i.changeType(3);
            b[2][2].changeType(3);
            b[2][3].changeType(3);
            b[2][4].changeType(3);

            foreach (Marble i in b[8]) i.changeType(4);
            foreach (Marble i in b[7]) i.changeType(4);
            b[6][2].changeType(4);
            b[6][3].changeType(4);
            b[6][4].changeType(4);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            marbleModel = Content.Load<Model>("sphere");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            angle += 0.001f;
            cameraPosition = distance * new Vector3((float)Math.Sin(angle), 0, (float)Math.Cos(angle));
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            foreach (Marble[] i in a) foreach (Marble z in i) DrawModel(marbleModel, WVP, z.position, z.color(), false);
            foreach (Marble[] i in b) foreach (Marble z in i) if (!(z.getPosition().Y == 0)) DrawModel(marbleModel, WVP, z.position, z.color(), false);

            base.Draw(gameTime);
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
                    effect.World = wvp[0] * mesh.ParentBone.Transform * Matrix.CreateTranslation(position) * Matrix.CreateRotationX(angle);
                    effect.View = wvp[1];
                    effect.Projection = wvp[2];
                }
                mesh.Draw();
            }
        }
    }
}
