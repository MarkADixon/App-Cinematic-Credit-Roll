using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traveller
{
    public static class Settings
    {
        public enum GameState
        {
            Intro,
        }

        public static GameState gameState = GameState.Intro; //default to intro on game startup

    }
}
