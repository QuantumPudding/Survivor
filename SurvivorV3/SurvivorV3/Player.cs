using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SurvivorV3
{
    public class PlayerStats
    {
        public float Strength;
        public float Endurance;
        public float Agility;
        public float WillPower;
        public float Intelligence;

        public PlayerStats(float strength, float endurance, float agility, float willpower, float intelligence)
        {
            Strength = strength;
            Endurance = endurance;
            Agility = agility;
            WillPower = willpower;
            Intelligence = intelligence;
        }
    }

    public class Player : Sprite
    {
        public PlayerStats Stats;

        public Player(Animation anim, Rectangle bounds, float radius, float speed, string statsfile)
            :base(anim, bounds, radius, speed)
        {
            LoadStats(statsfile);
        }

        public void LoadStats(string filename)
        {
            Stats = new PlayerStats(5, 5, 5, 5, 5);
        }

        public override void Update(GameTime gt)
        {
            ViewDirection = Vector2.Normalize(
                new Vector2(InputHelper.MouseX, InputHelper.MouseY) - Center);

            if (Rotation == 180)
                Animation.SetFrame(0);
            else if (Rotation == -135)
                Animation.SetFrame(1);
            else if (Rotation == -90)
                Animation.SetFrame(2);
            else if (Rotation == -45)
                Animation.SetFrame(3);
            else if (Rotation == 0)
                Animation.SetFrame(4);
            else if (Rotation == 45)
                Animation.SetFrame(5);
            else if (Rotation == 90)
                Animation.SetFrame(6);
            else if (Rotation == 135)
                Animation.SetFrame(7);

            base.Update(gt);
        }
    }
}
