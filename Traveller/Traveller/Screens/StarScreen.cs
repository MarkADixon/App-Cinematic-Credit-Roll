using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Traveller
{
    public class WarpStar
    {
        public Texture2D texture;
        public Vector2 pos = Vector2.Zero; //position of the star of screen
        public float scale = 0f; //size of the star
        public Color color = Color.White; //color of the star
        public float alpha = 1f; //transparency
        public float maxalpha = 1f; //max star brightness, rolled individually

        public WarpStar() //constructor
        {
        }
    }

    class StarScreen : GameScreen
    {
        Texture2D star1, star2,pixel;
        List<WarpStar> stars;

        Vector2 fromCenter;
        float dist,speed;

        public StarScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(5);
            TransitionOffTime = TimeSpan.FromSeconds(5);
        }


        public override void LoadContent()
        {
            Assets.UI_Textures.TryGetValue(Assets.UI.Star1, out star1);
            Assets.UI_Textures.TryGetValue(Assets.UI.Star2, out star2);
            Assets.UI_Textures.TryGetValue(Assets.UI.Pixel, out pixel);


            stars = new List<WarpStar>();
            for (int i = 0; i < 1500; i++)
            {
                WarpStar newstar = new WarpStar();
                if (Camera.rand.Next(0, 2) == 0) newstar.texture = star1;
                else newstar.texture = star2;
                newstar.pos = new Vector2 (Camera.rand.Next(0,Camera.ViewportWidth),Camera.rand.Next(0,Camera.ViewportHeight));
                newstar.scale = (float)Math.Pow(Camera.rand.Next(40, 71)/100f,2);
                newstar.alpha = Camera.rand.Next(0, 40) / 100f;
                newstar.maxalpha = Math.Max(Camera.rand.Next(30,101) / 100f, newstar.alpha);

                stars.Add(newstar);
            }
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
            for (int i = 0; i < stars.Count; i++)
            {
                //distance from center screen
                fromCenter = stars[i].pos - Camera.Origin;
                dist = fromCenter.Length();

                //adjust brightness based on distance
                stars[i].alpha = Math.Min(stars[i].alpha+(dist / 1000000f),stars[i].maxalpha);
                
                //movement speed
                speed = fromCenter.Length() / 1000f * stars[i].scale;

                //movement vector
                fromCenter.Normalize();
                fromCenter *= speed;

                //move star
                stars[i].pos += fromCenter;

                //if star is outside camera relocate to center area
                if (!Camera.WorldRectangleBuffered.Contains(new Rectangle ((int)stars[i].pos.X, (int)stars[i].pos.Y,1,1)))
                {
                    stars[i].pos = new Vector2(Camera.rand.Next(0, Camera.ViewportWidth), Camera.rand.Next(0, Camera.ViewportHeight));
                    stars[i].pos = (stars[i].pos + Camera.Origin) / 2;
                    stars[i].scale = (float)Math.Pow(Camera.rand.Next(40, 71) / 100f, 2);
                    stars[i].alpha = 0.0f; //set alpha transparent so to fade it in
                    stars[i].maxalpha = Camera.rand.Next(30, 101) / 100f;

                }

            }
            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void Draw(GameTime gameTime)
        {
            
            

            Camera.batch.Begin();
            for (int i = 0; i < stars.Count; i++)
            {
                Camera.batch.Draw(stars[i].texture, stars[i].pos, null, stars[i].color * stars[i].alpha, 0f, Vector2.Zero, stars[i].scale, SpriteEffects.None, 1f);
            }

            //fades on/off the screen by means of mask
            if (TransitionAlpha != 1f)  Camera.batch.Draw(pixel, Camera.Viewport, Color.Black*(1f-TransitionAlpha));

            Camera.batch.End();
            
            base.Draw(gameTime);
        }
        #endregion
    }
}
