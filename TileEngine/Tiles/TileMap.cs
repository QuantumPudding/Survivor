using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public static class TileMap
    {
        public static List<TileLayer> Layers = new List<TileLayer>();
        public static CollisionLayer CollisionLayer;

        public static void Draw(SpriteBatch sb)
        {
            Point min = Engine.PositionToCell(Camera.Position);
            Point max = Engine.PositionToCell(
                Camera.Position + new Vector2(
                    min.X + sb.GraphicsDevice.Viewport.Width + Engine.TileWidth,
                    min.Y + sb.GraphicsDevice.Viewport.Height + Engine.TileHeight));

            foreach (TileLayer l in Layers)
                l.Draw(sb, min, max);
        }
    }
}
