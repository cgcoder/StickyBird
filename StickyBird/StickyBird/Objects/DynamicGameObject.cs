using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Juicy.Engine;

namespace StickyBird.Objects
{
    public class DynamicGameObject : GameObj
    {
        protected Vector2 previousPosition;
        protected Vector2 velocity;
        protected Vector2 acceleration;
        protected float frictionCoeff;
        protected float mass;
        protected World world;
        protected ICollisionDetector collisionDetector;
        protected IShape shape;
        private Vector2 requiredCameraPosition;
        protected float angularVelocity;
        protected bool disableGravity;

        public enum ObjectType
        {
            Balance, Deadly, Star, Nest
        }

        public DynamicGameObject(World world)
        {
            velocity = new Vector2(0, 0);
            center = new Vector2(0, 0);
            mass = 1;
            frictionCoeff = 1f;
            this.world = world;
            angularVelocity = 0f;
            RequiredCameraPosition = new Vector2(0, 0);
            this.Type = ObjectType.Balance;
        }

        public ObjectType Type
        {
            get;
            set;
        }

        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }

        public override Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                previousPosition = new Vector2(position.X, position.Y);
            }
        }

        public Vector2 PrevPosition
        {
            get { return previousPosition; }
        }

        public bool IsGravityDisabled
        {
            get { return disableGravity; }
            set { disableGravity = value; }
        }

        public void ApplyImpulse(Vector2 force)
        {
            // f = ma
            // a = f/m
            Vector2 acc = force;
            acc.X = acc.X / mass;
            acc.Y = acc.Y / mass;

            velocity.X += acc.X;
            velocity.Y += acc.Y;
        }

        public float AngularVelocity
        {
            get { return angularVelocity; }
            set { angularVelocity = value; }
        }

        public ICollisionDetector CollisionDetector
        {
            get { return collisionDetector; }
            set { collisionDetector = value; }
        }

        public void ApplyFriction(float coeff)
        {
            frictionCoeff = coeff;
        }

        public void RemoveFriction()
        {
            frictionCoeff = 1f;
        }

        public IShape Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        public void ApplyGravity()
        {
            if (IsGravityDisabled) return;

            velocity.Y = velocity.Y + world.Gravity;
        }

        public void Stop()
        {
            velocity.X = velocity.Y = 0;
        }

        public override void UpdatePhysics(long timer)
        {
            // apply friction
            velocity = velocity * frictionCoeff;

            // update position
            previousPosition.X = position.X;
            previousPosition.Y = position.Y;
            position += velocity;

            if (velocity.LengthSquared() <= 0.0001f)
            {
                velocity.X = 0;
                velocity.Y = 0;
            }

            ApplyGravity();

            rotation += angularVelocity;
        }

        internal void MoveBackToPreviousPosition()
        {
            position.X = previousPosition.X;
            position.Y = previousPosition.Y;
        }

        public Vector2 RequiredCameraPosition
        {
            get
            {
                return requiredCameraPosition;
            }
            set
            {
                requiredCameraPosition = value;
            }
        }
    }
}
