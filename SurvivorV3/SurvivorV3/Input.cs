using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using TileEngine;

namespace SurvivorV3
{
    public class Input
    {
        public static KeyboardState NKS;
        public static KeyboardState PKS;
        public static MouseState NMS;
        public static MouseState PMS;

        public static Rectangle MouseBounds;
        public static MouseButtonState ButtonState;
        public enum MouseButtonState
        {
            LeftDown,
            RightDown,
            LeftClick,
            RightClick,
            None
        }

        public static bool Clicked;

        private Sprite player;

        public Input(Sprite player)
        {
            this.player = player;
        }

        public void Update()
        {
            NKS = Keyboard.GetState();
            NMS = Mouse.GetState();
            ButtonState = MouseButtonState.None;
            Clicked = false;

            

            //if (NMS.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
            //    PMS.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //    ButtonState = MouseButtonState.LeftDown;

            //else if (NMS.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
            //    PMS.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //    ButtonState = MouseButtonState.RightDown;

            //else if (NMS.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
            //    PMS.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //    ButtonState = MouseButtonState.LeftClick;

            //else if (NMS.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
            //    PMS.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            //    ButtonState = MouseButtonState.RightClick;

            //if (ButtonState == MouseButtonState.LeftClick ||
            //    ButtonState == MouseButtonState.RightClick)
            //    Clicked = true;

            MouseBounds = new Rectangle(NMS.X, NMS.Y, 1, 1);
            PKS = NKS;
            PMS = NMS;
        }
    }
}
