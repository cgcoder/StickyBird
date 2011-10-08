using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class World
    {
        private float gravity = 0.20f;

        public static World CurrentWorld = new World();

        public float Gravity
        {
            get { return gravity; }
        }
    }
}
