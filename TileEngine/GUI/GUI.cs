using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TileEngine
{
    public class GUI : DrawableGameComponent
    {
        public static Color BackgroundColor = Color.FromNonPremultiplied(48, 48, 48, 255);
        public static Color BorderColor = Color.Black;
        public static Color TextColor = Color.White;
        public static Color HighlightColor = Color.FromNonPremultiplied(255, 255, 255, 128);
        public static Color PressedColor = Color.FromNonPremultiplied(128, 48, 48, 128);

        public static SpriteBatch SpriteBatch;
        public static SpriteFont SpriteFont;
        public static ContentManager Content;
        public static GraphicsDevice Graphics;

        public static Texture2D BlankTexture;
        public static Texture2D Cross;

        public static bool HasFocus;

        public Dictionary<string, Dialog> Dialogs = new Dictionary<string, Dialog>();

        public GUI(Game game, ContentManager content)
            : base(game)
        {
            Content = content;
            Initialize();
            
            BlankTexture = new Texture2D(Graphics, 1, 1);
            BlankTexture.SetData<Color>(new Color[] { Color.White });
        }

        public override void Initialize()
        {
            base.Initialize();
            Graphics = GraphicsDevice;
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteFont = Content.Load<SpriteFont>("Fonts/Tahoma");

            Cross = Content.Load<Texture2D>("Cross");
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Dialog d in Dialogs.Values)
                d.Update();
        }

        public void SetColors(Color background, Color border, Color text)
        {
            BackgroundColor = background;
            BorderColor = border;
            TextColor = text;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            foreach (Dialog d in Dialogs.Values)
                d.Draw(SpriteBatch);

            SpriteBatch.End();
        }

    }
}
