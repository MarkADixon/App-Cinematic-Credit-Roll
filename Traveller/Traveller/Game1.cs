using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace Traveller
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        PresentationParameters parameters = new PresentationParameters();
        public ScreenManager screenManager;
        SpriteFont fontMG48;

        int introScreenNum = 0;
        float introSplashTimer = 7f;
        

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            

            //uncommenting this would change the default fram rate cycles to 50fps
            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 50f);

#if WINDOWS_PHONE
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
#endif

#if XBOX || WINDOWS
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
#endif
            graphics.PreferMultiSampling = true;
            parameters.MultiSampleCount = 16;
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            graphics.ApplyChanges();

            Camera.ViewportWidth = graphics.PreferredBackBufferWidth;
            Camera.ViewportHeight = graphics.PreferredBackBufferHeight;
            Camera.SetScreenRectangle();
     
            screenManager = new ScreenManager(this);
            Components.Add(screenManager); 
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

#if WINDOWS
            this.IsMouseVisible = true;
#endif      
            Camera.batch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            Assets.Load_UI_Assets(this);
            Assets.UI_Fonts.TryGetValue(Assets.Font.Microgramma48, out fontMG48);

            Assets.Load_Audio_Assets(this);
            Assets.Play_OST_Track(Assets.OST.Overture);
            
            // Activate the first screen (Intro)
            screenManager.AddScreen(new StarScreen(), null);
            Settings.gameState = Settings.GameState.Intro;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (Settings.gameState)
            {
                case Settings.GameState.Intro:
                    {
                        introSplashTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (introSplashTimer < 0 && introScreenNum < 13)
                        {
                            screenManager.AddScreen(new IntroSplashScreen(introScreenNum), null);
                            introScreenNum += 1;
                            introSplashTimer = 13f;
                        }
                        if (introScreenNum == 13 && MediaPlayer.State == MediaState.Stopped) Exit();
                        break; 
                    }
                default:
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
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            Camera.batch.Begin();
            Camera.batch.DrawString(fontMG48,gameTime.TotalGameTime.TotalSeconds.ToString("F2") +" SEC",new Vector2(10,0),Color.Blue,0f,Vector2.Zero,0.3f,SpriteEffects.None,1f);
            Camera.batch.End();


            //if (GameSettings.TitleSafe)
            //{
            //    spriteBatch.Begin();
            //    spriteBatch.Draw(pixel, new Rectangle(0, 0, 128, 720), Color.Red * 0.5f);
            //    spriteBatch.Draw(pixel, new Rectangle(1280 - 128, 0, 128, 720), Color.Red * 0.5f);
            //    spriteBatch.Draw(pixel, new Rectangle(0, 0, 1280, 72), Color.Red * 0.5f);
            //    spriteBatch.Draw(pixel, new Rectangle(0, 720 - 72, 1280, 72), Color.Red * 0.5f);
            //    spriteBatch.End();
            //}
        }
    }
}

