using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework;
using StickyBird.Screens;

namespace StickyBird
{
    class StickyBirdGameImpl : JuicyGame
    {
        public StickyBirdGameImpl(Game game)
            : base(game)
        {

        }

        protected override void AddScreens()
        {
            this.AddScreen(1, new PlayScreen());
        }

        public override void LoadContent()
        {
            base.LoadContent();
            GameConfig.initConfig(this);
        }
    }
}
