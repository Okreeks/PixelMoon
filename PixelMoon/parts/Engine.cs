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
    class Engine : Part
    {


        //public Int32 efficiency;
        //public Int32 consumption;

        public Engine(ContentLoader.TextureNames name, Rectangle startPositionPart, Int32 weightParameter, Int32 eff, Int32 consump, Int32 thr, Int32 isp)
        {
            partName = name;
            partPosition = startPositionPart;
            partTexture = ContentLoader.Textures[partName];
            partWeight = weightParameter;
            type = partType.engine;
            thrust = thr;
            efficiency = eff;
            consumption = consump;
            ISP = isp;
        }
    }
}