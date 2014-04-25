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

using PixelMoon.parts;

namespace PixelMoon
{
    public class Capsule : Part
    {


        //public Int32 electricityCapacity;
        //public Int32 oxygenCapacity;

        public Capsule(ContentLoader.TextureNames name, Rectangle startPositionPart, Int32 weightParameter)
        {
            partName = name;
            partPosition = startPositionPart;
            partTexture = ContentLoader.Textures[partName];
            partWeight = weightParameter;
            type = partType.capsule;
            //electricityCapacity = elec;
            //oxygenCapacity = oxy;
        }
    }
}