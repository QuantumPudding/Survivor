using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Animation
    {
        public Texture2D Texture;
        public Rectangle SpriteRect;
        public Color OverlayColor = Color.White;

        public float FrameRate;
        public float FrameLength;
        public int FrameWidth
        {
            get { return SpriteRect.Width; }
        }
        public int FrameHeight
        {
            get { return SpriteRect.Height; }
        }
        public int FrameCount;
        public int LoopCount;

        public bool Animated;

        private float animTimer;
        private int currentFrame;

        public Animation(Animation a)
        {
            Texture = a.Texture;
            SpriteRect = a.SpriteRect;
            FrameCount = a.FrameCount;
            FrameRate = a.FrameRate;
            FrameLength = 1.0f / FrameRate;

            animTimer = 0.0f;
            Animated = true;
        }

        public Animation(Texture2D spritesheet, Rectangle spriteRect, int frameCount, float frameRate)
        {
            Texture = spritesheet;
            SpriteRect = spriteRect;
            FrameCount = frameCount;
            FrameRate = frameRate;
            FrameLength = 1.0f / FrameRate;

            animTimer = 0.0f;
            Animated = true;
        }

        public Animation(Texture2D spritesheet, Rectangle spriteRect, int frameCount, float frameRate, bool animated)
        {
            Texture = spritesheet;
            SpriteRect = spriteRect;
            FrameCount = frameCount;
            FrameRate = frameRate;
            FrameLength = 1.0f / FrameRate;

            animTimer = 0.0f;
            Animated = animated;
        }

        public Animation(Texture2D spritesheet, int frameCount, float frameRate, bool animated)
        {
            Texture = spritesheet;
            SpriteRect = new Rectangle(0, 0, Texture.Width / frameCount, Texture.Height);
            FrameCount = frameCount;
            FrameRate = frameRate;
            FrameLength = 1.0f / FrameRate;

            animTimer = 0.0f;
            Animated = animated;
        }

        public Animation(Texture2D texture)
        {
            Texture = texture;
            SpriteRect = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public virtual void Update(GameTime gt)
        {
            if (!Animated)
                return;

            animTimer += (float)gt.ElapsedGameTime.TotalSeconds;

            if (animTimer >= FrameLength)
            {
                animTimer = 0.0f;
                currentFrame = (currentFrame + 1) % FrameCount;
                SpriteRect.X = (SpriteRect.X + SpriteRect.Width) % Texture.Width;

                if (currentFrame == 0)
                    LoopCount = (LoopCount + 1) % int.MaxValue;
            }
        }

        public void SetFrame(int index)
        {
            currentFrame = index;
            if (index > FrameCount - 1)
                currentFrame = FrameCount - 1;
            else if (index < 0)
                currentFrame = 0;

            SpriteRect.X = SpriteRect.Width * currentFrame;
        }

        public Animation Clone()
        {
            return new Animation(Texture, SpriteRect, FrameCount, FrameRate);
        }
    }
}
