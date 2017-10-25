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
using EasyStorage;

namespace Abalone
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SplashScreen splashScreen;
        TitleScreen titleScreen;
        GameScreen gameScreen;
        Screen currentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 800;
            //graphics.ToggleFullScreen();
            //graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Board.init();
            splashScreen = new SplashScreen(GraphicsDevice, this.Content, new EventHandler(screenEvent));
            titleScreen = new TitleScreen(GraphicsDevice, this.Content, new EventHandler(screenEvent));
            gameScreen = new GameScreen(GraphicsDevice, this.Content, new EventHandler(screenEvent));
            currentScreen = splashScreen;
        }

        public void screenEvent(object obj, EventArgs e)
        {
            if ((int)obj == 0) this.Exit();
            else if ((int)obj == 1) PromptMe();
            else if ((int)obj == 2)
            {
                currentScreen = gameScreen;
                currentScreen.Init();
            }
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
            currentScreen.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            currentScreen.Draw(gameTime);
            base.Draw(gameTime);
        }
        private void PromptMe()
        {
            // we can set our supported languages explicitly or we can allow the
            // game to support all the languages. the first language given will
            // be the default if the current language is not one of the supported
            // languages. this only affects the text found in message boxes shown
            // by EasyStorage and does not have any affect on the rest of the game.
            EasyStorageSettings.SetSupportedLanguages(Language.English);
            // on Windows Phone we use a save device that uses IsolatedStorage
            // on Windows and Xbox 360, we use a save device that gets a
            //shared StorageDevice to handle our file IO.
            // create and add our SaveDevice
            SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();
            this.Components.Add(sharedSaveDevice);
            // make sure we hold on to the device
            Global.SaveDevice = sharedSaveDevice;
            // hook two event handlers to force the user to choose a new device if they cancel the
            // device selector or if they disconnect the storage device after selecting it
            sharedSaveDevice.DeviceSelectorCanceled +=
                (s, e) => e.Response = SaveDeviceEventResponse.Force;
            sharedSaveDevice.DeviceDisconnected +=
                (s, e) => e.Response = SaveDeviceEventResponse.Force;
            // prompt for a device on the first Update we can
            sharedSaveDevice.PromptForDevice();
            sharedSaveDevice.DeviceSelected += (s, e) =>
            {
                currentScreen = titleScreen;
                currentScreen.Init();
            };
#if XBOX
            // add the GamerServicesComponent
            this.Components.Add(
                new Microsoft.Xna.Framework.GamerServices.GamerServicesComponent(ScreenManager.Game));
#endif
        }
    }
}
