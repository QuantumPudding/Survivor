using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Item : IItem
    {
        public Texture2D Texture { get; set; }

        public Rectangle Bounds { get; set; }

        public Object Parent { get; set; }

        public float Weight { get; set; }
        public float Rarity { get; set; }

        public bool CanPickup { get; set; }
        public bool Destroyed { get; set; }

        public int Uses { get; set; }

        public Item() { }

        public Item(Texture2D texture)
        {
            Texture = texture;
            CanPickup = true;
        }

        public Item(Texture2D texture, Rectangle bounds)
        {
            Texture = texture;
            Bounds = bounds;
            CanPickup = true;
        }

        public Item(Texture2D texture, Object parent)
        {
            Texture = texture;
            Parent = parent;
            CanPickup = true;
        }

        public Item(Texture2D texture, Rectangle bounds, float weight)
        {
            Texture = texture;
            Bounds = bounds;
            Weight = weight;
            CanPickup = true;
        }

        public Item(Texture2D texture, Object parent, float weight)
        {
            Texture = texture;
            Parent = parent;
            Weight = weight;
            CanPickup = true;
        }

        public Item(Texture2D texture, Rectangle bounds, float weight, float rarity)
        {
            Texture = texture;
            Bounds = bounds;
            Weight = weight;
            Rarity = rarity;
            CanPickup = true;
        }

        public Item(Texture2D texture, Object parent, float weight, float rarity)
        {
            Texture = texture;
            Parent = parent;
            Weight = weight;
            Rarity = rarity;
            CanPickup = true;
        }

        public Item(Texture2D texture, Rectangle bounds, float weight, float rarity, int uses)
        {
            Texture = texture;
            Bounds = bounds;
            Weight = weight;
            Rarity = rarity;
            Uses = uses;
            CanPickup = true;
        }

        public Item(Texture2D texture, Object parent, float weight, float rarity, int uses)
        {
            Texture = texture;
            Parent = parent;
            Weight = weight;
            Rarity = rarity;
            Uses = uses;
            CanPickup = true;
        }

        public virtual void Update()
        {

        }

        public virtual void Use()
        {
            if (Uses != -1)
                Uses--;

            if (Uses == 0)
                Destroyed = true;
        }

        public void Draw(SpriteBatch sb)
        {
            if (Parent == null)
                sb.Draw(Texture, Bounds, Color.White);
        }
    }

    public interface IItem
    {
        Texture2D Texture { get; set; }

        Rectangle Bounds { get; set; }

        Object Parent { get; set; }

        float Weight { get; set; }
        float Rarity { get; set; }

        bool CanPickup { get; set; }
        bool Destroyed { get; set; }

        int Uses { get; set; }
    }
}
