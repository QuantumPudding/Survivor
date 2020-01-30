using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Object
    {
        public Vector2 Origin 
        {
            get
            {
                return Position + OriginOffset;
            }
        }
        public Vector2 Center 
        {
            get
            {
                return new Vector2(
                    Position.X + (Bounds.Width / 2),
                    Position.Y + (Bounds.Height / 2));
            }
        }
        
        public Rectangle Bounds 
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Width,
                    HalfHeight);
            }
        }

        public Rectangle DrawBounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y - HalfHeight,
                    Width,
                    Height);
            }
        }

        public Vector2 Position;
        public Vector2 Direction;
        public Vector2 ViewDirection;
        public Vector2 OriginOffset;

        public List<Vector2> Path;

        public Object Target;

        public float CollisionRadius;
        public float TargetRadius;
        public float Rotation;
        public float MaxSpeed;
        public float Speed;
        public float Noise;
        public float NoiseThreshold;
        public float PathDeviation;

        public bool Moveable;
        public bool FollowTarget;
        public bool FollowPath;
        public bool RepeatPath;
        public bool DisableTileCollisionChecks;

        public int Width;
        public int Height;
        public int HalfWidth;
        public int HalfHeight;
        public int PathIndex;

        public Object()
        {
            Moveable = true;
        }

        public Object(Vector2 position, int width, int height)
        {
            Width = width;
            Height = height;
            HalfWidth = width / 2;
            HalfHeight = height / 2;
            Position = position;
            OriginOffset = new Vector2(Width / 2, Height);
            CollisionRadius = Width / 2;
            Moveable = true;
            TargetRadius = CollisionRadius;
            PathDeviation = MaxSpeed;
        }

        public Object(Vector2 position, int width, int height, float collisionRadius, float maxspeed)
        {
            Width = width;
            Height = height;
            HalfWidth = width / 2;
            HalfHeight = height / 2;
            Position = position;
            OriginOffset = new Vector2(Width / 2, Height);
            CollisionRadius = collisionRadius;
            MaxSpeed = maxspeed;
            Moveable = true;
            TargetRadius = CollisionRadius;
            PathDeviation = MaxSpeed;
        }

        public Object(Vector2 position, int width, int height, float collisionRadius, float maxspeed, float targetRadius)
        {
            Width = width;
            Height = height;
            HalfWidth = width / 2;
            HalfHeight = height / 2;
            Position = position;
            OriginOffset = new Vector2(Width / 2, Height);
            CollisionRadius = collisionRadius;
            MaxSpeed = maxspeed;
            Moveable = true;
            TargetRadius = targetRadius;
            PathDeviation = MaxSpeed;
        }

        public virtual void Update(GameTime gt)
        {
            if (!Moveable)
                return;

            //Follow target
            if (Target != null && FollowTarget)
            {
                if (!Engine.RadiusCollision(Center, Target.Center, TargetRadius, Target.TargetRadius))
                    Direction = Target.Center - Center;
            }
            //Follow path
            else if (FollowPath && Path.Count > 0)
            {
                if (PathIndex >= Path.Count - 1)
                {
                    Direction = Vector2.Zero;
                    PathIndex = 0;
                    if (!RepeatPath)
                        FollowPath = false;
                }
                else
                {
                    if (Engine.Distance(Path[PathIndex], Center) <= Speed)
                    {
                        PathIndex++;
                    }
                    else
                    {
                        Direction = Path[PathIndex] - Center;
                    }
                }
            }

            //Move
            if (Direction != Vector2.Zero)
            {
                Direction = Engine.VectorNormalise(Direction);
                Position += Direction * Speed;
            }

            //Tile collision check
            if (!DisableTileCollisionChecks)
                CheckForUnpassableTiles();

            //Get rotation
            Rotation = (float)Math.Atan2(ViewDirection.X, -ViewDirection.Y);
            Rotation = MathHelper.PiOver4 * (float)Math.Round(Rotation / MathHelper.PiOver4);
            Rotation = MathHelper.ToDegrees(Rotation);

            if (Rotation == -180)
                Rotation = 180;

            //Prepare speed for next update call
            Speed *= 0.99f;
        }

        public void CheckTilesForMotionChanges()
        {
            Point cell = Engine.PositionToCell(Origin);

            int index = TileMap.CollisionLayer.GetCellIndex(cell);

            if (index == 2)
            {

            }
        }

        public void CheckForUnpassableTiles()
        {
            if (TileMap.CollisionLayer == null)
                return;

            Point cell = Engine.PositionToCell(Center);
            int w = TileMap.CollisionLayer.Width - 1;
            int h = TileMap.CollisionLayer.Height - 1;

            Point? ul, u, ur, l, r, dl, d, dr;
            ul = u = ur = l = r = dl = d = dr = null;

            if (cell.Y > 0)
                u = new Point(cell.X, cell.Y - 1);

            if (cell.Y < h)
                d = new Point(cell.X, cell.Y + 1);

            if (cell.X > 0)
                l = new Point(cell.X - 1, cell.Y);

            if (cell.X < w)
                r = new Point(cell.X + 1, cell.Y);

            if (cell.X > 0 && cell.Y > 0)
                ul = new Point(cell.X - 1, cell.Y - 1);

            if (cell.X < w && cell.Y > 0)
                ur = new Point(cell.X + 1, cell.Y - 1);

            if (cell.X > 0 && cell.Y < h)
                dl = new Point(cell.X - 1, cell.Y + 1);

            if (cell.X < w && cell.Y < h)
                dr = new Point(cell.X + 1, cell.Y + 1);

            if (u != null && TileMap.CollisionLayer.GetCellIndex(u.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(u.Value);
                if (cellRect.Intersects(Bounds))
                    Position.Y = cell.Y * Engine.TileHeight;
            }
            if (d != null && TileMap.CollisionLayer.GetCellIndex(d.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(d.Value);
                if (cellRect.Intersects(Bounds))
                    Position.Y = d.Value.Y * Engine.TileHeight - Bounds.Height;
            }
            if (l != null && TileMap.CollisionLayer.GetCellIndex(l.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(l.Value);
                if (cellRect.Intersects(Bounds))
                    Position.X = cell.X * Engine.TileWidth;
            }       
            if (r != null && TileMap.CollisionLayer.GetCellIndex(r.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(r.Value);
                if (cellRect.Intersects(Bounds))
                    Position.X = r.Value.X * Engine.TileWidth - Bounds.Width;
            }
            

            if (ul != null && TileMap.CollisionLayer.GetCellIndex(ul.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(ul.Value);
                if (cellRect.Intersects(Bounds))
                {
                    Position.X = cell.X * Engine.TileWidth;
                    Position.Y = cell.Y * Engine.TileHeight;
                }
            }

            if (ur != null && TileMap.CollisionLayer.GetCellIndex(ur.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(ur.Value);
                if (cellRect.Intersects(Bounds))
                {
                    Position.X = r.Value.X * Engine.TileWidth - Bounds.Width;
                    Position.Y = cell.Y * Engine.TileHeight;
                }
            }

            if (dl != null && TileMap.CollisionLayer.GetCellIndex(dl.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(dl.Value);
                if (cellRect.Intersects(Bounds))
                {
                    Position.X = cell.X * Engine.TileWidth;
                    Position.Y = cell.Y * Engine.TileHeight;
                }
            }

            if (dr != null && TileMap.CollisionLayer.GetCellIndex(dr.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(dr.Value);
                if (cellRect.Intersects(Bounds))
                {
                    Position.X = r.Value.X * Engine.TileWidth - Bounds.Width;
                    Position.Y = d.Value.Y * Engine.TileHeight - Bounds.Height;
                }
            }
        }

        public static bool AreColliding(Object a, Object b, out Vector2 d)
        {
            d = b.Origin - a.Origin;
            return Engine.VectorLength(d) < a.CollisionRadius + b.CollisionRadius;
        }

        public static bool AreColliding(Object a, Object b)
        {
            return Engine.VectorLength(b.Origin - a.Origin) < a.CollisionRadius + b.CollisionRadius;
        }

        public void HandleCollisions(Object o)
        {
            Vector2 d;
            if (AreColliding(this, o, out d))
            {
                d.Normalize();
                Position = o.Position - (d * (CollisionRadius + o.CollisionRadius));
            }
        }

        public void Clamp()
        {
            if (Position.X < 0)
                Position = new Vector2(0, Position.Y);
            else if (Position.X > Engine.MapBounds.Width - Width)
                Position = new Vector2(Engine.MapBounds.Width - Width, Position.Y);

            if (Position.Y < 0)
                Position = new Vector2(Position.X, 0);
            else if (Position.Y > Engine.MapBounds.Height - Height)
                Position = new Vector2(Position.X, Engine.MapBounds.Height - Height);
        }

        public void Clamp(Rectangle bounds)
        {
            if (Position.X < bounds.X)
                Position = new Vector2(bounds.X, Position.Y);
            else if (Position.X > bounds.Width)
                Position = new Vector2(bounds.Width - Width, Position.Y);

            if (Position.Y < bounds.Y)
                Position = new Vector2(Position.X, bounds.Y);
            else if (Position.Y > bounds.Height)
                Position = new Vector2(Position.X, bounds.Height - Height);
        }
    }

    //public interface IObject
    //{
    //    Vector2 Position { get; set; }
    //    Vector2 Direction { get; set; }
    //    Vector2 ViewDirection { get; set; }
    //    Vector2 OriginOffset { get; set; }
    //    Vector2 Origin { get; }
    //    Vector2 Center { get; }

    //    Object Target { get; set; }

    //    Rectangle Bounds { get; set; }

    //    float CollisionRadius { get; set; }
    //    float TargetRadius { get; set; }
    //    float Rotation { get; set; }
    //    float MaxSpeed { get; set; }
    //    float Speed { get; set; }
    //    float Noise { get; set; }
    //    float NoiseThreshold { get; set; }

    //    bool Moveable { get; set; }
    //    bool FollowTarget { get; set; }
    //}
}
