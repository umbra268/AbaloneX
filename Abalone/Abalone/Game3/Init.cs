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
        #region("Vars")
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font;
        Model marbleModel;
        //Skybox skybox;
        TextureCube skyboxTexture;
        Texture2D hudTexture, msgTexture;
        Rectangle[] hud;

        Marble[][] a;
        Marble[][] b;
        Vector2[][][] p;
        Matrix[] WVP;
        Vector3 cameraPosition, cameraRotation;
        float angle, aspectRatio, distance;
        int currentPlayer, angle2;

        Vector4 screen;
        GamePadState[] newState, oldState;
        int[] numOff;
        Vector2 point;
        #endregion()
        public Game3()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //this.graphics.PreferredBackBufferWidth = 1280;
            //this.graphics.PreferredBackBufferHeight = 720;
        }
        #region("Init")
        protected override void Initialize()
        {
            //graphics.ToggleFullScreen();
            //graphics.ApplyChanges();
            #region("3D Init")
            WVP = new Matrix[3];
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            angle = 0;
            distance = 30;
            cameraPosition = distance * Vector3.UnitZ;
            cameraRotation = new Vector3(0, 1, 1);
            WVP[0] = Matrix.Identity;
            WVP[1] = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.UnitY);
            WVP[2] = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(55), aspectRatio, 0.1f, 100f);
            #endregion()
            #region("HUD Init")
            hud = new Rectangle[4];
            hud[0] = new Rectangle(0, 0, 150, GraphicsDevice.Viewport.Height);
            hud[1] = new Rectangle(150, 0, GraphicsDevice.Viewport.Width - 300, 100);
            hud[2] = new Rectangle(GraphicsDevice.Viewport.Width - 150, 0, 150, GraphicsDevice.Viewport.Height);
            hud[3] = new Rectangle(150, GraphicsDevice.Viewport.Height - 100, GraphicsDevice.Viewport.Width - 300, 100);
            #endregion()
            #region("MarbleInit")
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
            changeMarbles(1);
            #endregion()
            screen = Vector4.Zero;
            newState = new GamePadState[GameConstants.numPlayers];
            oldState = new GamePadState[GameConstants.numPlayers];
            numOff = new int[GameConstants.numPlayers];
            base.Initialize();
        }
        #endregion()
        #region("Load & Unload Content")
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            marbleModel = Content.Load<Model>("sphere");
            //Font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion()

        #region("Marble Placement")
        protected void changeMarbles(int boardType)
        {
            foreach (Marble[] i in a) foreach (Marble z in i) z.changeType(0);
            foreach (Marble[] i in b) foreach (Marble z in i) z.changeType(0);

            if (GameConstants.boardType == 1) Board1();
            else if (GameConstants.boardType == 2) Board2();
        }
        #endregion()        
    }
}
