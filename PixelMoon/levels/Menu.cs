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
    class Menu
    {

        Single transparancy = 1f;
        Single transparancyIncrement = 0.01f;
        TouchCollection currentTouches;
        Point touchPoints = new Point(0, 0);

        // Cloud info.
        Single cloudSpeed = 0.35f;
        Vector2 cloud1Location;
        Vector2 cloud2Location;

        Random rng;

        public Menu()
        {
            rng = new Random();
            cloud1Location.X = newCloudLocation(0, (Game1.screenWidth + 50));
            cloud1Location.Y = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1].Height, 350);

            cloud2Location.X = newCloudLocation(0, (Game1.screenWidth + 50));
            cloud2Location.Y = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2].Height, 350);
            
        }

        public void update(GameTime gameTime)
        {
            transparancy -= transparancyIncrement;
            transparancy = MathHelper.Clamp(transparancy, 0, 1);

            // Cloud animations.
            cloud1Location.X -= cloudSpeed;
            cloud2Location.X -= cloudSpeed;

            if (cloud1Location.X <= (ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1].Width - Game1.screenWidth))
            {
                cloud1Location.X = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1].Width + 480, ((ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1].Width + Game1.screenWidth) + 200));
                cloud1Location.Y = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1].Height, 350);
            }

            if (cloud2Location.X <= (ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2].Width - Game1.screenWidth))
            {
                cloud2Location.X = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2].Width + 480, ((ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2].Width + Game1.screenWidth) + 200));
                cloud2Location.Y = newCloudLocation(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2].Height, 350);
            }

            // Button presses
            currentTouches = TouchPanel.GetState();
            if (currentTouches.Count > 0)
            {
                touchPoints.X = (int)currentTouches[0].Position.X;
                touchPoints.Y = (int)currentTouches[0].Position.Y;
                if (ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_start].Contains(touchPoints) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    resetState(gameTime);
                    Game1.gamestate = Game1.Gamestate.builder;
                }

                if (ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_options].Contains(touchPoints) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    resetState(gameTime);
                    Game1.gamestate = Game1.Gamestate.options;
                }

                if (ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_quit].Contains(touchPoints) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {

                    resetState(gameTime);
                    //Game1.gamestate = Game1.Gamestate.exit;
                }
            }


        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_background], ContentLoader.rectangles[ContentLoader.TextureNames.menu_background], Color.Lerp(Color.White, Color.Transparent, transparancy));

            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud1], cloud1Location, Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_cloud2], cloud2Location, Color.Lerp(Color.White, Color.Transparent, transparancy));

            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_button_start], ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_start], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_button_options], ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_options], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.menu_button_quit], ContentLoader.rectangles[ContentLoader.TextureNames.menu_button_quit], Color.Lerp(Color.White, Color.Transparent, transparancy));
            
        }

        public int newCloudLocation(int min, int max)
        {
            return rng.Next(min, max);

        }

        public void resetState(GameTime gameTime)
        {
            Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
            transparancy = 1f;
        }
    }
}