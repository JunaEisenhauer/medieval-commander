using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace StrategyGame.Service.View
{
    public struct Rectangle
    {
        public Vector2 Center => Min + (0.5f * Size);

        public Vector2 Min { get; }

        public Vector2 Max { get; }

        public Vector2 Size => Max - Min;

        public Rectangle(float minX, float minY, float width, float height)
        {
            Min = new Vector2(minX, minY);
            Max = Min + new Vector2(width, height);
        }

        public static Rectangle FromCenterSize(float centerX, float centerY, float width, float height)
        {
            return new Rectangle(centerX - (0.5f * width), centerY - (0.5f * height), width, height);
        }

        public bool Intersects(Rectangle rectangle)
        {
            var noXintersect = (Max.X <= rectangle.Min.X) || (Min.X >= rectangle.Max.X);
            var noYintersect = (Max.Y <= rectangle.Min.Y) || (Min.Y >= rectangle.Max.Y);
            return !(noXintersect || noYintersect);
        }

        public Rectangle NewLocation(float minX, float minY)
        {
            return new Rectangle(minX, minY, Size.X, Size.Y);
        }

        public Rectangle Translate(Vector2 t)
        {
            return new Rectangle(Min.X + t.X, Min.Y + t.Y, Size.X, Size.Y);
        }

        public Rectangle Translate(float tx, float ty, float sizeX, float sizeY)
        {
            return new Rectangle(Min.X + tx, Min.Y + ty, sizeX, sizeY);
        }
    }
}
