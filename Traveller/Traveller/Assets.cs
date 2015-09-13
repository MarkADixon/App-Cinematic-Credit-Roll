using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Traveller
{
    public static class Assets
    {
        private static ContentManager UI_Content;
        public static Dictionary<UI,Texture2D> UI_Textures;
        public static Dictionary<Font, SpriteFont> UI_Fonts;
        
        //list of keys for textures, they match texture names
        public enum UI
        {
            Star1,
            Star2,
            Pixel,
        }

        //list of keys for fonts
        public enum Font
        {
            Microgramma48,


        }

        public static void Load_UI_Assets(Game game)
        {
            if (UI_Content == null) //load only if the Content has not already been loaded
            {
                //initialize
                UI_Content = new ContentManager(game.Services, "Content");
                UI_Textures = new Dictionary<UI, Texture2D>();
                UI_Fonts = new Dictionary<Font, SpriteFont>();

                //load fonts
                SpriteFont MG48 = UI_Content.Load<SpriteFont>(@"UI\Microgramma48");
                UI_Fonts.Add(Font.Microgramma48, MG48);


                //load UI textures
                Texture2D star1 = UI_Content.Load<Texture2D>(@"UI\Star1");
                UI_Textures.Add(UI.Star1, star1);
                Texture2D star2 = UI_Content.Load<Texture2D>(@"UI\Star2");
                UI_Textures.Add(UI.Star2, star2);
                Texture2D pixel = UI_Content.Load<Texture2D>(@"UI\pixel");
                UI_Textures.Add(UI.Pixel, pixel);
            }
            return;
        }

        private static ContentManager Audio_Content;
        public static Dictionary<OST,Song> Audio_Music;
        public static Dictionary<Sound, SoundEffect> Audio_Effect;

        public enum OST //keys for soundtracks
        {
            Overture,
        }

        public enum Sound //keys for audio effects
        {

        }

        public static void Load_Audio_Assets(Game game)
        {
            if (Audio_Content == null)
            {
                //initialize
                Audio_Content = new ContentManager(game.Services, "Content");
                Audio_Music = new Dictionary<OST, Song>();
                Audio_Effect = new Dictionary<Sound, SoundEffect>();

                //load tracks
                Song OST_overture = Audio_Content.Load<Song>(@"OST\Overture");
                Audio_Music.Add(OST.Overture, OST_overture);

                //load sound effects
                //none yet
            }
            return;
        }

        public static void Play_OST_Track(OST ost)
        {
            Song play_this;
            Audio_Music.TryGetValue(ost, out play_this);
            MediaPlayer.Play(play_this);
            return;
        }

    }
}
