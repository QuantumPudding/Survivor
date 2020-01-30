using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TileEngine
{
    public class Dialog
    {
        public Texture2D Background;
        public Rectangle Bounds;
        public Rectangle TitleBounds;
        public Rectangle ExitBounds;

        public Button ExitButton;

        public string Title = "Dialog";
        public bool Enabled;

        public Dialog(Rectangle bounds, string title)
        {
            Bounds = bounds;
            Title = title;
            TitleBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, 24);

            List<Color> colors = new List<Color>();
            for (int y = 0; y < Bounds.Width; y++)
            {
                for (int x = 0; x < Bounds.Width; x++)
                {
                    if ((x < 2 || x > Bounds.Width - 3) || (y < 2 || y > Bounds.Height - 3))
                        colors.Add(GUI.BorderColor);
                    else
                        colors.Add(GUI.BackgroundColor);
                }
            }

            Background = new Texture2D(GUI.Graphics, Bounds.Width, Bounds.Height, false, SurfaceFormat.Color);
            Background.SetData<Color>(colors.ToArray());

            ExitButton = new Button(this, GUI.Cross, new Point(12, 12), new Point(Bounds.Width - 19, 7));

            Enabled = false;
        }

        Point mouseDownPos;
        public virtual void Update()
        {
            if (!Enabled)
                return;

            if (Bounds.Intersects(InputHelper.MouseBounds))
            {
                GUI.HasFocus = true;
                if (TitleBounds.Intersects(InputHelper.MouseBounds))
                {
                    if (InputHelper.IsMouseDown(true))
                    {
                        mouseDownPos = InputHelper.MousePos;
                    }
                    else if (InputHelper.IsMouseHeldDown(true))
                    {
                        Point mousePos = InputHelper.MousePos;

                        if (mousePos != mouseDownPos)
                        {
                            Bounds.X = mousePos.X + TitleBounds.X - mouseDownPos.X;
                            Bounds.Y = mousePos.Y + TitleBounds.Y - mouseDownPos.Y;
                            TitleBounds.X = Bounds.X;
                            TitleBounds.Y = Bounds.Y;
                        }

                        mouseDownPos = InputHelper.MousePos;
                    }
                }
            }
            else if (InputHelper.IsMouseUp(true))
            {
                GUI.HasFocus = false;
            }

            if (ExitButton.Update())
            {
                Enabled = false;
                return;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (!Enabled)
                return;

            sb.Draw(Background, Bounds, Color.FromNonPremultiplied(255, 255, 255, 198));
            sb.DrawString(GUI.SpriteFont, Title, new Vector2(Bounds.X + 8, Bounds.Y + 4), GUI.TextColor);
            ExitButton.Draw(sb);
        }

        protected string WrapText(string s)
        {
            string[] words = s.Split(' ');

            StringBuilder sb = new StringBuilder();
            float lineWidth = 0;
            float spaceWidth = GUI.SpriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = GUI.SpriteFont.MeasureString(word);

                if (lineWidth + size.X < Bounds.Width)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n " + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        public void Open()
        {
            Enabled = true;
        }

        public void Close()
        {
            Enabled = false;
        }
    }
}
