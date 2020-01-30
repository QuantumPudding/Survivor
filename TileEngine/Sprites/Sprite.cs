using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Sprite : Object
    {
        public Animation Animation;

        public Sprite()
        {

        }

        public Sprite(Texture2D texture)
            : base(Vector2.Zero, texture.Width, texture.Height)
        {
            Animation = new Animation(texture);
        }

        public Sprite(Texture2D texture, Rectangle bounds)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height)
        {
            Animation = new Animation(texture);
        }

        public Sprite(Animation anim, Rectangle bounds)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height)
        {
            Animation = anim.Clone();
        }

        public Sprite(Texture2D texture, Rectangle bounds, float radius, float speed)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, radius, speed)
        {
            Animation = new Animation(texture);
        }

        public Sprite(Animation anim, Rectangle bounds, float radius, float speed)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, radius, speed)
        {
            Animation = anim.Clone();
        }

        public Sprite(Texture2D texture, Rectangle bounds, float radius, float speed, float targetRadius)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, radius, speed, targetRadius)
        {
            Animation = new Animation(texture);
        }

        public Sprite(Animation anim, Rectangle bounds, float radius, float speed, float targetRadius)
            : base(new Vector2(bounds.X, bounds.Y), bounds.Width, bounds.Height, radius, speed, targetRadius)
        {
            Animation = anim.Clone();
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            Animation.Update(gt);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Animation.Texture, DrawBounds, Animation.SpriteRect, Color.White);
        }
    }
}
