using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class RectangleBlock : DynamicGameObject, IStickOn
    {
        protected ISticker sticker;

        protected float touchDownX;

        public RectangleBlock(World world)
            : base(world)
        {
            this.IsGravityDisabled = true;
            sticker = null;
            this.Shape = new WrapperRectangle(this);
            this.CollisionDetector = new RectangleCollisionDetector(this);
        }

        #region IStickOn Members

        public bool IsStuck()
        {
            return sticker != null;
        }

        public void SetStickingObject(ISticker sticker)
        {
            this.sticker = sticker;
            if (sticker != null)
            {
                touchDownX = (sticker as DynamicGameObject).Position.X - this.Position.X;
            }
        }

        public ISticker GetStickingObject()
        {
            return sticker;
        }

        public void MoveStickingObject()
        {
            if (sticker != null)
            {
                DynamicGameObject dgo = (sticker as DynamicGameObject);
                dgo.UpdatePosition(this.Position.X + touchDownX, dgo.Position.Y);
            }
        }

        public void ReleaseStickingObject()
        {
            sticker = null;
        }

        #endregion
    }
}
