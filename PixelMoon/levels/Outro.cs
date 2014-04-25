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



namespace PixelMoon
{
    class Outro
    {

        Single transparancy = 1f;
        Single transparancy1 = 1f;
        Single transparancy2 = 1f;
        Single transparancy3 = 1f;
        Single transparancy4 = 1f;
        Single transparancy5 = 1f;

        Single transparancyMoon = 1f;
        Single transparancyIncrement = 0.005f;

        Boolean end = false;
        Int32 waitingTime;

        // Touch info.
        TouchCollection currentTouches;
        Vector2 itemPosition = new Vector2(30, 250);

        public Outro()
        {

        }

        public void update(GameTime gameTime)
        {

            transparancyMoon -= transparancyIncrement;
            transparancyMoon = MathHelper.Clamp(transparancyMoon, 0, 1);

            currentTouches = TouchPanel.GetState();
            if (currentTouches.Count > 0 && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
            {
                Game1.gamestate = PixelMoon.Game1.Gamestate.menu;
            }

            if (transparancyMoon <= 0 && !end)
            {
                transparancy -= transparancyIncrement;
            }

            if(transparancy <= 0){
                transparancy1 -= transparancyIncrement;
            }

            if (transparancy1 <= 0)
            {
                transparancy2 -= transparancyIncrement;
            }

            if (transparancy2 <= 0)
            {
                transparancy3 -= transparancyIncrement;
            }

            if (transparancy3 <= 0)
            {
                transparancy4 -= transparancyIncrement;
            }

            if (transparancy4 <= 0)
            {
                transparancy5 -= transparancyIncrement;
            }

            if (transparancy5 <= 0)
            {
                transparancyMoon += transparancyIncrement;
                transparancy += transparancyIncrement;
                transparancy1 += transparancyIncrement;
                transparancy2 += transparancyIncrement;
                transparancy3 += transparancyIncrement;
                transparancy4 += transparancyIncrement;
                transparancy5 += transparancyIncrement;
                end = true;
                waitingTime = gameTime.TotalGameTime.Seconds + 10;
            }

            if (end && transparancyMoon == 0 && gameTime.TotalGameTime.Seconds >= waitingTime)
            {
                Game1.gamestate = Game1.Gamestate.menu;
            }

            transparancy = MathHelper.Clamp(transparancy, 0, 1);
            transparancy1 = MathHelper.Clamp(transparancy1, 0, 1);
            transparancy2 = MathHelper.Clamp(transparancy2, 0, 1);
            transparancy3 = MathHelper.Clamp(transparancy3, 0, 1);
            transparancy4 = MathHelper.Clamp(transparancy4, 0, 1);
            transparancy5 = MathHelper.Clamp(transparancy5, 0, 1);


        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.moonAndStar], ContentLoader.rectangles[ContentLoader.TextureNames.moonAndStar], Color.Lerp(Color.White, Color.Transparent, transparancyMoon));


            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro1], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy));
            itemPosition.Y += 90;
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro2], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy1));
            itemPosition.Y += 90;
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro3], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy2));
            itemPosition.Y += 90;
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro4], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy3));
            itemPosition.Y += 90;
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro5], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy4));
            itemPosition.Y += 90;
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.outro6], itemPosition, Color.Lerp(Color.White, Color.Transparent, transparancy5));
            itemPosition.Y = 250;
          


        }

        public void resetState(GameTime gameTime)
        {
            Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
            itemPosition.X = 30;
            itemPosition.Y = 250;
            transparancyMoon = 1f;
            transparancy = 1f;
            transparancy1 = 1f;
            transparancy2 = 1f;
            transparancy3 = 1f;
            transparancy4 = 1f;
            transparancy5 = 1f;
        }

    }
}