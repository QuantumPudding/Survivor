using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace SurvivorV3
{
    public class Level
    {
        public Level()
        {

        }
    }

    public class World
    {
        public Dictionary<string, Level> Levels;

        private ContentManager content;

        public World(ContentManager c)
        {
            content = c;
        }

        public void LoadLevel(string name)
        {

        }
    }
}
