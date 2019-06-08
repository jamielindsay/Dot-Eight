using System;
using System.Collections.Generic;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace DotEight
{
    internal class Framebuffer
    {
        public RectangleShape[] Pixels { get; }

        public Framebuffer()
        {
            Pixels = new RectangleShape[2048];
            ClearScreen();
        }

        private static RectangleShape NewPixel(Vector2f position, byte red, byte green, byte blue)
        {
            RectangleShape pixel = new RectangleShape
            {
                Size = new Vector2f(20, 20),
                FillColor = new Color(red, green, blue),
                Position = position
            };
            return pixel;
        }

        public void ClearScreen()
        {
            int i = 0;
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    Pixels[i] = NewPixel(new Vector2f(x * 20, y * 20), 0, 0, 0);
                    i++;
                }
            }
        }

        public void TestDraw()
        {
            Pixels[100].FillColor = new Color(255, 0, 0);
            Pixels[101].FillColor = new Color(255, 255, 0);
            Pixels[102].FillColor = new Color(255, 0, 0);
            Pixels[103].FillColor = new Color(255, 0, 255);
        }
    }
}