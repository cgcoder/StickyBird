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

        const string NORMAL = "normal";
        const string STARRING = "starring";
        const string ELECTRO = "electrocuted";
        const string KILLED = "killed";

        public StickyBirdObj(World world)
            : base(world)
        {
            this.SpriteName = "bird2";
            shape = new WrapperCircle(this, 25f);
            collisionDetector = new CircleCollisionDetector(this, shape);
            IsAlive = true;
            InitAnimator();
            this.UpdateAnchorPoint(27.5f, 27.5f);
        }

        protected void InitAnimator()
        {
            FrameAnimator animator = new FrameAnimator(55, 55);
            AnimationSequence seq = new AnimationSequence(NORMAL, 1, 1);
            seq.Mode = AnimationSequence.AnimationMode.LOOP;

            AnimationSequence starring = new AnimationSequence(STARRING, 2, 5);
            starring.Mode = AnimationSequence.AnimationMode.STOP_AT_END;

            AnimationSequence electro = new AnimationSequence(ELECTRO, 11, 14);
            electro.Mode = AnimationSequence.AnimationMode.LOOP;

            AnimationSequence killed = new AnimationSequence(KILLED, 15, 20);
            killed.Mode = AnimationSequence.AnimationMode.LOOP;

            animator.AddAnimation(seq);
            animator.AddAnimation(starring);
            animator.AddAnimation(electro);
            animator.AddAnimation(killed);

            animator.CurrentAnimationName = "starring";

            this.Animator = animator;
            this.AnimationNotifier = OnAnimationComplete;
        }

        protected void OnAnimationComplete(FrameAnimator animator, AnimationSequence.AnimationStatus status)
        {
            if (animator.CurrentAnimationName == STARRING)
            {
                animator.RewindCurrentAnimation();
                animator.CurrentAnimationName = NORMAL;
            }
        }

        public override void Update(long timer)
        {
            if (RandomUtil.Random.Next(50) == 2)
            {
                animator.CurrentAnimationName = STARRING;
            }

            base.Update(timer);
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

        public bool IsAlive
        {
            get;
            set;
        }
    }
}
