using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StickyBird.Objects
{

    public class RedBall : CircleObject
    {
        public RedBall(World world)
            : base(world)
        {
            this.spriteName = "redbean";
            this.IsGravityDisabled = true;
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }

        public override void Update(long time)
        {
            base.Update(time);

            if (sticker != null)
            {
                (sticker as DynamicGameObject).Rotation = rotation;
                stickingAngle += angularVelocity;
            }
        }
    }
}
