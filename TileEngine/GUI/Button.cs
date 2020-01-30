using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Button
    {
        public Texture2D Image;
        public Texture2D Background;
        public Rectangle ImageBounds;
        public Rectangle Bounds;

        public Vector2 TextPosition;
        public string Text;
        public bool Highlighted;
        public bool Pressed;

        public Dialog Parent;
        public Point Offset;

        public Button(Texture2D image, Texture2D background, Rectangle bounds, Rectangle imageBounds, Color highlight, Color pressed, Vector2 textPos, string text)
        {
            Image = image;
            Background = background;
            Bounds = bounds;
            ImageBounds = imageBounds;

            TextPosition = textPos;
            Text = text;
        }

        public Button(Texture2D image, Texture2D background, Rectangle bounds, string text)
        {
            Image = image;
            Background = background;
            Bounds = bounds;
            ImageBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Height, Bounds.Height);

            Text = text;
        }

        public Button(Dialog parent, Texture2D image, Point size, Point offset)
        {
            Parent = parent;
            Image = image;
            Offset = offset;
            Bounds = new Rectangle(Parent.Bounds.X + Offset.X, Parent.Bounds.Y + Offset.Y, size.X, size.Y);
            ImageBounds = Bounds;
        }

        public Button(string text)
        {
            Text = text;
            TextPosition = Vector2.Zero;
        }

        public bool Update()
        {
            Highlighted = false;

            Bounds = new Rectangle(Parent.Bounds.X + Offset.X, Parent.Bounds.Y + Offset.Y, Bounds.Width, Bounds.Height);
            ImageBounds = new Rectangle(Parent.Bounds.X + Offset.X, Parent.Bounds.Y + Offset.Y, ImageBounds.Width, ImageBounds.Height);

            if (Bounds.Intersects(InputHelper.MouseBounds))
            {
                GUI.HasFocus = true;
                Highlighted = true;

                if (InputHelper.IsMouseHeldDown(true))
                {
                    Highlighted = false;
                    Pressed = true;
                }
                else if (InputHelper.IsMouseClicked(true))
                {
                    Pressed = false;
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch sb)
        {
            if (Background != null)
                sb.Draw(Background, Bounds, Color.White);

            if (Image != null)
                sb.Draw(Image, ImageBounds, Color.White);

            if (Text != null)
                sb.DrawString(GUI.SpriteFont, Text, TextPosition, GUI.TextColor);

            if (Highlighted)
                sb.Draw(GUI.BlankTexture, Bounds, GUI.HighlightColor);
            else if (Pressed)
                sb.Draw(GUI.BlankTexture, Bounds, GUI.PressedColor);
        }
    }
}
