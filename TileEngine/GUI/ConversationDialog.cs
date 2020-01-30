using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class ConversationDialog : Dialog
    {
        public Conversation Conversation = null;
        public NPC Npc = null;

        public bool FirstFrame = true;

        int currentHandler = 0;

        public ConversationDialog(Game game, ContentManager content)
            : base(new Rectangle(0, 0, 600, 400), "")
        {
            Enabled = false;
        }

        public override void Update()
        {
            if (Conversation != null && Npc != null)
            {
                if (!FirstFrame)
                {
                    if (InputHelper.IsKeyPressed(Keys.Up))
                    {
                        //Possible bug
                        currentHandler = (currentHandler - 1) % Conversation.Handlers.Count;
                    }
                    if (InputHelper.IsKeyPressed(Keys.Down))
                    {
                        currentHandler = (currentHandler + 1) % Conversation.Handlers.Count;
                    }

                    if (InputHelper.IsKeyPressed(Keys.Space))
                    {
                        Conversation.Handlers[currentHandler].Invoke(Npc);
                        currentHandler = 0;
                    }
                }
                else FirstFrame = false;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Conversation == null)
                return;

            Rectangle dest = new Rectangle(
                GUI.Graphics.Viewport.Width / 2 - Bounds.Width / 2,
                GUI.Graphics.Viewport.Height / 2 - Bounds.Height / 2,
                Bounds.Width, Bounds.Height);

            sb.Draw(Background, dest, new Color(0, 0, 0, 100));

            string fulltext = WrapText(Conversation.Text);

            sb.DrawString(
                GUI.SpriteFont,
                fulltext,
                new Vector2(dest.X, dest.Y),
                Color.White);

            int lineheight = (int)GUI.SpriteFont.MeasureString(" ").Y;
            int startingHeight = (int)GUI.SpriteFont.MeasureString(fulltext).Y + lineheight;

            for (int i = 0; i < Conversation.Handlers.Count; i++)
            {
                string caption = Conversation.Handlers[i].Caption;
                Color color = (i == currentHandler) ? Color.Yellow : Color.White;

                sb.DrawString(
                    GUI.SpriteFont,
                    caption,
                    new Vector2(dest.X, dest.Y + startingHeight + (i * lineheight)),
                    color);
            }
        }
    }
}
