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



namespace PixelMoon.levels
{
    class Intro
    {

        Single transparancy = 1f;
        Single transparancy1 = 1f;
        Single transparancy2 = 1f;
        Single transparancyIncrement = 0.01f;
        
        // Touch info.
        TouchCollection currentTouches;
        


        public Intro()
        {
            
        }

        public void update(GameTime gameTime)
        {

            currentTouches = TouchPanel.GetState();
            if (currentTouches.Count > 0)
            {
                Game1.gamestate = PixelMoon.Game1.Gamestate.menu;
            }


            if (gameTime.TotalGameTime.Seconds > 2 && gameTime.TotalGameTime.Seconds < 5)
            {
                transparancy -= transparancyIncrement;
            }

            if (gameTime.TotalGameTime.Seconds > 5 && gameTime.TotalGameTime.Seconds < 8)
            {
                transparancy = 0f;
                transparancy1 -= transparancyIncrement;
            }

            if (gameTime.TotalGameTime.Seconds > 8 && gameTime.TotalGameTime.Seconds < 11)
            {
                transparancy = 0f;
                transparancy1 = 0f;
                transparancy2 -= transparancyIncrement;

                if (transparancy2 < 0)
                {
                    transparancy2 = 0f;
                }
            }

            if(gameTime.TotalGameTime.Seconds > 11){

                // Fade everything out.

                transparancy += transparancyIncrement;
                transparancy1 += transparancyIncrement;
                transparancy2 += transparancyIncrement;

                if (transparancy > 1 && transparancy1 > 1 && transparancy2 > 1)
                {
                    if (gameTime.TotalGameTime.Seconds > 14)
                    {
                        resetState(gameTime);
                        Game1.gamestate = PixelMoon.Game1.Gamestate.menu;
                    }
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            transparancy = MathHelper.Clamp(transparancy, 0, 1);
            transparancy1 = MathHelper.Clamp(transparancy1, 0, 1);
            transparancy2 = MathHelper.Clamp(transparancy2, 0, 1);

            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.LoadingScreenBG], ContentLoader.rectangles[ContentLoader.TextureNames.LoadingScreenBG], Color.White);

            
            //spriteBatch.DrawString(font, "MOON IMAGE", new Vector2(200, 200), Color.Lerp(Color.White, Color.Transparent, transparancy2));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.moonAndStar], ContentLoader.rectangles[ContentLoader.TextureNames.moonAndStar], Color.Lerp(Color.White, Color.Transparent, transparancy2));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.reachingHand], ContentLoader.rectangles[ContentLoader.TextureNames.reachingHand], Color.Lerp(Color.White, Color.Transparent, transparancy2));

            //spriteBatch.DrawString(font, "\"All I can dream of...\"", new Vector2(80, 242), Color.Lerp(Color., Color.Transparent, transparancy));
            //spriteBatch.DrawString(font, "\"Is to reach the Moon...\"", new Vector2(23, 290), Color.Lerp(Color.White, Color.Transparent, transparancy1));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.text_AllIDreamOff], ContentLoader.rectangles[ContentLoader.TextureNames.text_AllIDreamOff], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.text_IsToReachTheMoon], ContentLoader.rectangles[ContentLoader.TextureNames.text_IsToReachTheMoon], Color.Lerp(Color.White, Color.Transparent, transparancy1));
            


        }

        public void resetState(GameTime gameTime)
        {
            Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
            transparancy = 1f;
        }

    }
}