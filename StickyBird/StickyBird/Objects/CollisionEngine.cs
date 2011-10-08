using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StickyBird.Objects
{
    public interface ICollidable
    {
        bool DoesCollide(ICollidable other);
        Vector2 FindExactCollidingPoint(ICollidable other);
        void OnCollision(ICollidable other);

        IShape Shape
        {
            get;
        }

        float Mass
        {
            get;
        }

        Vector2 Velocity
        {
            get;
        }
    }

    public interface IStickOn
    {
        bool IsStuck();
        void SetStickingObject(ISticker sticker);
        ISticker GetStickingObject();
        void MoveStickingObject();
        void ReleaseStickingObject();
    }

    public interface ISticker
    {
        IStickOn GetStickingOnObject();
        void SetStickingOnObject(IStickOn on);
        void StickOnMoved(float oldX, float oldY, float newX, float newY);
    }

    public interface ICollisionDetector
    {
        bool DoesCollide(IShape shape);
        Vector2 FindExactCollidingPoint(IShape shape);
    }
}
