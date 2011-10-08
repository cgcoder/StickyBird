using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class Nest : RectangleBlock
    {
        public Nest(World world)
            : base(world)
        {
            this.SpriteName = "nest";
            this.Type = ObjectType.Nest;
        }
    }
}
