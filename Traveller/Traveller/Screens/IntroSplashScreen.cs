using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Traveller
{
    class IntroSplashScreen : GameScreen
    {
        SpriteFont fontMG48;
        string text1 = "";
        string text2 = "";
        float scale1 = 0.5f;
        float scale2 = 1.0f;
        Vector2 length1 = Vector2.Zero;
        Vector2 length2 = Vector2.Zero;
        Vector2 pos1 = Vector2.Zero;
        Vector2 pos2 = Vector2.Zero;
        float timer = 2.5f; //how long text stays on before transition out

        List<string> text1source = new List<string> 
        {
            "From the makers of",
            "Comes an ambitious new concept...",
            "Created in association with",
            "Produced by",
            "Presenting",
            "Lead Programmer",
            "Creative Design",
            "Creative Design",
            "Creative Design / Graphic Art",
            "Programmer",
            "Graphic Art / Community Director",
            "Graphic Art / Webmaster ",
            "TANNHAUSER ZERO",
        };

        List<string> text2source = new List<string>
        {
            "PRODUCE WARS",
            "",
            "SERAPHYM STUDIOS",
            "GIGALOTH GAMES",
            "TANNHAUSER ZERO",
            "Mark A. Dixon",
            "Bruce Abbott",
            "Robert Haynes",
            "William Thomas",
            "Jason Thomas",
            "Chris Dixon",
            "A.J. Hartmann",
            "NOW IN PRODUCTION"
        };


        public IntroSplashScreen(int intro_screen_num)
        {
            text1 = text1source[intro_screen_num];
            text2 = text2source[intro_screen_num];
            if (intro_screen_num == 4)
            {
                scale2 = 1.33f;
                timer = 4f;
            }
            TransitionOnTime = TimeSpan.FromSeconds(5);
            TransitionOffTime = TimeSpan.FromSeconds(2.5);
        }


        public override void LoadContent()
        {
            Assets.UI_Fonts.TryGetValue(Assets.Font.Microgramma48, out fontMG48);
            length1 = fontMG48.MeasureString(text1) * scale1;
            length2 = fontMG48.MeasureString(text2) * scale2;
            pos1 = Camera.Origin - (length1 / 2) + new Vector2(0,-80);
            pos2 = Camera.Origin - (length2 / 2) + new Vector2(0, -20);
        }


        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (this.ScreenState == Traveller.ScreenState.Active) 
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timer < 0) ExitScreen();
            }
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {             
            Camera.batch.Begin();
            Camera.batch.DrawString(fontMG48, text1, pos1, Color.Blue*TransitionAlpha, 0f, Vector2.Zero, scale1, SpriteEffects.None, 1.0f);
            if (this.ScreenState == Traveller.ScreenState.TransitionOn)
                Camera.batch.DrawString(fontMG48, text2, pos2, Color.Blue*(float)Math.Pow(TransitionAlpha,2), 0f, Vector2.Zero, scale2, SpriteEffects.None, 1.0f);
            else
                Camera.batch.DrawString(fontMG48, text2, pos2, Color.Blue *TransitionAlpha, 0f, Vector2.Zero, scale2, SpriteEffects.None, 1.0f);
            Camera.batch.End();
            base.Draw(gameTime);
        }


        #endregion
    }
}
