using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StickyBird.Objects
{

    public class RectangleCollisionDetector : ICollisionDetector
    {
        protected RectangleBlock dgo;
        protected IRectangle myShape;
        protected LineCollisionDetector[] lineDetectors;

        protected int hitSide;

        public RectangleCollisionDetector(RectangleBlock dgo)
        {
            this.dgo = dgo;
            this.myShape = dgo.Shape as IRectangle;
            lineDetectors = new LineCollisionDetector[4];
            for (int i = 0; i < lineDetectors.Length; i++)
            {
                lineDetectors[i] = new LineCollisionDetector(dgo, myShape.Lines[0]);
            }
        }

        #region ICollisionDetector Members

        public bool DoesCollide(IShape shape)
        {
            IRectangle rect = this.myShape;
            ICircle ball = shape as ICircle;
            bool collide = false;
            for (int i = 0; i < rect.Lines.Length; i++)
            {
                if (lineDetectors[i].DoesCollide(shape))
                {
                    collide = true;
                    hitSide = i;
                    break;
                }
            }

            return collide;
        }

        public Vector2 FindExactCollidingPoint(IShape shape)
        {
            ICircle ball = shape as ICircle;

            IRectangle rect = this.myShape;
            return lineDetectors[hitSide].FindExactCollidingPoint(shape);
        }

        #endregion
    }

    public class LineCollisionDetector : ICollisionDetector
    {

        protected DynamicGameObject dgo;
        protected ILine line;

        public LineCollisionDetector(DynamicGameObject dgo)
        {
            this.dgo = dgo;
            line = dgo.Shape as ILine;
        }

        public LineCollisionDetector(DynamicGameObject dgo, ILine line)
        {
            this.dgo = dgo;
            this.line = line;
        }

        #region ICollisionDetector Members
        
        public bool DoesCollide(IShape shape)
        {
            ICircle ball = shape as ICircle;

            float moreX = line.LineStart.X > line.LineEnd.X ? line.LineStart.X : line.LineEnd.X;
            float moreY = line.LineStart.Y > line.LineEnd.Y ? line.LineStart.Y : line.LineEnd.Y;

            float lessX = line.LineStart.X < line.LineEnd.X ? line.LineStart.X : line.LineEnd.X;
            float lessY = line.LineStart.Y < line.LineEnd.Y ? line.LineStart.Y : line.LineEnd.Y;

            if ((ball.Position.X < lessX && ball.Position.Y < lessY)
                || (ball.Position.X > moreX && ball.Position.Y < lessY)
                || (ball.Position.X > moreX && ball.Position.Y > moreY)
                || (ball.Position.X < lessX && ball.Position.Y > moreY))
            {
                return false;
            }

            Vector2 normal;

            if (VectorUtil.intersect(line.LineStart, line.LineEnd, ball.Position, out normal))
            {
                if (VectorUtil.Magnitude(normal) <= ball.Radius)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector2 FindExactCollidingPoint(IShape shape)
        {
            ICircle ball = shape as ICircle;

            Vector2 normal;

            VectorUtil.intersect(line.LineStart, line.LineEnd, ball.Position, out normal);

            Vector2 newPos = new Vector2();
            Vector2 unitNormal = VectorUtil.Unitize(normal);
            Vector2 pt1 = line.LineStart;
            Vector2 pt2 = line.LineEnd;
            Vector2 bp = ball.Position;

            normal = unitNormal * ball.Radius;
            
            Vector2 tl = new Vector2(pt1.X - pt2.X, pt1.Y - pt2.Y);
            Vector2 bv = new Vector2(bp.X - pt2.X, bp.Y - pt2.Y);

            Vector2 ball_on_line = VectorUtil.Project(bv, tl);

            newPos.X = pt2.X + ball_on_line.X;
            newPos.Y = pt2.Y + ball_on_line.Y;

            return newPos + normal;
        }

        #endregion
    }

    public class CircleCollisionDetector : ICollisionDetector
    {
        protected DynamicGameObject dgo;
        protected ICircle circle;

        public CircleCollisionDetector(DynamicGameObject dgo, IShape shape)
        {
            this.dgo = dgo;
            circle = shape as ICircle;
        }

        #region ICollisionDetector Members

        public bool DoesCollide(IShape shape)
        {
            
            ICircle c1 = circle;
            ICircle c2 = shape as ICircle;

            float totRadius = c1.Radius + c2.Radius;
            float centerDistance = (float)Math.Sqrt((c1.Position.X - c2.Position.X) * (c1.Position.X - c2.Position.X)
                    + (c1.Position.Y - c2.Position.Y) * (c1.Position.Y - c2.Position.Y));

            return centerDistance <= totRadius;
        }

        public Vector2 FindExactCollidingPoint(IShape shape)
        {
            ICircle c1 = circle;
            ICircle c2 = shape as ICircle;

            float totRadius = c1.Radius + c2.Radius;
            float centerDistance = (float)Math.Sqrt((c1.Position.X - c2.Position.X) * (c1.Position.X - c2.Position.X)
                    + (c1.Position.Y - c2.Position.Y) * (c1.Position.Y - c2.Position.Y));

            Vector2 ballMoveVector = new Vector2(shape.Position.X - shape.PrevPosition.X, shape.Position.Y - shape.PrevPosition.Y);
            Vector2 ballDirUnitVector = VectorUtil.Unitize(ballMoveVector);

            Vector2 exact = new Vector2(circle.Position.X - totRadius*ballDirUnitVector.X,
                circle.Position.Y - totRadius * ballDirUnitVector.Y);

            return exact;
        }

        #endregion
    }
}
