using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TileEngine;
using Object = TileEngine.Object;

namespace SurvivorV3
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        FrameRateCounter fps;

        QuadTree<Sprite> quadtree;
        Pathfinder pathfinder;

        Camera camera;
        Dialog dialog;
        GUI gui;

        List<Sprite> renderList = new List<Sprite>();
        Comparison<Sprite> renderSort = new Comparison<Sprite>(RenderSpriteCompare);

        List<Zombie> zombies = new List<Zombie>();
        Player player;
        Item item;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Game.ScreenBounds.Width;
            graphics.PreferredBackBufferHeight = Game.ScreenBounds.Height;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gui = new GUI(this, Content);
            dialog = new Dialog(new Rectangle(20, 20, 200, 200), "Inventory");

            gui.Dialogs.Add("Inventory", dialog);

            Components.Add(gui);

            fps = new FrameRateCounter(this, Content);
            Components.Add(fps);

            dialog.Open();

            base.Initialize();

            camera = new Camera(Game.ScreenBounds.Width, Game.ScreenBounds.Height);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TileMap.Layers.Add(TileLayer.FromFile(Content, "Layers/Layer1"));
            TileMap.CollisionLayer = CollisionLayer.FromFile("Layers/Collisions");

            quadtree = new QuadTree<Sprite>(
                (x => x.Bounds),
                Engine.MapBounds);

            pathfinder = new Pathfinder();

            Game.PlayerAnims = new Dictionary<string, Animation>();
            Game.PlayerAnims.Add("Static", new Animation(
                Content.Load<Texture2D>("Sprites/PlayerStatic"),
                8, 0, false));

            player = new Player(
                Game.PlayerAnims["Static"],
                new Rectangle(32, 32, 32, 32),
                8, 4, "");

            Random r = new Random();
            for (int i = 0; i < 0; i++)
            {
                zombies.Add(new Zombie(
                    new Animation(Content.Load<Texture2D>("Sprites/Enemy")),
                    new Rectangle(r.Next(50, Engine.MapBounds.Width - 100), r.Next(50, Engine.MapBounds.Height - 100), 32, 32), 8, 3));

                quadtree.Add(zombies[i]);
            }

            Game.Items.Add("Golfclub", new Item(
                Content.Load<Texture2D>("Items/GolfClub"), 
                new Rectangle(300, 300, 16, 16), 
                5, 0, -1));

            //npcs.Add(new NPC(
            //        Game.PlayerAnims["Static"],
            //        new Rectangle(r.Next(50, 600), r.Next(50, 500), 32, 32),
            //        8, 4,
            //        Content.Load<Script>("Scripts/NPC1"), 
            //        dialog));
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.Update();
            renderList.Clear();

            if (InputHelper.IsKeyPressed(Keys.I))
                dialog.Enabled = !dialog.Enabled;

            //bool pressed = false;
            //if (InputHelper.IsKeyDown(Keys.W))
            //{
            //    player.Direction += new Vector2(0, -1.0f);
            //    pressed = true;
            //}
            //if (InputHelper.IsKeyDown(Keys.A))
            //{
            //    player.Direction += new Vector2(-1.0f, 0);
            //    pressed = true;
            //}
            //if (InputHelper.IsKeyDown(Keys.S))
            //{
            //    player.Direction += new Vector2(0, 1.0f);
            //    pressed = true;
            //}
            //if (InputHelper.IsKeyDown(Keys.D))
            //{
            //    player.Direction += new Vector2(1.0f, 0);
            //    pressed = true;
            //}

            if (InputHelper.IsMouseClicked(true) && !GUI.HasFocus)
            {
                player.Path = pathfinder.FindPath(
                    Engine.PositionToCell(player.Origin),
                    Engine.PositionToCell(new Vector2(InputHelper.MouseX, InputHelper.MouseY)));

                player.FollowPath = true;
                player.DisableTileCollisionChecks = true;
            }
            //else if (!player.FollowPath)
            //    player.DisableTileCollisionChecks = false;

            if (InputHelper.IsKeyDown(Keys.LeftShift))
                player.Speed = player.MaxSpeed;
            else if (InputHelper.IsKeyDown(Keys.LeftControl))
                player.Speed = player.MaxSpeed * 0.25f;
            else
                player.Speed = player.MaxSpeed * 0.5f;

            foreach (Sprite s in quadtree.GetItems(Camera.SafeBounds))
            {
                Zombie z = s as Zombie;
                z.Update(gameTime);
                z.HandleCollisions(player);
                z.ProcessSounds(player);
                z.ProcessSight(player);
                foreach (Zombie z2 in quadtree.GetItems(new Point((int)z.Origin.X, (int)z.Origin.Y)))
                    if (z != z2)
                        z.HandleCollisions(z2);

                if (Object.AreColliding(player, z))
                    player.Speed *= 0.4f;

                quadtree.UpdatePosition(s);
                renderList.Add(s);
            }

            player.Update(gameTime);
            player.Clamp();
            renderList.Add(player);

            camera.Lock(player);
            camera.Clamp();

            base.Update(gameTime);
        }

        static int RenderSpriteCompare(Sprite a, Sprite b)
        {
            return a.Origin.Y.CompareTo(b.Origin.Y);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            TileMap.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.TransformMatrix);

            renderList.Sort(renderSort);

            foreach (Item i in Game.Items.Values)
                i.Draw(spriteBatch);

            foreach (Sprite s in renderList)
                s.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
