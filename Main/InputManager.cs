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

        public bool isLockedOnWrite = false;
        public IWritable writeTo = null;
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

            HandleTextInput();
        }

        public void LockOnWrite(IWritable write)
        {
            isLockedOnWrite = true;
            writeTo = write;
        }

        public void UnlockWrite()
        {
            isLockedOnWrite = false;
        }

        public void HandleTextInput()
        {
            if (writeTo == null) return;

            if (IsPressed(Keys.Back))
            {
                if (!string.IsNullOrEmpty(writeTo.stringInput))
                {
                    writeTo.stringInput = writeTo.stringInput.Substring(0, writeTo.stringInput.Length - 1);
                }
            }

            if (IsPressed(Keys.Delete))
            {
                writeTo.stringInput = "";
            }

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (IsPressed(key))
                {
                    string charStr = KeyToChar(key, _kbCurrent);
                    if (string.IsNullOrEmpty(charStr)) continue;

                    char c = charStr[0];

                    if (char.IsLetter(c) && writeTo.includeAlp)
                    {
                        writeTo.stringInput += c;
                    }
                    else if (char.IsDigit(c) && writeTo.includeNum)
                    {
                        writeTo.stringInput += c;
                    }
                    else if (!char.IsLetterOrDigit(c) && writeTo.includeAlp)
                    {
                        writeTo.stringInput += c;
                    }
                }
            }
        }

        private string KeyToChar(Keys key, KeyboardState kbState)
        {
            bool shift = kbState.IsKeyDown(Keys.LeftShift) || kbState.IsKeyDown(Keys.RightShift);

            if (key >= Keys.A && key <= Keys.Z)
            {
                char c = (char)('a' + (key - Keys.A));
                return shift ? c.ToString().ToUpper() : c.ToString();
            }

            if (key >= Keys.D0 && key <= Keys.D9)
            {
                if (shift)
                {
                    string[] shiftSymbols = new[] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")" };
                    return shiftSymbols[key - Keys.D0];
                }
                return ((char)('0' + (key - Keys.D0))).ToString();
            }

            if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
            {
                return ((char)('0' + (key - Keys.NumPad0))).ToString();
            }

            if (key == Keys.Space) return " ";
            if (key == Keys.OemPeriod) return ".";
            if (key == Keys.OemComma) return ",";
            if (key == Keys.OemQuestion) return "?";
            if (key == Keys.OemSemicolon) return ";";
            if (key == Keys.OemQuotes) return "'";
            if (key == Keys.OemOpenBrackets) return shift ? "{" : "[";
            if (key == Keys.OemCloseBrackets) return shift ? "}" : "]";
            if (key == Keys.OemPlus) return shift ? "+" : "=";
            if (key == Keys.OemMinus) return shift ? "_" : "-";
            if (key == Keys.OemBackslash) return shift ? "|" : "\\";

            return "";
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
        public Vector2 GetMousePosition() => new Vector2(_msCurrent.X, _msCurrent.Y);

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