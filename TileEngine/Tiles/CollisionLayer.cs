using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TileEngine
{
    public class CollisionLayer
    {
        public static int[,] Map;

        public int Width;
        public int Height;

        public CollisionLayer(int width, int height)
        {
            Map = new int[width, height];

            Width = width;
            Height = height;
        }

        public static CollisionLayer FromFile(string filename)
        {
            CollisionLayer layer = null;

            List<List<int>> tempLayout = new List<List<int>>();
            bool readingLayout = false;

            using (StreamReader sr = new StreamReader(filename))
            {
                while (sr.Peek() != -1)
                {
                    string line = sr.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
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

            layer = new CollisionLayer(width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    layer.SetCellIndex(new Point(x, y), tempLayout[y][x]);

            return layer;
        }

        public int GetCellIndex(Point p)
        {
            if (p.X < 0)
                p.X = 0;
            else if (p.X > Width - 1)
                p.X = Width - 1;

            if (p.Y < 0)
                p.Y = 0;
            else if (p.Y > Height - 1)
                p.Y = Height - 1;

            return Map[p.X, p.Y];
        }

        public void SetCellIndex(Point p, int index)
        {
            Map[p.X, p.Y] = index;
        }
    }
}
