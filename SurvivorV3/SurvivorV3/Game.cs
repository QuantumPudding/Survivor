using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TileEngine;

namespace SurvivorV3
{
    public static class Game
    {
        //Game dimensions
        public static Rectangle ScreenBounds = new Rectangle(0, 0, 800, 600);

        //Background
        public static List<Animation> TileTextures;

        public static Dictionary<string, Animation> PlayerAnims;

        //Items
        public static Dictionary<string, Item> Items = new Dictionary<string,Item>();
    }
}
