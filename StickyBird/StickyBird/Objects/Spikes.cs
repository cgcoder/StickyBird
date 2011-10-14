using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StickyBird.Objects
{
    public class Spikes : RectangleBlock
    {
        public Spikes(World world)
            : base(world)
        {
            this.SpriteName = "spikes";
            this.Type = ObjectType.Deadly;
        }

        #region IStickOn Members

        public override bool IsStuck()
        {
            return sticker != null;
        }

        public override void SetStickingObject(ISticker sticker)
        {
            this.sticker = sticker;
        }

        public override ISticker GetStickingObject()
        {
            return sticker;
        }

        public override void MoveStickingObject()
        {

        }

        public override void ReleaseStickingObject()
        {
            sticker = null;
        }

        #endregion
    }
}
