using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Abalone
{
    class Screen
    {
        protected EventHandler ScreenEvent;
        protected GraphicsDevice device;
        protected SpriteBatch sprBatch;
        protected static Marble[][] a;
        protected static Marble[][] b;
        protected static Options op;
        protected static Matrix[] WVP;
        protected static Vector3 cameraPosition, cameraRotation;
        protected static float aspectRatio, distance;

        public Screen(GraphicsDevice theDevice, EventHandler theScreenEvent)
        {
            device = theDevice;
            ScreenEvent = theScreenEvent;
            sprBatch = new SpriteBatch(device);
        }

        public virtual void Init()
        {
        }

        //Update any information specific to the screen
        public virtual void Update(GameTime gameTime)
        {
        }

        //Draw any objects specific to the screen
        public virtual void Draw(GameTime gameTime)
        {
        }
    }

    enum LogicalGamerIndex
    {
        One,
        Two,
        Three,
        Four
    }

    static class LogicalGamer
    {
        private static readonly PlayerIndex[] playerIndices =
        {
            PlayerIndex.One,            
            PlayerIndex.Two,
            PlayerIndex.Three,
            PlayerIndex.Four,
        };

        public static PlayerIndex GetPlayerIndex(LogicalGamerIndex index)
        {
            return playerIndices[(int)index];
        }

        public static void SetPlayerIndex(LogicalGamerIndex gamerIndex,PlayerIndex playerIndex)
        {
            playerIndices[(int)gamerIndex] = playerIndex;
        }
    }
}
