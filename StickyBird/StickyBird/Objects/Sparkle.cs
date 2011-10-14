using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class Sparkle : DynamicGameObject
    {
        private int frames;

        public Sparkle(World world)
            : base(world)
        {
            this.SpriteName = "sparkle";
            this.IsGravityDisabled = false;
            frames = 0;
            this.DeleteMarker = gameObj => gameObj.Position.Y > 480;
        }

        public override void Update(long timer)
        {
            frames++;
            scale = 0.95f * scale;
            base.Update(timer);
        }
    }
}
