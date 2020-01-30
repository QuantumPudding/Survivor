using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Camera
    {
        public static Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    ViewWidth, ViewHeight);
            }
        }

        public static Rectangle SafeBounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X - Engine.TileWidth,
                    (int)Position.Y - Engine.TileHeight,
                    ViewWidth + Engine.TileWidth,
                    ViewHeight + Engine.TileHeight);
            }
        }

        public static Vector2 Position = Vector2.Zero;

        public static int ViewWidth = 800;
        public static int ViewHeight = 600;
        public static int HalfWidth = 400;
        public static int HalfHeight = 300;

        public Camera() { }
        public Camera(int screenWidth, int screenHeight)
        {
            ViewWidth = screenWidth;
            ViewHeight = screenHeight;

            HalfWidth = screenWidth / 2;
            HalfHeight = screenHeight / 2;
        }

        public static Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0));
            }
        }

        public void Lock(Object target)
        {
            Position.X = target.Position.X + (target.Bounds.Width / 2) - HalfWidth;
            Position.Y = target.Position.Y + (target.Bounds.Height / 2) - HalfHeight;
        }

        public void Clamp()
        {
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            int maxWidth = Engine.MapBounds.Width - ViewWidth;
            int maxHeight = Engine.MapBounds.Height- ViewHeight;
            if (maxWidth < 0)
                maxWidth = 0;
            if (maxHeight < 0)
                maxHeight = 0;

            if (Position.X > maxWidth)
                Position.X = maxWidth;
            if (Position.Y > maxHeight)
                Position.Y = maxHeight;

        }

        public void Clamp(int width, int height)
        {
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;

            if (Position.X < width)
                Position.X = width;
            if (Position.Y < height)
                Position.Y = height;
        }

        public void Clamp(int x, int y, int width, int height)
        {
            if (Position.X < x)
                Position.X = x;
            if (Position.Y < y)
                Position.Y = y;

            if (Position.X < width)
                Position.X = width;
            if (Position.Y < height)
                Position.Y = height;
        }

        
    }
}
