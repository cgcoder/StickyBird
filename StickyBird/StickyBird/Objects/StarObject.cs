using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Juicy.Engine;
using Microsoft.Xna.Framework;

namespace StickyBird.Objects
{
    public class StarObject : LineObject
    {
        public StarObject(World currentWorld)
            : base(currentWorld)
        {
            this.SpriteName = "stars";
            this.IsGravityDisabled = true;
            InitAnimator();
        }

        protected void InitAnimator()
        {
            FrameAnimator a = new FrameAnimator(25, 25);
            AnimationSequence seq = new AnimationSequence("rotating", 0, 8);
            seq.Mode = AnimationSequence.AnimationMode.LOOP;
            a.AddAnimation(seq);
            a.CurrentAnimationName = "rotating";
            this.Type = ObjectType.Star;
            this.Animator = a;
        }

        public override void OnObjManagerAdd(GameObjectManager gom)
        {
            int width = 25;
            base.OnObjManagerAdd(gom);

            if (spriteName == null || spriteName.Length == 0) return;

            boundary = new Rectangle(0, 0, (int)(width * scale), (int)(sprite.Height * scale));
            center.X = width / 2;
            center.Y = sprite.Height / 2;
            anchor = center;
        }
    }
}
