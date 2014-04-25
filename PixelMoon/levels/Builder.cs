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
    class Builder
    {
        Single transparancy = 1f;
        Single transparancyIncrement = 0.01f;

        // TouchableRegion and drag vars
        TouchCollection currentTouches;
        TouchCollection oldTouch;
        Point touchPoints = new Point(0, 0);
        Single yDrag = 0;
        Rectangle dragArea = new Rectangle(((Game1.screenWidth / 2) - 50), 0, 101, Game1.screenHeight);

        // Sliders for the parts.
        Slider sliderCapsules;
        Slider sliderTanks;
        Slider sliderEngines;

        // Parts.
        public static Dictionary<ContentLoader.TextureNames, Part> masterList = new Dictionary<ContentLoader.TextureNames, Part>();
        Dictionary<ContentLoader.TextureNames, Part> capsules = new Dictionary<ContentLoader.TextureNames, Part>();
        Dictionary<ContentLoader.TextureNames, Part> tanks = new Dictionary<ContentLoader.TextureNames, Part>();
        Dictionary<ContentLoader.TextureNames, Part> engines = new Dictionary<ContentLoader.TextureNames, Part>();

        
        public static Boolean sliderArrowVisible = true;

        public static ContentLoader.TextureNames movingstate = ContentLoader.TextureNames.empty;

        // Grid/Rocket.
        Rectangle grid;
        Boolean gridTouched = false;
        public static Stack<Part> rocketStack;
        Stack<Rectangle> partStack;
        Int32 partY;
        Int32 stages;
        Stack<Stack> staging; // Must contain the staging in the future, in this will be stacks with parts.

        // Part info to be displayed.
        public Int32 partWeightTotal;
        public Int32 electricityCapacityTotal;
        public Int32 oxygenCapacityTotal;
        public Int32 fuelCapacityTotal;
        public Int32 fuelWeightTotal;
        public Int32 maxThrust;

        Boolean formulasOpen = false;

        // Locations to not use new vector2
        Vector2 stringLocations = new Vector2(0, 0);
        Vector2 partLocation = new Vector2(190, 0);

        public Builder()
        {
            // Masterlist with part objects.
            masterList.Add(ContentLoader.TextureNames.capsule_test, new Capsule(ContentLoader.TextureNames.capsule_test, new Rectangle(0, 0, 101, 101), 50));
            masterList.Add(ContentLoader.TextureNames.capsule_1, new Capsule(ContentLoader.TextureNames.capsule_1, new Rectangle(0, 0, 101, 101), 50));
            masterList.Add(ContentLoader.TextureNames.tank_1, new Tank(ContentLoader.TextureNames.tank_1, new Rectangle(0, 0, 101, 101), 120, 100, 1));
            masterList.Add(ContentLoader.TextureNames.engine_1, new Engine(ContentLoader.TextureNames.engine_1, new Rectangle(0, 0, 101, 101), 150, 100, 5, 1000, 1));

            // Create a list of capsules for the slider.
            capsules.Add(ContentLoader.TextureNames.capsule_test, masterList[ContentLoader.TextureNames.capsule_test]);
            capsules.Add(ContentLoader.TextureNames.capsule_1, masterList[ContentLoader.TextureNames.capsule_1]);

            // Create a list of tanks for the slider.
            tanks.Add(ContentLoader.TextureNames.tank_1, masterList[ContentLoader.TextureNames.tank_1]);

            // Create a list of engines for the slider.
            engines.Add(ContentLoader.TextureNames.engine_1, masterList[ContentLoader.TextureNames.engine_1]);

            sliderCapsules = new Slider(150, capsules);
            sliderTanks = new Slider(250, tanks);
            sliderEngines = new Slider(350, engines);

            // 1st spot in grid.
            grid = new Rectangle(190, 100, 101, 101);


            // Initiate the staging/rocket.
            rocketStack = new Stack<Part>();
            partStack = new Stack<Rectangle>();

            oldTouch = TouchPanel.GetState();
        }

        public void update(GameTime gameTime)
        {

            sliderCapsules.update();
            sliderTanks.update();
            sliderEngines.update();

            transparancy -= transparancyIncrement;
            transparancy = MathHelper.Clamp(transparancy, 0f, 1f);

            currentTouches = TouchPanel.GetState();

            if (currentTouches.Count > 0)
            {
                touchPoints.X = (int)currentTouches[0].Position.X;
                touchPoints.Y = (int)currentTouches[0].Position.Y;
            }

            rocketDrag();
            checkPartDrop();

            if (currentTouches.Count > 0)
            {

                // Drag and move rocket up and down.

                if (movingstate == ContentLoader.TextureNames.empty && ContentLoader.rectangles[ContentLoader.TextureNames.button_launchRocket].Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y)) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    if(rocketStack.Count > 0)
                    {
                        resetState(gameTime);
                        // Launcher.masterList = masterList;
                        // Launcher.rocketStack = rocketStack;
                        Game1.gamestate = Game1.Gamestate.launcher;
                    }
                }

                if (movingstate == ContentLoader.TextureNames.empty && ContentLoader.rectangles[ContentLoader.TextureNames.button_closeBuilder].Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y)) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    
                    resetState(gameTime);
                    Game1.gamestate = Game1.Gamestate.menu;
                }

                if (movingstate == ContentLoader.TextureNames.empty && ContentLoader.rectangles[ContentLoader.TextureNames.button_refreshBuilder].Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y)) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    
                    resetRocket();
                }

                if (movingstate == ContentLoader.TextureNames.empty && ContentLoader.rectangles[ContentLoader.TextureNames.button_showFormulas].Contains(new Point((int)currentTouches[0].Position.X, (int)currentTouches[0].Position.Y)) && gameTime.TotalGameTime.Seconds >= Game1.touchTick)
                {
                    Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
                    if (formulasOpen == true)
                    {
                        formulasOpen = false;
                    }
                    else
                    {
                        formulasOpen = true;
                    }

                }
            }
        }

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.LoadingScreenBG], ContentLoader.rectangles[ContentLoader.TextureNames.LoadingScreenBG], Color.White);
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.button_launchRocket], ContentLoader.rectangles[ContentLoader.TextureNames.button_launchRocket], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.button_refreshBuilder], ContentLoader.rectangles[ContentLoader.TextureNames.button_refreshBuilder], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.button_showFormulas], ContentLoader.rectangles[ContentLoader.TextureNames.button_showFormulas], Color.Lerp(Color.White, Color.Transparent, transparancy));
            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.button_closeBuilder], ContentLoader.rectangles[ContentLoader.TextureNames.button_closeBuilder], Color.Lerp(Color.White, Color.Transparent, transparancy));

            sliderCapsules.drawArrow(spriteBatch);
            sliderTanks.drawArrow(spriteBatch);
            sliderEngines.drawArrow(spriteBatch);
            sliderCapsules.draw(spriteBatch);
            sliderTanks.draw(spriteBatch);
            sliderEngines.draw(spriteBatch);

            if (movingstate != ContentLoader.TextureNames.empty)
            {
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Name: " + masterList[movingstate].partName, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Weight: " + masterList[movingstate].partWeight, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;

                switch (masterList[movingstate].type)
                {
                    case Part.partType.capsule:
                        spriteBatch.DrawString(font, "Oxygen: " + masterList[movingstate].oxygenCapacity, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y += 15;
                        spriteBatch.DrawString(font, "Electr: " + masterList[movingstate].electricityCapacity, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y = 0;
                        break;

                    case Part.partType.tank:
                        spriteBatch.DrawString(font, "Fuel: " + masterList[movingstate].fuelCapacity, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y += 15;
                        spriteBatch.DrawString(font, "F weight: 1:" + masterList[movingstate].fuelWeight, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y = 0;
                        break;

                    case Part.partType.engine:
                        spriteBatch.DrawString(font, "Thrust: " + masterList[movingstate].efficiency, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y += 15;
                        spriteBatch.DrawString(font, "ISP: " + masterList[movingstate].ISP, stringLocations, Color.FromNonPremultiplied(141, 114, 114, 255));
                        stringLocations.Y = 0;
                        // spriteBatch.DrawString(font, "Consump: " + masterList[movingstate].consumption, new Vector2(0, 75), Color.FromNonPremultiplied(141, 114, 114, 255));
                        break;
                }
            }
            else
            {
                // Part info to be displayed.
                stringLocations.Y = 0;
                spriteBatch.DrawString(font, "Stages: " + stages, new Vector2(0, 0), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Weight: " + partWeightTotal, new Vector2(0, 15), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Electr: " + electricityCapacityTotal, new Vector2(0, 30), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Oxygen: " + oxygenCapacityTotal, new Vector2(0, 45), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Fuel: " + fuelCapacityTotal, new Vector2(0, 60), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "F Weight: " + fuelWeightTotal, new Vector2(0, 75), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y += 15;
                spriteBatch.DrawString(font, "Thrust: " + maxThrust, new Vector2(0, 90), Color.FromNonPremultiplied(141, 114, 114, 255));
                stringLocations.Y = 0;
            }

            spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.grid], new Rectangle(grid.X, grid.Y + (int)yDrag, grid.Width, grid.Height), Color.Lerp(Color.White, Color.Transparent, 0.5f));

            if(rocketStack.Count > 0)
            {
                partY = rocketStack.Count;
                foreach (Part part in rocketStack)
                {
                    partLocation.Y = (101 * partY) + (int)yDrag;
                    // If a part is not the last engine, set fairings.
                    if (part.type == Part.partType.engine && partY < rocketStack.Count)
                    {
                        spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.fairing], partLocation, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(ContentLoader.Textures[part.partName], partLocation, Color.White);
                    }                 
                    partY--;
                }
            }

            if(movingstate != ContentLoader.TextureNames.empty)
            {
                spriteBatch.Draw(ContentLoader.Textures[movingstate], new Vector2(currentTouches[0].Position.X, currentTouches[0].Position.Y), null, Color.Lerp(Color.White, Color.Transparent, 0.5f), 0f, new Vector2((ContentLoader.Textures[movingstate].Width / 2), (ContentLoader.Textures[movingstate].Height / 2)), 2f, SpriteEffects.None, 0f);
            }         

            // Draw formulas
            if(formulasOpen)
            {
                spriteBatch.Draw(ContentLoader.Textures[ContentLoader.TextureNames.formulas], ContentLoader.rectangles[ContentLoader.TextureNames.formulas], Color.White);
            }

        }

        public static void setMovingstate(ContentLoader.TextureNames movingstateParameter){
            if (movingstate == ContentLoader.TextureNames.empty)
            {
                movingstate = movingstateParameter;
            }
        }

        public void checkPartDrop()
        {
            // See if part is dropped in rectangle.
            if (currentTouches.Count > 0)
            {
                if (movingstate != ContentLoader.TextureNames.empty && grid.Contains(touchPoints))
                {
                    gridTouched = true;
                }
            }
            else
            {
                if (gridTouched)
                {
                    // Add to stack.
                    //rocketStack.Push(masterList[movingstate]);

                    switch (movingstate)
                    {
                        case ContentLoader.TextureNames.capsule_test:
                            rocketStack.Push(new Capsule(ContentLoader.TextureNames.capsule_test, new Rectangle(0, 0, 101, 101), 50));
                            break;

                        case ContentLoader.TextureNames.capsule_1:
                            rocketStack.Push(new Capsule(ContentLoader.TextureNames.capsule_1, new Rectangle(0, 0, 101, 101), 50));
                            break;

                        case ContentLoader.TextureNames.tank_1:
                            rocketStack.Push(new Tank(ContentLoader.TextureNames.tank_1, new Rectangle(0, 0, 101, 101), 120, 100, 1));
                            break;

                        case ContentLoader.TextureNames.engine_1:
                            rocketStack.Push(new Engine(ContentLoader.TextureNames.engine_1, new Rectangle(0, 0, 101, 101), 150, 100, 5, 1000, 1));
                            break;
                    }
                    
                    partStack.Push(new Rectangle(190, (rocketStack.Count * 101), 101, 101));
                    gridTouched = false;
                    grid.Y += 101;

                    // Set totals back to 0.
                    stages = 0;
                    partWeightTotal = 0;
                    electricityCapacityTotal = 0;
                    oxygenCapacityTotal = 0;
                    fuelCapacityTotal = 0;
                    fuelWeightTotal = 0;
                    maxThrust = 0;

                    // Calculate new rocket totals.
                    foreach (Part part in rocketStack)
                    {
                        part.stageNumber = stages;
                        if(part.type == Part.partType.engine)
                        {
                            stages ++;
                        }
                        partWeightTotal += part.partWeight;
                        electricityCapacityTotal += part.electricityCapacity;
                        oxygenCapacityTotal += part.oxygenCapacity;
                        fuelCapacityTotal += part.fuelCapacity;
                        fuelWeightTotal += part.fuelWeight;
                        maxThrust += part.thrust;
                    }
                }

                movingstate = ContentLoader.TextureNames.empty;
            }
        }

        public void rocketDrag()
        {
            if (currentTouches.Count > 0 && oldTouch.Count > 0 && dragArea.Contains(touchPoints) && movingstate == ContentLoader.TextureNames.empty)
            {
                yDrag += currentTouches[0].Position.Y - oldTouch[0].Position.Y;
                // yDrag = MathHelper.Clamp(yDrag, Game1.screenHeight - (rocketStack.Count * 101), (rocketStack.Count * 101) + 101);
            }

            oldTouch = currentTouches;
        }

        public void resetRocket()
        {
            // Reset rocket stack.
            yDrag = 0;
            rocketStack.Clear();
            grid.Y = 100;
            partWeightTotal = 0;
            electricityCapacityTotal = 0;
            oxygenCapacityTotal = 0;
            fuelCapacityTotal = 0;
            fuelWeightTotal = 0;
            maxThrust = 0;
        }

        public Stack<Part> getRocket()
        {
            return rocketStack;
        }

        public void resetState(GameTime gameTime)
        {
            Game1.setTouchTick((int)gameTime.TotalGameTime.Seconds);
            yDrag = 0;
            sliderCapsules.resetState();
            sliderTanks.resetState();
            sliderEngines.resetState();
            formulasOpen = false;
            transparancy = 1f;
        }
    }
}