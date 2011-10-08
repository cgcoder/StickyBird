using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class WoodBlock : RectangleBlock
    {
        public WoodBlock(World world)
            : this(world, false)
        {
        }

        public WoodBlock(World world, bool half)
            : base(world)
        {
            this.SpriteName = half ? "woodh" : "stone";
        }
    }
}
