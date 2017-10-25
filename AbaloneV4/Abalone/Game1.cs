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

namespace Abalone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sprBatch;

        Marble[][] a;
        Marble[][] b;
        Options op;
        Model marble;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);            
            Content.RootDirectory = "Content";
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            a = new Marble[9][];
            b = new Marble[9][];
            op = new Options();

            for (int i = 0; i < 4; i++)
            {
                a[i] = new Marble[i + 5];
                b[i] = new Marble[i + 5];
                a[8 - i] = new Marble[i + 5];
                b[8 - i] = new Marble[i + 5];
            }
            a[4] = new Marble[9];
            b[4] = a[4];
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            marble = Content.Load<Model>("sphere");
            sprBatch = new SpriteBatch(GraphicsDevice);
            SCREEN_MANAGER.add_screen(new MenuScreen(GraphicsDevice, sprBatch, ref op, ref a, ref b, ref marble));
            //SCREEN_MANAGER.add_screen(new GameScreen(GraphicsDevice, sprBatch, ref op, ref a, ref b, ref marble));
            //SCREEN_MANAGER.add_screen(new OptionScreen(GraphicsDevice));
            //SCREEN_MANAGER.add_screen(new GameScreen(GraphicsDevice));
            SCREEN_MANAGER.goto_screen("Menu");

            SCREEN_MANAGER.Init();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();
            SCREEN_MANAGER.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SCREEN_MANAGER.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}