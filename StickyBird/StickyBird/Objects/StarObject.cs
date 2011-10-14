using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StickyBird.Objects
{
    public class StarObject : CircleObject
    {
        private DynamicGameObject rotatorObj;

        public StarObject(World currentWorld)
            : base(currentWorld, 25/2)
        {
            this.SpriteName = "stars";
            this.IsGravityDisabled = true;
            rotatorObj = new DynamicGameObject(currentWorld);
            rotatorObj.IsGravityDisabled = true;
            rotatorObj.SpriteName = "rotator";
            rotatorObj.Rotation = (float) (RandomUtil.Random.NextDouble() * Math.PI * 2);
            rotatorObj.AngularVelocity = 0.15f;
            this.Type = ObjectType.Star;
            //InitAnimator();
        }
        
        protected void InitAnimator()
        {
            FrameAnimator a = new FrameAnimator(25, 25);
            AnimationSequence seq = new AnimationSequence("rotating", 0, 8);
            seq.Mode = AnimationSequence.AnimationMode.LOOP;
            a.AddAnimation(seq);
            a.CurrentAnimationName = "rotating";
            this.Animator = a;
        }

        protected override void updateChildObjs()
        {
            base.updateChildObjs();
            rotatorObj.UpdatePosition(this.Position.X, this.Position.Y);
        }

        public override void OnObjManagerAdd(GameObjectManager gom)
        {
            base.OnObjManagerAdd(gom);
            rotatorObj.OnObjManagerAdd(gom);
        }

        public override void Update(long timer)
        {
            rotatorObj.Update(timer);
            base.Update(timer);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
            rotatorObj.Draw(batch);
        }

        public override bool Visible
        {
            set 
            {
                base.Visible = value;
                rotatorObj.Visible = value;
            }
        }
    }
}
