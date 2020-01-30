using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TileEngine
{
    public class TileLayer
    {
        public List<Animation> TileTextures = new List<Animation>();
        public int[,] Map;

        public static int LayerWidth;
        public static int LayerHeight;

        public int PixelWidth
        {
            get
            {
                return Engine.TileWidth * Map.GetLength(0);
            }
        }

        public int PixelHeight
        {
            get
            {
                return Engine.TileWidth * Map.GetLength(1);
            }
        }

        public float Alpha
        {
            get { return Alpha; }
            set
            {
                alpha = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        float alpha = 1f;

        public TileLayer(int width, int height)
        {
            Map = new int[width, height];

            LayerWidth = width;
            LayerHeight = height;
        }

        public TileLayer(int[,] map)
        {
            Map = (int[,])map.Clone();

            LayerWidth = Map.GetLength(0);
            LayerHeight = Map.GetLength(1);
        }

        public static TileLayer FromFile(ContentManager content, string filename)
        {
            TileLayer layer = null;

            List<List<int>> tempLayout = new List<List<int>>();
            List<string> textureNames = new List<string>();
            bool readingTextures = false;
            bool readingLayout = false;

            using (StreamReader sr = new StreamReader(filename))
            {
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Textures]"))
                    {
                        readingTextures = true;
                        readingLayout = false;
                    }
                    else if (line.Contains("[Layout]"))
                    {
                        readingTextures = false;
                        readingLayout = true;
                    }
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();
                        string[] cells = line.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string c in cells)
                            row.Add(int.Parse(c));

                        tempLayout.Add(row);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            layer = new TileLayer(width, height);

            if (Engine.MapBounds.Width < layer.PixelWidth)
                Engine.MapBounds.Width = layer.PixelWidth;
            if (Engine.MapBounds.Height < layer.PixelHeight)
                Engine.MapBounds.Height = layer.PixelHeight;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    layer.SetCellIndex(new Point(x, y), tempLayout[y][x]);

            layer.LoadTileTextures(content, textureNames.ToArray());

            return layer;
        }

        public void LoadTileTextures(ContentManager content, params string[] textureNames)
        {
            TileTextures = new List<Animation>();

            foreach (string name in textureNames)
                TileTextures.Add(new Animation(content.Load<Texture2D>(name)));
        }

        public int GetCellIndex(Point p)
        {
            return Map[p.X, p.Y];
        }

        public void SetCellIndex(Point p, int index)
        {
            Map[p.X, p.Y] = index;
        }

        public void Draw(SpriteBatch sb, Point min, Point max)
        {
            sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, null, Camera.TransformMatrix);

            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);
            max.X = (int)Math.Min(max.X, LayerWidth);
            max.Y = (int)Math.Min(max.Y, LayerHeight);

            for (int y = min.Y; y < max.Y; y++)
            {
                for (int x = min.X; x < max.X; x++)
                {
                    int index = Map[x, y];
                    if (index == -1)
                        continue;

                    sb.Draw(
                        TileTextures[index].Texture,
                        new Rectangle(
                            x * Engine.TileWidth,
                            y * Engine.TileHeight,
                             Engine.TileWidth, Engine.TileHeight),
                        new Color(1, 1, 1, alpha));
                }
            }

            sb.End();
        }
    }
}
