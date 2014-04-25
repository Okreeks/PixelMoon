using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using PixelMoon.levels;
using PixelMoon.parts;

namespace PixelMoon.parts
{
    public abstract class Part
    {

        public ContentLoader.TextureNames partName;
        public Rectangle partPosition;
        public Int32 partWeight;
        public Texture2D partTexture;
        public partType type;
        public Int32 stageNumber;
        

        public enum partType
        {
            capsule,
            tank,
            engine
        }

        // Capsule vars
        public Int32 electricityCapacity;
        public Int32 electricityLevel;
        public Int32 oxygenCapacity;
        public Int32 oxygenLevel;

        // Tank vars
        public Int32 fuelCapacity;
        public Int32 fuelLevel;
        public Int32 fuelWeight;

        // Engine vars
        public Int32 thrust;
        public Int32 thrustLevel;
        public Int32 efficiency;
        public Int32 consumption;
        public Int32 ISP;

        public Part()
        {
           
        }
    }
}
