using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StickyBird.Objects
{
    public class StickyBirdObj : DynamicGameObject, ISticker
    {
        private IStickOn stickingOn;


        public StickyBirdObj(World world)
            : base(world)
        {
            this.SpriteName = "bird";
            shape = new WrapperCircle(this, 20.0f);
            collisionDetector = new CircleCollisionDetector(this, shape);
        }

        #region ISticker Members

        public IStickOn GetStickingOnObject()
        {
            return stickingOn;
        }

        public void StickOnMoved(float oldX, float oldY, float newX, float newY)
        {
            position.X = newX;
            position.Y = newY;
        }

        public void SetStickingOnObject(IStickOn on)
        {
            stickingOn = on;
            if (stickingOn != null)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }

            disableGravity = stickingOn != null;
        }

        #endregion
    }
}
