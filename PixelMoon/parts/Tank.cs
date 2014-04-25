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

namespace PixelMoon.parts
{
    class Tank : Part
    {

        //public Int32 fuelCapacity;
        //public Int32 fuelWeight;

        public Tank(ContentLoader.TextureNames name, Rectangle startPositionPart, Int32 weightParameter, Int32 fuel, Int32 fweight)
        {
            partName = name;
            partPosition = startPositionPart;
            partTexture = ContentLoader.Textures[partName];
            partWeight = weightParameter;
            type = partType.tank;
            fuelCapacity = fuel;
            fuelWeight = fweight;

            //this._fuelCapacity = fuel;
            //this._fuelWeight = fweight;
        }
    }
}