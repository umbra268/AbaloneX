using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace Abalone
{
    public class Options
    {
        //static string[] players = { "2 Players", "4 Players" };
        public int noPlayers = 4;

        private static string[] boardT = { "Standard", "German Daisy" };
        public int boardType = 0;

        public int saved = 0;

        private Color[] ColorA = null;

        private Vector4[] ColorVA = null;
        
        public Options()
        {            
            boardType = 0;
            noPlayers = 4;
            ColorA = new Color[5];
            ColorVA = new Vector4[5];
            ColorA[0] = Color.FromNonPremultiplied(255, 255, 255, 255);
            ColorA[1] = Color.FromNonPremultiplied(255, 30, 0, 255);
            ColorA[2] = Color.FromNonPremultiplied(0, 191, 50, 255);
            ColorA[3] = Color.FromNonPremultiplied(66, 18, 175, 255);
            ColorA[4] = Color.FromNonPremultiplied(255, 218, 0, 255);
            ColorVA[0] = new Vector4(255, 255, 255, 0.3f * 255) / 255;
            ColorVA[1] = new Vector4(255, 30, 0, 0.8f * 255) / 255;
            ColorVA[2] = new Vector4(0, 191, 50, 0.8f * 255) / 255;
            ColorVA[3] = new Vector4(66, 18, 175, 0.8f * 255) / 255;
            ColorVA[4] = new Vector4(255, 218, 0, 0.8f * 255)/255;
            saved = 0;
            //load game options
        }

        public Color getColor(int type)
        {
            return ColorA[type];
        }

        public Vector4 getColorV(int type)
        {
            return ColorVA[type];
        }

        public String boardTypeName()
        {
            return boardT[boardType];
        }

        internal void Load()
        {
            if (Global.SaveDevice.FileExists(Global.containerName, Global.fileName_options))
            {
                Global.SaveDevice.Load(
                    Global.containerName,
                    Global.fileName_options,
                    stream =>
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            boardType = int.Parse(reader.ReadLine());
                            noPlayers = int.Parse(reader.ReadLine());
                            saved = int.Parse(reader.ReadLine());
                        }
                    });
            }
        }

        internal void Save()
        {
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
                            writer.WriteLine(boardType);
                            writer.WriteLine(noPlayers);
                            writer.WriteLine(saved);
                        }
                    });
            }
        }
    }
}
