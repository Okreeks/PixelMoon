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

namespace PixelMoon.levels
{
    class Options
    {
        Single transparancy = 1f;
        Single transparancyIncrement = 0.01f;
        Int32 pressed;

        // Touch info.
        TouchCollection currentTouches;

        public Options()
        {

        }

        public void update(GameTime gameTime)
        {

            

            currentTouches = TouchPanel.GetState();
            if (currentTouches.Count > 0 && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
            {
                Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds + 1);
                resetState();
                Game1.gamestate = PixelMoon.Game1.Gamestate.menu;
            }

            transparancy -= transparancyIncrement;
            transparancy = MathHelper.Clamp(transparancy, 0, 1);
            
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, "I didnt dream about any other option...", new Vector2(50, 100), Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.DrawString(font, "Then going to the moon...", new Vector2(50, 200), Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.DrawString(font, "MOON IMAGE", new Vector2(50, 300), Color.Lerp(Color.White, Color.Transparent, transparancy));
        }

        public void resetState()
        {
            transparancy = 1f;
        }

    }
}