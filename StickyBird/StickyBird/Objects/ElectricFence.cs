using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class ElectricFence : LineObject
    {
        public ElectricFence(World w)
            : base(w, "lighting")
        {
            this.Type = ObjectType.Deadly;
        }
    }
}
