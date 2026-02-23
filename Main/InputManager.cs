using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RedPaint
{
    public class InputManager
    {
        Maincode mc;

        private KeyboardState _kbCurrent;
        private KeyboardState _kbPrevious;

        private MouseState _msCurrent;
        private MouseState _msPrevious;

        public InputManager(Maincode imc)
        {
            mc = imc;

            _kbPrevious = Keyboard.GetState();
            _kbCurrent = Keyboard.GetState();
            _msPrevious = Mouse.GetState();
            _msCurrent = Mouse.GetState();
        }

        public void Update()
        {
            _kbPrevious = _kbCurrent;
            _kbCurrent = Keyboard.GetState();

            _msPrevious = _msCurrent;
            _msCurrent = Mouse.GetState();
        }

        public bool IsDown(Keys key) => _kbCurrent.IsKeyDown(key);
        public bool IsDown(Button button) => GetButtonState(_msCurrent, button) == ButtonState.Pressed;

        public bool IsPressed(Keys key) =>
            _kbCurrent.IsKeyDown(key) && _kbPrevious.IsKeyUp(key);

        public bool IsPressed(Button button) =>
            GetButtonState(_msCurrent, button) == ButtonState.Pressed &&
            GetButtonState(_msPrevious, button) == ButtonState.Released;

        public bool IsReleased(Keys key) =>
            _kbCurrent.IsKeyUp(key) && _kbPrevious.IsKeyDown(key);

        public bool IsReleased(Button button) =>
            GetButtonState(_msCurrent, button) == ButtonState.Released &&
            GetButtonState(_msPrevious, button) == ButtonState.Pressed;


        private ButtonState GetButtonState(MouseState state, Button button)
        {
            return button switch
            {
                Button.LeftButton => state.LeftButton,
                Button.RightButton => state.RightButton,
                Button.MiddleButton => state.MiddleButton,
                Button.XButton1 => state.XButton1,
                Button.XButton2 => state.XButton2,
                _ => ButtonState.Released
            };
        }
        public Point GetMousePosition() => new Point(_msCurrent.X, _msCurrent.Y);

        public int GetMouseWheelValue() => _msCurrent.ScrollWheelValue;

        public bool IsMouseWheelScrolledUp() =>
            _msCurrent.ScrollWheelValue > _msPrevious.ScrollWheelValue;

        public bool IsMouseWheelScrolledDown() =>
            _msCurrent.ScrollWheelValue < _msPrevious.ScrollWheelValue;
    }
    public enum Button
    {
        LeftButton,
        RightButton,
        MiddleButton,
        XButton1,
        XButton2
    }
}