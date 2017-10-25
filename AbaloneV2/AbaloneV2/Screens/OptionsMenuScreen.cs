#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System.IO;
#endregion

namespace AbaloneV2
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry playersMenuEntry;
        MenuEntry boardTypeMenuEntry;

        static string[] players = { "2 Players", "4 Players" };
        static int currentNoPlayers = 0;

        static string[] boardType = { "Standard", "German Daisy" };
        static int currentBoardType = 0;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.\
            playersMenuEntry = new MenuEntry(string.Empty);
            boardTypeMenuEntry = new MenuEntry(string.Empty);

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            playersMenuEntry.Selected += playersMenuEntrySelected;
            boardTypeMenuEntry.Selected += boardTypeMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(playersMenuEntry);
            MenuEntries.Add(boardTypeMenuEntry);
            MenuEntries.Add(back);

            if (Global.SaveDevice.FileExists(Global.containerName, Global.fileName_options))
            {
                Global.SaveDevice.Load(
                    Global.containerName,
                    Global.fileName_options,
                    stream =>
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            currentNoPlayers = int.Parse(reader.ReadLine());
                            currentBoardType = int.Parse(reader.ReadLine());
                        }
                    });
            }
            SetMenuEntryText();
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            playersMenuEntry.Text = "Number of Players: " + players[currentNoPlayers];
            boardTypeMenuEntry.Text = "Board Setup: " + boardType[currentBoardType];
        }


        #endregion

        #region Handle Input
        /// <summary>
        /// Event handler for when the Number of Players menu entry is selected.
        /// </summary>
        void playersMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentNoPlayers = (currentNoPlayers + 1) % players.Length;
            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Board Type menu entry is selected.
        /// </summary>
        void boardTypeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentBoardType = (currentBoardType + 1) % boardType.Length;
            SetMenuEntryText();
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            // make sure the device is ready
            if (Global.SaveDevice.IsReady)
            {
                // save a file asynchronously. this will trigger IsBusy to return true
                // for the duration of the save process.
                Global.SaveDevice.SaveAsync(
                    Global.containerName,
                    Global.fileName_options,
                    stream =>
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(currentNoPlayers);
                            writer.WriteLine(currentBoardType);
                        }
                    });
            }
            base.OnCancel(playerIndex);
        }
        #endregion
    }
}
