using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    class Weapon : Item
    {
        public float Damage;

        public Weapon(Texture2D texture, float weight, float rarity, float damage, int uses)
            : base(texture, Rectangle.Empty, weight, rarity, uses)
        {
            Damage = damage;
        }

        public Weapon(Weapon w, Rectangle bounds)
            : base(w.Texture, bounds, w.Weight, w.Rarity, w.Uses)
        {
            Damage = w.Damage;
        }

        public Weapon(Weapon w, Object parent)
            : base(w.Texture, parent, w.Weight, w.Rarity, w.Uses)
        {
            Damage = w.Damage;
        }
    }
}
