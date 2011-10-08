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

        public bool IsStuck()
        {
            return sticker != null;
        }

        public void SetStickingObject(ISticker sticker)
        {
            this.sticker = sticker;
        }

        public ISticker GetStickingObject()
        {
            return sticker;
        }

        public void MoveStickingObject()
        {

        }

        public void ReleaseStickingObject()
        {
            sticker = null;
        }

        #endregion
    }
}
