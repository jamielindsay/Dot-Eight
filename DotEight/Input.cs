using System;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;

namespace DotEight
{
    class Input
    {
        private static readonly Dictionary<int, Keyboard.Key> _keyMappings = new Dictionary<int, Keyboard.Key>()
        {
            { 1, Keyboard.Key.Num1 },
            { 2, Keyboard.Key.Num2 },
            { 3, Keyboard.Key.Num3 },
            { 0xC, Keyboard.Key.Num4 },
            { 4, Keyboard.Key.Q },
            { 5, Keyboard.Key.W },
            { 6, Keyboard.Key.E },
            { 0xD, Keyboard.Key.R },
            { 7, Keyboard.Key.A },
            { 8, Keyboard.Key.S },
            { 9, Keyboard.Key.D },
            { 0xE, Keyboard.Key.F },
            { 0xA, Keyboard.Key.Z },
            { 0, Keyboard.Key.X },
            { 0xB, Keyboard.Key.C },
            { 0xF, Keyboard.Key.V },
        };

        public static bool IsPressed(UInt16 key)
        {
            return Keyboard.IsKeyPressed(_keyMappings[key % 16]);
        }

        public static int WaitForKeyPress()
        {
            while (true)
            {
                for (UInt16 i = 0; i < _keyMappings.Count; i++)
                {
                    if (IsPressed(i))
                    {
                        return i;
                    }
                }
            }
        }
    }

}
