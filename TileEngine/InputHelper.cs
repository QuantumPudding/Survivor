using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public static class InputHelper
    {
        public static int MouseX
        {
            get { return NMS.X - (int)Camera.TransformMatrix.Translation.X; }
        }
        public static int MouseY
        {
            get { return NMS.Y - (int)Camera.TransformMatrix.Translation.Y; }
        }

        public static Point MousePos
        {
            get { return new Point(MouseX, MouseY); }
        }
        public static Point LastMousePos;

        public static Rectangle MouseBounds
        {
            get
            {
                return new Rectangle(MouseX, MouseY, 1, 1);
            }
        }

        static KeyboardState NKS;
        static KeyboardState OKS;
        static MouseState NMS;
        static MouseState OMS;

        public static bool MouseHandled;

        public static void Update()
        {
            OKS = NKS;
            OMS = NMS;
            NKS = Keyboard.GetState();
            NMS = Mouse.GetState();

            LastMousePos = new Point(MouseX, MouseY);
        }

        public static bool IsMouseClicked(bool leftbutton)
        {
            if (MouseHandled)
                return false;

            int x = MouseX;
            int y = MouseY;
            if (x < 0 || x > Camera.ViewWidth || y < 0 || y > Camera.ViewHeight)
                return false;

            return leftbutton ?
                NMS.LeftButton == ButtonState.Released && OMS.LeftButton == ButtonState.Pressed :
                NMS.RightButton == ButtonState.Released && OMS.RightButton == ButtonState.Pressed;
        }

        public static bool IsMouseDown(bool leftbutton)
        {
            if (MouseHandled)
                return false;

            return leftbutton ?
                NMS.LeftButton == ButtonState.Pressed && OMS.LeftButton == ButtonState.Released :
                NMS.RightButton == ButtonState.Pressed && OMS.RightButton == ButtonState.Released;
        }

        public static bool IsMouseHeldDown(bool leftbutton)
        {
            if (MouseHandled)
                return false;

            return leftbutton ?
                NMS.LeftButton == ButtonState.Pressed :
                NMS.RightButton == ButtonState.Pressed;
        }

        public static bool IsMouseUp(bool leftbutton)
        {
            if (MouseHandled)
                return false;

            return leftbutton ?
                NMS.LeftButton == ButtonState.Released :
                NMS.RightButton == ButtonState.Released;
        }

        public static bool IsKeyPressed(Keys key)
        {
            return NKS.IsKeyDown(key) && OKS.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return NKS.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return NKS.IsKeyUp(key);
        }
    }
}
