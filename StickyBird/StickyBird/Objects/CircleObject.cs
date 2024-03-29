using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StickyBird.Objects
{
    public class CircleObject : DynamicGameObject, IStickOn
    {
        protected ISticker sticker = null;
        protected float stickingAngle;

        public CircleObject(World world)
            : base(world)
        {
            shape = new WrapperCircle(this);
            collisionDetector = new CircleCollisionDetector(this, shape);
        }

        public CircleObject(World world, int radius)
            : base(world)
        {
            shape = new WrapperCircle(this, radius);
            collisionDetector = new CircleCollisionDetector(this, shape);
        }

        public bool IsStuck()
        {
            return sticker != null;
        }

        public void SetStickingObject(ISticker sticker)
        {
            if (this.sticker == null && sticker != null)
            {
                DynamicGameObject col = sticker as DynamicGameObject;

                Vector2 thisPos = this.position;
                Vector2 spos = col.Position;

                float delX = thisPos.X - spos.X;
                float delY = thisPos.Y - spos.Y;
                float hyp = (float)Math.Sqrt(delX * delX + delY * delY);

                float ang = (float)Math.Asin(delX / hyp);
                stickingAngle = ang + (float)Math.PI / 2;
                if (delY > 0) stickingAngle = -stickingAngle;
            }
            this.sticker = sticker;
        }

        public ISticker GetStickingObject()
        {
            return this.sticker;
        }

        public void MoveStickingObject()
        {
            DynamicGameObject col = sticker as DynamicGameObject;
            float rad = (col.Shape as ICircle).Radius + (shape as ICircle).Radius;

            Vector2 position = new Vector2(this.Position.X + (float)(rad * Math.Cos(stickingAngle)),
                    this.Position.Y + (float)(rad * Math.Sin(stickingAngle)));
            col.Position = position;
        }

        public void ReleaseStickingObject()
        {
            sticker = null;
        }
    }
}
