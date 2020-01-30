using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace TileEngine
{
    public static class Engine
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;
        public const int HalfTileWidth = 32;
        public const int HalfTileHeight = 32;

        public static Rectangle MapBounds = Rectangle.Empty;

        public static Point PositionToCell(Vector2 pos)
        {
            return new Point(
                (int)(pos.X / (float)TileWidth),
                (int)(pos.Y / (float)TileHeight));
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        public static bool RectangleCollision(Rectangle a, Rectangle b)
        {
            return (Abs(a.X - b.X) * 2 < (a.Width + b.Width)) &&
                   (Abs(a.Y - b.Y) * 2 < (a.Height + b.Height));
        }

        public static bool RadiusCollision(Vector2 v1, Vector2 v2, float r1, float r2)
        {
            return VectorLength(v2 - v1) < r1 + r2;
        }

        public static bool RadiusCollision(Vector2 v1, Vector2 v2, float r1, float r2, out float distance)
        {
            distance = VectorLength(v2 - v1);
            return distance < r1 + r2;
        }

        public static int Abs(int i)
        {
            return i > 0 ? i : -i;
        }

        public static float VectorLength(Vector2 v)
        {
            return Sqrt((v.X * v.X) + (v.Y * v.Y));
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            float p = a.X - b.X;
            float q = a.Y - b.Y;

            return Sqrt((p * p) + (q * q));
        }

        public static float Sqrt(float z)
        {
            if (z == 0) return 0;
            FloatIntUnion u;
            u.tmp = 0;
            float xhalf = 0.5f * z;
            u.f = z;
            u.tmp = 0x5f375a86 - (u.tmp >> 1);
            u.f = u.f * (1.5f - xhalf * u.f * u.f);
            return u.f * z;
        }

        public static Vector2 VectorNormalise(Vector2 v)
        {
            float l = VectorLength(v);
            return new Vector2(v.X / l, v.Y / l);
        }

        public static bool LineIntersectsRect(Vector2 p1, Vector2 p2, Rectangle r)
        {
            return LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height)) ||
                   LineIntersectsLine(p1, p2, new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y)) ||
                   (r.Contains(new Point((int)p1.X, (int)p1.Y)) && r.Contains(new Point((int)p2.X, (int)p2.Y)));
        }

        private static bool LineIntersectsLine(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
        {
            float q = (l1p1.Y - l2p1.Y) * (l2p2.X - l2p1.X) - (l1p1.X - l2p1.X) * (l2p2.Y - l2p1.Y);
            float d = (l1p2.X - l1p1.X) * (l2p2.Y - l2p1.Y) - (l1p2.Y - l1p1.Y) * (l2p2.X - l2p1.X);

            if (d == 0)
            {
                return false;
            }

            float r = q / d;

            q = (l1p1.Y - l2p1.Y) * (l1p2.X - l1p1.X) - (l1p1.X - l2p1.X) * (l1p2.Y - l1p1.Y);
            float s = q / d;

            if (r < 0 || r > 1 || s < 0 || s > 1)
            {
                return false;
            }

            return true;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatIntUnion
        {
            [FieldOffset(0)]
            public float f;

            [FieldOffset(0)]
            public int tmp;
        }
    }
}
