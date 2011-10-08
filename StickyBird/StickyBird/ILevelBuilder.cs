using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StickyBird.Screens;
using StickyBird.Objects;

namespace StickyBird
{
    interface ILevelBuilder
    {
        void BuildLevel(PlayScreen screen);
        void resetGame(StickyBirdObj bird);
    }
}
