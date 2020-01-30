using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using Microsoft.Xna.Framework;

namespace SurvivorV3
{
    class Zombie : NPC
    {
        public float SoundRadius;
        public float SightRadius;
        public float HalfFOV = 45;
        public float TargetNoise;
        public Vector2 TargetSource;

        public Zombie(Animation anim, Rectangle bounds, float radius, float speed)
            :base(anim, bounds, radius, speed, null, null)
        {
            SoundRadius = 400;
            SightRadius = 200;
            FollowTarget = true;
        }

        public Zombie(Animation anim, Rectangle bounds, float radius, float soundRadius, float sightRadius, float speed)
            :base(anim, bounds, radius, speed, null, null)
        {
            SoundRadius = soundRadius;
            SightRadius = sightRadius;
            FollowTarget = true;
        }

        public void ProcessSounds(Sprite s)
        {
            float distance = 0;
            if (Engine.RadiusCollision(Center, s.Center, SoundRadius, s.CollisionRadius, out distance))
            {
                float attenuatedSound = s.Noise * (1 / distance);

                if (attenuatedSound >= TargetNoise)
                {
                    TargetNoise = s.Noise;
                    TargetSource = s.Center;
                }
            }

            if (TargetNoise > 0.02f)
            {
                Direction = Engine.VectorNormalise(TargetSource - Center);

                ViewDirection = Direction;
                Speed = MaxSpeed * 0.33f;
            }

            if (Engine.VectorLength(TargetSource - Center) < Engine.TileWidth)
            {
                Direction = Vector2.Zero;
                TargetNoise = 0;
            }
        }

        public void ProcessSight(Sprite s)
        {
            if (Engine.RadiusCollision(Center, s.Center, SightRadius, s.CollisionRadius))
            {
                Vector2 d = Vector2.Normalize(s.Center - Center);

                float angle = MathHelper.ToDegrees((float)Math.Atan2(-(Center.X - s.Center.X), (Center.Y - s.Center.Y)));

                if (angle >= Rotation - HalfFOV && angle >= Rotation - HalfFOV)
                {
                    Target = s;
                    TargetSource = s.Center;
                    Speed = MaxSpeed;

                    if (Engine.LineIntersectsRect(Center, s.Center, s.Bounds))
                    {

                    }
                }
                else
                {
                    Target = null;
                }

            }
            else
            {
                Target = null;
            }
        }

        public override void Update(GameTime gt)
        {
            ViewDirection = Direction;
            base.Update(gt);
        }
    }
}
