using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
    public class ContentLoader
    {
        public static Dictionary<TextureNames, Texture2D> Textures { get; set; }
        public static Dictionary<SoundNames, SoundEffect> Sounds { get; set; }
        public static Dictionary<SoundNames, Song> Songs { get; set; }
        public static Dictionary<SpriteFonts, SpriteFont> spritefonts { get; set; }
        public static Dictionary<TextureNames, Rectangle> rectangles { get; set; }
        public static Dictionary<TextureNames, Part> parts { get; set; }

        public enum TextureNames
        {
            empty,
            menu_background,
            LoadingScreenBG,
            menu_button_start,
            menu_button_options,
            menu_button_quit,
            text_AllIDreamOff,
            text_IsToReachTheMoon,
            moon,
            moonAndStar,
            reachingHand,
            menu_cloud1,
            menu_cloud2,
            button_closeBuilder,
            button_launchRocket,
            button_showFormulas,
            button_refreshBuilder,
            slideOut,
            sliderFrame,
            capsule_test,
            capsule_1,
            tank_1,
            engine_1,
            fairing,
            grid,
            launcherBG,
            diamondParticle,
            throttleStation,
            throttleHandle,
            outro1,
            outro2,
            outro3,
            outro4,
            outro5,
            outro6,
            formulas
        }

        public enum SoundNames
        {

        }

        public enum SpriteFonts
        {
            //sp
        }

        public static void loadContent(Game1 game)
        {
            Textures = new Dictionary<TextureNames, Texture2D>();
            Sounds = new Dictionary<SoundNames, SoundEffect>();
            Songs = new Dictionary<SoundNames, Song>();
            spritefonts = new Dictionary<SpriteFonts, SpriteFont>();
            rectangles = new Dictionary<TextureNames, Rectangle>();
            parts = new Dictionary<TextureNames, Part>();

            Textures.Add(TextureNames.menu_background, game.Content.Load<Texture2D>("sprites/backgrounds/menuBG"));
            rectangles.Add(TextureNames.menu_background, new Rectangle(0, 0, 480, 800));

            Textures.Add(TextureNames.LoadingScreenBG, game.Content.Load<Texture2D>("sprites/backgrounds/LoadingScreenBG"));
            rectangles.Add(TextureNames.LoadingScreenBG, new Rectangle(0, 0, 480, 800));

            Textures.Add(TextureNames.menu_button_start, game.Content.Load<Texture2D>("sprites/buttons/startButton"));
            rectangles.Add(TextureNames.menu_button_start, new Rectangle((Game1.screenWidth / 2) - (Textures[TextureNames.menu_button_start].Width / 2), (Game1.screenHeight / 2), 250, 80));

            Textures.Add(TextureNames.menu_button_options, game.Content.Load<Texture2D>("sprites/buttons/optionsButton"));
            rectangles.Add(TextureNames.menu_button_options, new Rectangle((Game1.screenWidth / 2) - (Textures[TextureNames.menu_button_options].Width / 2), (Game1.screenHeight / 2) + 100, 250, 80));

            Textures.Add(TextureNames.menu_button_quit, game.Content.Load<Texture2D>("sprites/buttons/quitButton"));
            rectangles.Add(TextureNames.menu_button_quit, new Rectangle((Game1.screenWidth / 2) - (Textures[TextureNames.menu_button_quit].Width / 2), (Game1.screenHeight / 2) + 200, 250, 80));

            Textures.Add(TextureNames.moon, game.Content.Load<Texture2D>("sprites/global/moon"));
            rectangles.Add(TextureNames.moon, new Rectangle((Game1.screenWidth - Textures[TextureNames.moon].Width), 0, 215, 204));

            Textures.Add(TextureNames.text_AllIDreamOff, game.Content.Load<Texture2D>("sprites/global/allIDreamOff"));
            rectangles.Add(TextureNames.text_AllIDreamOff, new Rectangle((Game1.screenWidth / 2) - (Textures[TextureNames.text_AllIDreamOff].Width / 2), 233, 480, 35));

            Textures.Add(TextureNames.text_IsToReachTheMoon, game.Content.Load<Texture2D>("sprites/global/isToReachTheMoon"));
            rectangles.Add(TextureNames.text_IsToReachTheMoon, new Rectangle((Game1.screenWidth / 2) - (Textures[TextureNames.text_IsToReachTheMoon].Width / 2), 290, 480, 26));

            Textures.Add(TextureNames.moonAndStar, game.Content.Load<Texture2D>("sprites/global/moonAndStars"));
            rectangles.Add(TextureNames.moonAndStar, new Rectangle(0, 0, 480, 399));

            Textures.Add(TextureNames.reachingHand, game.Content.Load<Texture2D>("sprites/global/reachingHand"));
            rectangles.Add(TextureNames.reachingHand, new Rectangle(0, (Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.reachingHand].Height), 480, 490));

            Textures.Add(TextureNames.menu_cloud1, game.Content.Load<Texture2D>("sprites/effects/cloud1_dev"));
            rectangles.Add(TextureNames.menu_cloud1, new Rectangle(0, 0, 180, 100));

            Textures.Add(TextureNames.menu_cloud2, game.Content.Load<Texture2D>("sprites/effects/cloud2_dev"));
            rectangles.Add(TextureNames.menu_cloud2, new Rectangle(0, 0, 180, 100));
            // Builder buttons
            Textures.Add(TextureNames.button_closeBuilder, game.Content.Load<Texture2D>("sprites/buttons/button_closeBuilder"));
            rectangles.Add(TextureNames.button_closeBuilder, new Rectangle(0, (Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.button_closeBuilder].Height), 103, 103));

            Textures.Add(TextureNames.button_launchRocket, game.Content.Load<Texture2D>("sprites/buttons/button_launchRocket"));
            rectangles.Add(TextureNames.button_launchRocket, new Rectangle(0, ((Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.button_launchRocket].Height) - ContentLoader.Textures[ContentLoader.TextureNames.button_closeBuilder].Height), 103, 250));

            Textures.Add(TextureNames.button_refreshBuilder, game.Content.Load<Texture2D>("sprites/buttons/button_refreshBuilder"));
            rectangles.Add(TextureNames.button_refreshBuilder, new Rectangle(0, (((((Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.button_launchRocket].Height) - ContentLoader.Textures[ContentLoader.TextureNames.button_closeBuilder].Height) - 5) - ContentLoader.Textures[ContentLoader.TextureNames.button_refreshBuilder].Height) - 5), 103, 103));

            Textures.Add(TextureNames.button_showFormulas, game.Content.Load<Texture2D>("sprites/buttons/button_showFormulas"));
            rectangles.Add(TextureNames.button_showFormulas, new Rectangle(0, (((((Game1.screenHeight - ContentLoader.Textures[ContentLoader.TextureNames.button_launchRocket].Height) - ContentLoader.Textures[ContentLoader.TextureNames.button_showFormulas].Height) - 5) - (ContentLoader.Textures[ContentLoader.TextureNames.button_showFormulas].Height * 2)) - 5), 103, 103));

            Textures.Add(TextureNames.slideOut, game.Content.Load<Texture2D>("sprites/buttons/slideOut"));
            rectangles.Add(TextureNames.slideOut, new Rectangle((Game1.screenWidth - ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Width),  (ContentLoader.Textures[ContentLoader.TextureNames.slideOut].Height * 2), 44, 89));

            Textures.Add(TextureNames.sliderFrame, game.Content.Load<Texture2D>("sprites/backgrounds/slider"));
            rectangles.Add(TextureNames.sliderFrame, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.sliderFrame].Width), 0, 121, 800));

            // Rocket parts etc, try to do it by sets!
            Textures.Add(TextureNames.capsule_test, game.Content.Load<Texture2D>("sprites/parts/capsules/test"));
            rectangles.Add(TextureNames.capsule_test, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.capsule_test].Width), 0, 101, 101));
            // Set 0
            // Set 1
            Textures.Add(TextureNames.capsule_1, game.Content.Load<Texture2D>("sprites/parts/capsules/capsule_1"));
            rectangles.Add(TextureNames.capsule_1, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.capsule_1].Width), 0, 101, 101));

            Textures.Add(TextureNames.tank_1, game.Content.Load<Texture2D>("sprites/parts/tanks/tank_1"));
            rectangles.Add(TextureNames.tank_1, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.tank_1].Width), (ContentLoader.Textures[ContentLoader.TextureNames.tank_1].Width + 50), 101, 101));

            Textures.Add(TextureNames.fairing, game.Content.Load<Texture2D>("sprites/parts/engines/engine_fairing"));
            rectangles.Add(TextureNames.fairing, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.fairing].Width), 0, 101, 101));

            Textures.Add(TextureNames.engine_1, game.Content.Load<Texture2D>("sprites/parts/engines/engine_1"));
            rectangles.Add(TextureNames.engine_1, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.engine_1].Width), 0, 101, 101));

            Textures.Add(TextureNames.grid, game.Content.Load<Texture2D>("sprites/global/grid"));
            rectangles.Add(TextureNames.grid, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.grid].Width), 0, 101, 101));

            Textures.Add(TextureNames.launcherBG, game.Content.Load<Texture2D>("sprites/backgrounds/launcherBG"));
            rectangles.Add(TextureNames.launcherBG, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.launcherBG].Width), 0, 480, 800));

            Textures.Add(TextureNames.diamondParticle, game.Content.Load<Texture2D>("sprites/effects/diamond"));
            rectangles.Add(TextureNames.diamondParticle, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.diamondParticle].Width), 0, 101, 101));

            Textures.Add(TextureNames.throttleStation, game.Content.Load<Texture2D>("sprites/buttons/throttleStation"));
            rectangles.Add(TextureNames.throttleStation, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.throttleStation].Width), 0, 103, 250));

            Textures.Add(TextureNames.throttleHandle, game.Content.Load<Texture2D>("sprites/buttons/throttleHandle"));
            rectangles.Add(TextureNames.throttleHandle, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Width), 0, 103, 29));

            // Outro
            Textures.Add(TextureNames.outro1, game.Content.Load<Texture2D>("sprites/effects/AsIGotCloser"));
            Textures.Add(TextureNames.outro2, game.Content.Load<Texture2D>("sprites/effects/iFeltSomethingStrange"));
            Textures.Add(TextureNames.outro3, game.Content.Load<Texture2D>("sprites/effects/andWhenSheGotInMyReach"));
            Textures.Add(TextureNames.outro4, game.Content.Load<Texture2D>("sprites/effects/iWokeUp"));
            Textures.Add(TextureNames.outro5, game.Content.Load<Texture2D>("sprites/effects/sheStillRemainsADream"));
            Textures.Add(TextureNames.outro6, game.Content.Load<Texture2D>("sprites/effects/aDream"));
            //rectangles.Add(TextureNames.throttleHandle, new Rectangle((Game1.screenWidth + ContentLoader.Textures[ContentLoader.TextureNames.throttleHandle].Width), 0, 103, 29));

            Textures.Add(TextureNames.formulas, game.Content.Load<Texture2D>("sprites/buttons/formulas"));
            rectangles.Add(TextureNames.formulas, new Rectangle((ContentLoader.Textures[ContentLoader.TextureNames.button_showFormulas].Width), 0, 375, 800));

        }
    }
}