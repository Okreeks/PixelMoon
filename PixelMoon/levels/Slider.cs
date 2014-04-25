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
    class Slider
    {

        enum sliderType
        {
            capsule,
            tank,
            engine
        }
        Single transparancy = 1f;
        Single transparancyIncrement = 0.01f;

        Int32 resetY;

        TouchCollection currentTouches;

        // Harmonica info.
        Int32 slideSpeed = 15;
        Rectangle slider = ContentLoader.rectangles[ContentLoader.TextureNames.slideOut];
        Rectangle sliderFrame = ContentLoader.rectangles[ContentLoader.TextureNames.sliderFrame];
        

        // Parts
        Dictionary<ContentLoader.TextureNames, Part> parts = new Dictionary<ContentLoader.TextureNames, Part>();
        Dictionary<ContentLoader.TextureNames, Rectangle> partsHitList = new Dictionary<ContentLoader.TextureNames, Rectangle>();

        public enum SliderState
        {
            open,
            closed,
            transitionToOpen,
            transitionToClosed
        }

        SliderState slideState = SliderState.closed;

        private int partXPos = Game1.screenWidth + 24;

        public Slider(Int32 sliderY, Dictionary<ContentLoader.TextureNames, Part> partList)
        {
            resetY = slider.Y = sliderY;
            parts = partList;

            Int32 partY = 25;

            foreach (KeyValuePair<ContentLoader.TextureNames, Part> entry in parts)
            {               
                entry.Value.partPosition.Y = partY;
                partY = (entry.Value.partTexture.Height + 25) + partY;
                partsHitList.Add(entry.Key, new Rectangle(partXPos, partY, ContentLoader.Textures[entry.Key].Width, ContentLoader.Textures[entry.Key].Height));
            }
        }

        public void update()
        {
            transparancy -= transparancyIncrement;
            transparancy = MathHelper.Clamp(transparancy, 0f, 1f);

            if (slideState == SliderState.transitionToClosed || slideState == SliderState.transitionToOpen)
            {
                transition(slideState);
            }

            currentTouches = TouchPanel.GetState();
            if (currentTouches.Count > 0)
            {
                if (slider.Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y)))
                {
                    // Fold out the harmonicatab.
                    if (slideState == SliderState.open || slideState == SliderState.closed && Builder.sliderArrowVisible)
                    {
                        // Allowed movment.
                        if (slideState == SliderState.open)
                        {
                            // Builder.sliderVisible = false;
                            slideState = SliderState.transitionToClosed;
                        }
                        else
                        {
                            Builder.sliderArrowVisible = false;
                            slideState = SliderState.transitionToOpen;
                        }
                    }
                }

                // Check parts for being "clicked", sounds less perferted then touched...
                foreach (KeyValuePair<ContentLoader.TextureNames, Rectangle> entry in partsHitList)
                {
                    //Console.WriteLine("position on: " + entry.Value);
                    if(entry.Value.Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y))){

                        // Handle the click.
                        Builder.setMovingstate(entry.Key);
                    }
                }
            }
        }

        public void drawArrow(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.slideOut], slider, Color.Lerp(Color.White, Color.Transparent, transparancy));
        }

        public void draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.sliderFrame], sliderFrame, Color.Lerp(Color.White, Color.Transparent, transparancy));
           
            foreach (KeyValuePair<ContentLoader.TextureNames, Part> entry in parts)
            {
                spriteBatch.Draw(ContentLoader.Textures[entry.Key], new Rectangle(partXPos, entry.Value.partPosition.Y, entry.Value.partPosition.Width, entry.Value.partPosition.Height), Color.White);
            }

        }

        public void transition(SliderState direction)
        {
            if (direction == SliderState.transitionToOpen)
            {
                // Move slider.
                slider.X -= slideSpeed;
                partXPos -= slideSpeed;
                sliderFrame.X = slider.X + ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Width;

                // Check if done.
                // For half way use: if (slider.X == ((Game1.screenWidth / 2) - ContentManager.rectangles[ContentManager.TextureNames.slideOut].Width))
                if (slider.X <= ((Game1.screenWidth - ContentLoader.rectangles[ContentLoader.TextureNames.sliderFrame].Width) - ContentLoader.rectangles[ContentLoader.TextureNames.slideOut].Width))
                {
                    slideState = SliderState.open;
                    Builder.sliderArrowVisible = false;
                }
            }
            else
            {
                // Move slider.
                slider.X += slideSpeed;
                partXPos += slideSpeed;
                sliderFrame.X = slider.X + ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Width;

                // Check if done.
                if (slider.X >= (Game1.screenWidth - ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Width))
                {
                    slider.X = ContentLoader.rectangles[ContentLoader.TextureNames.slideOut].X;
                    sliderFrame.X = ContentLoader.rectangles[ContentLoader.TextureNames.sliderFrame].X;
                    slideState = SliderState.closed;
                    Builder.sliderArrowVisible = true;
                }
            }

            foreach (KeyValuePair<ContentLoader.TextureNames, Part> entry in parts)
            {
                // Update rectangle location for the items.
                partsHitList[entry.Key] = new Rectangle(partXPos, entry.Value.partPosition.Y, entry.Value.partPosition.Width, entry.Value.partPosition.Height);

            }

            // For halfway use: slider.X = (int)MathHelper.Clamp(slider.X, ((Game1.screenWidth / 2) - ContentManager.rectangles[ContentManager.TextureNames.slideOut].Width), (Game1.screenWidth - ContentManager.Textures[ContentManager.TextureNames.slideOut].Width));
            slider.X = (int)MathHelper.Clamp(slider.X, ((Game1.screenWidth - ContentLoader.rectangles[ContentLoader.TextureNames.sliderFrame].Width) - ContentLoader.rectangles[ContentLoader.TextureNames.slideOut].Width), (Game1.screenWidth - ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Width));
            sliderFrame.X = (int)MathHelper.Clamp(sliderFrame.X, (slider.X + ContentLoader.rectangles[ContentLoader.TextureNames.slideOut].Width), Game1.screenWidth);
        }

        public void resetState()
        {
            // Set everything back to starter values.
            slider = ContentLoader.rectangles[ContentLoader.TextureNames.slideOut];
            slider.Y = resetY;
            sliderFrame = ContentLoader.rectangles[ContentLoader.TextureNames.sliderFrame];
            partXPos = Game1.screenWidth + 24;
            transparancy = 1f;
        }
    }
}