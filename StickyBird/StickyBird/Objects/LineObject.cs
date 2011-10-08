using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StickyBird.Objects;
using Microsoft.Xna.Framework;

namespace StickyBird
{
    public class LineObject : DynamicGameObject, IStickOn
    {
        protected ISticker sticker;

        protected Vector2 lineStart;
        protected Vector2 lineEnd;

        protected Vector2 deltaVector;
        protected Vector2 normalVector;

        public LineObject(World w)
            : this(w, "line")
        {

        }

        public LineObject(World w, string sprite)
            : base(w)
        {
            this.spriteName = sprite;
            this.IsGravityDisabled = true;
            lineStart = new Vector2();
            lineEnd = new Vector2();
            deltaVector = new Vector2(0, 0);
            normalVector = new Vector2(0, 0);
            this.Shape = new WrapperLine(this);
            this.CollisionDetector = new LineCollisionDetector(this);
        }

        #region IStickOn Members

        public bool IsStuck()
        {
            return sticker != null;
        }

        public override float Rotation
        {
            set 
            {
                base.Rotation = value;
            }
        }

        protected override void updateChildObjs()
        {
            base.updateChildObjs();
        }

        protected virtual void UpdateInternalPoints()
        {
            float wby2 = this.W / 2;
            float hby2 = this.H / 2;

            lineStart.X = position.X + (wby2 * (float)Math.Cos(this.rotation));
            lineStart.Y = position.Y + (wby2 * (float)Math.Sin(this.rotation));

            lineEnd.X = position.X - (wby2 * (float)Math.Cos(this.rotation));
            lineEnd.Y = position.Y - (wby2 * (float)Math.Sin(this.rotation));
        }

        public void SetStickingObject(ISticker sticker)
        {
            this.sticker = sticker;
            if (sticker != null)
            {
                ICircle ball = ((sticker as DynamicGameObject).Shape as ICircle);

                Vector2 normal;

                VectorUtil.intersect(LineStart, LineEnd, ball.Position, out normal);
                Vector2 unitNormal = VectorUtil.Unitize(normal);
                Vector2 pt1 = LineStart;
                Vector2 pt2 = LineEnd;
                Vector2 bp = ball.Position;

                normal = unitNormal * (ball.Radius);

                Vector2 tl = new Vector2(pt1.X - pt2.X, pt1.Y - pt2.Y);
                Vector2 bv = new Vector2(bp.X - pt2.X, bp.Y - pt2.Y);

                Vector2 ball_on_line = VectorUtil.Project(bv, tl);

                deltaVector = new Vector2(ball_on_line.X, ball_on_line.Y);
                normalVector.X = normal.X;
                normalVector.Y = normal.Y;
            }
        }

        public ISticker GetStickingObject()
        {
            return sticker;
        }

        public void MoveStickingObject()
        {
            ICircle ball = ((sticker as DynamicGameObject).Shape as ICircle);

            Vector2 pt2 = LineEnd;
            (sticker as DynamicGameObject).UpdatePosition(deltaVector.X + pt2.X + normalVector.X, deltaVector.Y + pt2.Y + normalVector.Y);

        }

        public void ReleaseStickingObject()
        {
            sticker = null;
        }

        public Vector2 LineStart
        {
            get 
            {
                UpdateInternalPoints();
                return lineStart; 
            }
        }

        public Vector2 LineEnd
        {
            get 
            {
                UpdateInternalPoints();
                return lineEnd; 
            }
        }

        #endregion
    }
}
