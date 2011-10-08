using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StickyBird.Objects
{
    public interface IShape
    {
        Vector2 Position
        {
            get;
        }

        Vector2 PrevPosition
        {
            get;
        }

        // update shape attributes, based on the actual object
        void Sync();
    }

    public interface ICircle : IShape
    {
        float Radius
        {
            get;
        }
    }

    public interface ILine : IShape
    {
        Vector2 LineStart
        {
            get;
        }

        Vector2 LineEnd
        {
            get;
        }
    }

    public interface IRectangle : IShape
    {
        ILine[] Lines
        {
            get;
        }

        ILine GetLine(int side);
    }

    public class WrapperCircle : ICircle
    {
        private DynamicGameObject gdo;
        private float radius;
        private bool radiusOverriden;

        public WrapperCircle(DynamicGameObject gdo)
        {
            this.gdo = gdo;
            radiusOverriden = false;
        }

        public WrapperCircle(DynamicGameObject gdo, float radius)
        {
            this.gdo = gdo;
            this.radius = radius;
            radiusOverriden = true;
        }

        #region ICircle Members

        public float Radius
        {
            get
            {
                if (radiusOverriden) return radius;

                return gdo.Sprite.Width / 2;
            }
        }

        #endregion

        #region IShape Members

        public Vector2 Position
        {
            get
            {
                return new Vector2(gdo.Position.X, gdo.Position.Y);
            }
        }

        public Vector2 PrevPosition
        {
            get
            {
                return new Vector2(gdo.PrevPosition.X, gdo.PrevPosition.Y);
            }
        }

        public void Sync()
        {

        }

        #endregion
    }

    /// <summary>
    /// Wrapper Line object. Can wrap either a rectangle or a line object.
    /// For rectangle, it can represent any one of the four side (side), for line (side = -1)
    /// For rectangle, sides are calculated dynamically, where as for line, they are just 
    /// returned from the Line Wrapped object.
    /// </summary>
    public class WrapperLine : ILine
    {
        private DynamicGameObject wrapObj;
        private Vector2 start;
        private Vector2 end;
        private int side;

        public WrapperLine(LineObject line)
        {
            wrapObj = line;
            this.side = -1;
        }

        public WrapperLine(RectangleBlock rect) : this(rect, 0)
        {
        }

        public WrapperLine(RectangleBlock rect, int side)
        {
            this.wrapObj = rect;
            this.side = side;

            start = new Vector2();
            end = new Vector2();

            Sync();
        }

        #region IShape Members

        public Vector2 Position
        {
            get
            {
               return new Vector2(wrapObj.Position.X, wrapObj.Position.Y);
            }
        }

        public Vector2 PrevPosition
        {
            get
            {
                return new Vector2(wrapObj.PrevPosition.X, wrapObj.PrevPosition.Y);
            }
        }

        public void Sync()
        {
            // no sync for line
            if (wrapObj is LineObject) return;

            switch (side)
            {
                case 0: // top
                    start.X = wrapObj.Position.X - wrapObj.W / 2;
                    start.Y = wrapObj.Position.Y - wrapObj.H / 2;

                    end.X = wrapObj.Position.X + wrapObj.W / 2;
                    end.Y = wrapObj.Position.Y - wrapObj.H / 2;
                    break;
                case 1: // right
                    start.X = wrapObj.Position.X + wrapObj.W / 2;
                    start.Y = wrapObj.Position.Y - wrapObj.H / 2;

                    end.X = wrapObj.Position.X + wrapObj.W / 2;
                    end.Y = wrapObj.Position.Y + wrapObj.H / 2;
                    break;
                case 2: // bottom
                    start.X = wrapObj.Position.X - wrapObj.W / 2;
                    start.Y = wrapObj.Position.Y + wrapObj.H / 2;

                    end.X = wrapObj.Position.X + wrapObj.W / 2;
                    end.Y = wrapObj.Position.Y + wrapObj.H / 2;
                    break;
                case 3: // left
                    start.X = wrapObj.Position.X - wrapObj.W / 2;
                    start.Y = wrapObj.Position.Y - wrapObj.H / 2;

                    end.X = wrapObj.Position.X - wrapObj.W / 2;
                    end.Y = wrapObj.Position.Y + wrapObj.H / 2;
                    break;
            }
        }

        #endregion

        #region ILine Members

        public Vector2 LineStart
        {
            get 
            {
                if (wrapObj is LineObject)
                {
                    return (wrapObj as LineObject).LineStart;
                }
                else
                {
                    Sync();
                    return start;
                }
            }
        }

        public Vector2 LineEnd
        {
            get 
            {
                if (wrapObj is LineObject)
                {
                    return (wrapObj as LineObject).LineEnd;
                }
                else
                {
                    Sync();
                    return end;
                }
            }
        }

        #endregion
    }

    public class WrapperRectangle : IRectangle
    {

        protected ILine[] lines;

        protected RectangleBlock rblock;

        public WrapperRectangle(RectangleBlock rblock)
        {
            this.rblock = rblock;
            lines = new ILine[4];

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = new WrapperLine(rblock, i);
            }
        }


        #region IRectangle Members

        public ILine[] Lines
        {
            get
            {
                return lines;
            }
        }

        public ILine GetLine(int side)
        {
            return lines[side];
        }

        #endregion

        #region IShape Members

        public Vector2 Position
        {
            get { return new Vector2(rblock.Position.X, rblock.Position.Y); }
        }

        public Vector2 PrevPosition
        {
            get { return new Vector2(rblock.PrevPosition.X, rblock.PrevPosition.Y); }
        }

        public void Sync()
        {
            foreach(ILine line in lines)
            {
                line.Sync();
            }
        }

        #endregion
    }
}
