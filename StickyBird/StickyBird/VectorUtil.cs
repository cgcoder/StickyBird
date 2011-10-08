using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StickyBird
{
    public class VectorUtil
    {
        public static bool intersect(Vector2 pt1, Vector2 pt2, Vector2 ball, out Vector2 normal)
        {
            normal = new Vector2(0, 0);

            float lessX = pt1.X < pt2.X ? pt1.X : pt2.X;
            float moreX = pt1.X < pt2.X ? pt2.X : pt1.X;

            float lessY = pt1.Y < pt2.Y ? pt1.Y : pt2.Y;
            float moreY = pt1.Y < pt2.Y ? pt2.Y : pt1.Y;

            Vector2 line = new Vector2(pt1.X - pt2.X, pt1.Y - pt2.Y);
            Vector2 bv = new Vector2(ball.X - pt2.X, ball.Y - pt2.Y);

            Vector2 ball_on_line = Project(bv, line);

            if (VectorUtil.Magnitude(ball_on_line) > VectorUtil.Magnitude(line))
            {
                return false;
            }

            Vector2 tnormal = bv - ball_on_line;

            normal.X = tnormal.X;
            normal.Y = tnormal.Y;

            return true;
        }

        public static Vector2 Unitize(Vector2 vect)
        {
            float magnitude = (float)Math.Sqrt(vect.X * vect.X + vect.Y * vect.Y);
            return new Vector2(vect.X / magnitude, vect.Y / magnitude);
        }

        public static float Magnitude(Vector2 v)
        {
            return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float Dot(Vector2 v1, Vector2 v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y);
        }

        public static Vector2 Project(Vector2 vect, Vector2 onto)
        {
            Vector2 ontoUnit = Unitize(onto);
            Vector2 proj = new Vector2(ontoUnit.X, ontoUnit.Y);

            float magnitude = (float)Math.Sqrt(onto.X * onto.X + onto.Y * onto.Y);

            float s = (Dot(vect, onto) / magnitude);

            proj.X = proj.X * s;
            proj.Y = proj.Y * s;

            return proj;
        }
    }
}
