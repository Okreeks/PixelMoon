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
    class Launcher
    {

        Single transparancy = 1f;
        Single transparancyIncrement = 0.01f;

        // public static Dictionary<ContentLoader.TextureNames, Part> masterList;
        // public static Stack<Part> rocketStack;
        Int32 altitude = 0; // In KM.
        Int32 stratosphere = 50; // In KM.
        Int32 rocketSpeed = 0; // In M/S
        Part engineCheck;
        Boolean launched = true;
        Int32 throttle = 0; // Min 0, max 100.
        Int32 deltaV = 0;
        Single ISP = 0;
        Single GM;
        Single singularForce;
        Single accceleration;
        Single pullBack;

        // TouchableRegion and drag vars
        TouchCollection currentTouches;
        TouchCollection oldTouch;
        Point touchPoints = new Point(0, 0);
        Single yDrag = 0;
        Rectangle dragArea = new Rectangle(((Game1.screenWidth / 2) - 50), 0, 101, Game1.screenHeight);
        Int32 partY;
        Boolean dragging = false; // Used so that the buttons can check wether something is being dragged, so they wont react.

        // Part info to be displayed. TEMP!
        public Single partWeightTotal;
        public Int32 electricityCapacityTotal;
        public Int32 oxygenCapacityTotal;
        public Int32 fuelCapacityTotal;
        public Single fuelLevel;
        public Int32 fuelWeightTotal;
        public Int32 maxThrust;
        private ParticleEngine particleEngine;
        public Boolean particles = false;
        public Boolean startOutro = false;
        // Buttons
        Rectangle throttleRectangle = new Rectangle(Game1.screenWidth - ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Width, Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Height, ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Width, ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Height);
        Rectangle throttleHandleRectangle = new Rectangle(Game1.screenWidth - ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Width, Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Height, ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Width, ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Height);

        // Timers
        float timer = 1;         // Initialize a 1 second timer
        float timerReset = 1;

        // Locations to not use new vector2
        Vector2 stringLocations = new Vector2(0, 15);
        Vector2 partLocation = new Vector2(190, 0);

        public Launcher(Game1 game)
        {
            oldTouch = TouchPanel.GetState();
            particleEngine = new ParticleEngine(game);
        }

        public void update(GameTime gameTime)
        {
            transparancy -= transparancyIncrement;
            transparancy = MathHelper.Clamp(transparancy, 0f, 1f);

            currentTouches = TouchPanel.GetState();

            if(currentTouches.Count > 0)
            {
                touchPoints.X = (int)currentTouches[0].Position.X;
                touchPoints.Y = (int)currentTouches[0].Position.Y;
            }

            rocketDrag();
            checkButtons(gameTime);
            calculateTravel(gameTime);

            // Particles.
            particleEngine.throttle = throttle;
            engineCheck = Builder.rocketStack.Peek();
            if (engineCheck.type == Part.partType.engine && throttle > 0 && altitude < 384399)
            {
                particles = true;
            }
            else
            {
                particles = false;
            }

            if(particles)
            {
            particleEngine.EmitterLocation = new Vector2(240, (((Builder.rocketStack.Count * 101) + 101) + yDrag));
            particleEngine.Update(particles);
            // particleEngine.Update(false);
            }

            if(partWeightTotal == 0){
                // Calculate new rocket totals.
                foreach (Part part in Builder.rocketStack)
                {
                    partWeightTotal += part.partWeight;
                    electricityCapacityTotal += part.electricityCapacity;
                    oxygenCapacityTotal += part.oxygenCapacity;
                    fuelLevel = fuelCapacityTotal += part.fuelCapacity;
                    maxThrust += part.thrust;
                    if (part.ISP > ISP)
                    {
                        ISP = part.ISP;
                    }

                    if(part.fuelWeight > fuelWeightTotal)
                    {
                        fuelWeightTotal = part.fuelWeight;
                    }
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font, GraphicsDeviceManager graphics)
        {
            // graphics.GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, (int)(255 - (altitude * 0000.66)), 255));
            graphics.GraphicsDevice.Clear(Color.Black);

            // spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.launcherBG], ContentLoader.rectangles[ContentLoader.TextureNames.launcherBG], Color.FromNonPremultiplied(0, 0, (int)(255 - (altitude * 0000.66)), 255));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.button_closeBuilder], ContentLoader.rectangles[ContentLoader.TextureNames.button_closeBuilder], Color.Lerp(Color.White, Color.Transparent, transparancy));

            // Part info to be displayed.
            spriteBatch.DrawString(font, "Weight: " + partWeightTotal, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
            stringLocations.Y += 15;
            spriteBatch.DrawString(font, "Fuel: " + fuelLevel + "/" + fuelCapacityTotal, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
            stringLocations.Y += 15;
            spriteBatch.DrawString(font, "Thrust: " + maxThrust, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
            stringLocations.Y += 15;
            spriteBatch.DrawString(font, "Alt: " + altitude, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
            stringLocations.Y = 15;

            // Particles go woosh!
            particleEngine.Draw(spriteBatch);


            partY = Builder.rocketStack.Count;
            foreach (Part part in Builder.rocketStack)
            {
                // If a part is not the last engine, set fairings.
                partLocation.Y = (101 * partY) + (int)yDrag;
                if (part.type == Part.partType.engine && partY < Builder.rocketStack.Count)
                {
                    
                    spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.fairing], partLocation, Color.Lerp(Color.White, Color.Transparent, transparancy));
                }
                else
                {
                    spriteBatch.Draw(ContentLoader.Textures[part.partName], partLocation, Color.Lerp(Color.White, Color.Transparent, transparancy));
                }
                partY--;
            }

            // Draw throttle
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.throttleStation], throttleRectangle, Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle], throttleHandleRectangle, Color.Lerp(Color.White, Color.Transparent, transparancy));

        }

        private void checkButtons(GameTime gameTime)
        {
            if (currentTouches.Count > 0)
            {
                if (ContentLoader.rectangles[ContentLoader.TextureNames.button_closeBuilder].Contains(touchPoints) && gameTime.TotalGameTime.Seconds >= Game1.touchTick && !dragging)
                {
                    resetState(gameTime);
                    Game1.gamestate = Game1.Gamestate.builder;
                }

                if (throttleRectangle.Contains(touchPoints) && gameTime.TotalGameTime.Seconds >= Game1.touchTick && !dragging)
                {
                    throttleHandleRectangle.Y = (int)currentTouches[0].Position.Y;
                    // Value, min, max.
                    throttleHandleRectangle.Y = (int)MathHelper.Clamp(throttleHandleRectangle.Y, (Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Height), (Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Height));
                    throttle = (int)((((throttleHandleRectangle.Y - Game1.screenHeight) *-1) * 0.4525f) - 13);
                }
            }
        }

        private void calculateTravel(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Seconds >= timer)
            {
               
                // GM = (altitude * (float)00000.26) / 10000;
                // GM = MathHelper.Clamp(GM, 0, (float)9.81);

                // deltaV = partWeightTotal - (fuelCapacityTotal * fuelWeightTotal);

                engineCheck = Builder.rocketStack.Peek();

                // Remove fuel.
                if (fuelLevel > 0 && engineCheck.type == Part.partType.engine && altitude < 384399)
                {
                    fuelLevel -= ISP * ((float)throttle / 100);
                    partWeightTotal -= ISP * ((float)throttle / 100);
                    pullBack = (fuelLevel * fuelWeightTotal);// *GM;
                    accceleration = maxThrust * ((float)throttle / 100);
                    singularForce = accceleration * pullBack;
                    // singularForce = throttle; // * pullBack;
                    altitude += (int)singularForce / 100;
                    // altitude += throttle;
                    // sole.WriteLine("F: " + singularForce);
                }
                else
                {
                    // Game1.gamestate = Game1.Gamestate.outro;
                }
                // Reset Timer.
                timer = gameTime.TotalGameTime.Seconds + (int)timerReset;   
            }
        }

        public void setRocket(Stack<Part> rocket)
        {
            Builder.rocketStack = rocket;
        }

        public void rocketDrag()
        {
            if (currentTouches.Count > 0 && oldTouch.Count > 0 && dragArea.Contains(touchPoints) && altitude < 384399)
            {
                yDrag += currentTouches[0].Position.Y - oldTouch[0].Position.Y;
                // yDrag = MathHelper.Clamp(yDrag, Game1.screenHeight - (rocketStack.Count * 101), (rocketStack.Count * 101) + 101);
                dragging = true;
            }

            oldTouch = currentTouches;
        }

        public void resetState(GameTime gameTime)
        {
            Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
            // Set totals back to 0.
            throttleHandleRectangle.Y = (int)Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Height;
            partWeightTotal = 0;
            electricityCapacityTotal = 0;
            oxygenCapacityTotal = 0;
            fuelCapacityTotal = 0;
            fuelLevel = 0;
            fuelWeightTotal = 0;
            maxThrust = 0;
            rocketSpeed = 0;
            ISP = 0;
            transparancy = 1f;
            deltaV = 0;
            altitude = 0;
            startOutro = false;
            // rocketStack.Clear();
        }
    }
}