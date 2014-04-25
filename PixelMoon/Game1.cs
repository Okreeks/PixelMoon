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
using PixelMoon.parts;

namespace PixelMoon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Boolean debug = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Fonts.
        SpriteFont font;
        //SpriteFont pixel_font;

        // Screensize.
        public static Int32 screenWidth = 480;
        public static Int32 screenHeight = 800;

        // To prefent too rapid tabbing trough screens
        public static Int32 touchTick;

        // Levels.
        Intro intro;
        Menu menu;
        Builder builder;
        Launcher launcher;
        Options options;
        Outro outro;

        // Vars.

        public enum Gamestate
        {
            start,
            loading,
            menu,
            options,
            builder,
            launcher,
            endgame,
            outro,
            exit
        }

        // Locations to not use new vector2
        Vector2 stringLocations = new Vector2(0, 700);

        public static Gamestate gamestate = Gamestate.start;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitUpsideDown;
            //graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLoader.loadContent(this);

            font = this.Content.Load<SpriteFont>("spriteFont1");
            //pixel_font = Content.Load<SpriteFont>("pixelmix");

            // Menu sprites

            intro = new Intro();
            menu = new Menu();
            builder = new Builder();
            launcher = new Launcher(this);
            options = new Options();
            outro = new Outro();

            // gamestate = Gamestate.menu;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Check for different gamestates and act accordingly.
            switch (gamestate)
            {
                case Gamestate.start:
                    intro.update(gameTime);
                    break;

                case Gamestate.menu:
                    // Check the menu buttons for me, tyvm
                    menu.update(gameTime);
                    break;

                case Gamestate.builder:
                    builder.update(gameTime);
                    break;

                case Gamestate.launcher:
                    launcher.update(gameTime);
                    break;

                case Gamestate.options:
                    outro.update(gameTime);
                    break;

                case Gamestate.exit:
                    // Exit game.
                    //Exit();
                    Activity.Finish();
                    break;


            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if(gamestate != Gamestate.launcher)
            {
                graphics.GraphicsDevice.Clear(Color.Black);
            }

            spriteBatch.Begin();

            // Check for different gamestates and act accordingly.
            switch (gamestate)
            {
                case Gamestate.start:
                    intro.draw(spriteBatch, font);
                    break;

                case Gamestate.menu:
                    menu.draw(spriteBatch);
                    break;

                case Gamestate.builder:
                    builder.draw(spriteBatch, font);
                    break;

                case Gamestate.launcher:
                    launcher.draw(spriteBatch, font, graphics);
                    break;

                case Gamestate.options:
                    outro.draw(spriteBatch, font);
                    break;
            }
            if(debug){
                spriteBatch.DrawString(font, "Gamestate: " + gamestate, stringLocations, Color.Cyan);
                stringLocations.Y += 20;
                spriteBatch.DrawString(font, "tick: " + touchTick, stringLocations, Color.Cyan);
                stringLocations.Y += 20;
                spriteBatch.DrawString(font, "Movingstate: " + Builder.movingstate, stringLocations, Color.Cyan);
                stringLocations.Y = 700;
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void setTouchTick(Int32 tick)
        {
            touchTick = tick + 1;
        }
    }
}
